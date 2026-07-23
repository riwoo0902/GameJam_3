using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    public class NavAgent2D : MonoBehaviour, INavAgent2D
    {
        [SerializeField] private NavMeshBakeDataSo navMeshData;
        [SerializeField] private float speed;
        [SerializeField] private float stoppingDistance = 0.3f;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isCalculating;
        private Vector2 _lastDestination;
        
        private readonly Vector2[] _waypoints = new Vector2[128]; //최대 128개의 폴리곤까지 지날 수 있음.
        private int _wayPointCount;
        private int _wayPointIndex;
        
        public bool PathPending => _isCalculating;
        public bool HasPath { get; private set; }
        public bool IsPathStale { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsPartialPath { get; private set;  } // 목적지까지 갈 수 없어 일부 경로만 반환된경우

        public float StoppingDistance
        {
            get => stoppingDistance;
            set => stoppingDistance = Mathf.Max(0, value);
        }

        public float Speed
        {
            get => speed;
            set => speed = Mathf.Max(0, value); //음수는 못들어가게
        }

        public float RemainingDistance
        {
            get
            {
                if (_isCalculating) return float.PositiveInfinity;
                if (!HasPath || _wayPointIndex >= _wayPointCount) return 0; //이미 도착했거나 경로가 없다면 0
                
                float dist = Vector2.Distance(transform.position, _waypoints[_wayPointIndex]);
                for(int i = _wayPointIndex; i < _wayPointCount - 1; i++)
                    dist += Vector2.Distance(_waypoints[i], _waypoints[i + 1]);
                
                return dist;
            }
        }

        public void InvalidatePath()
        {
            if(HasPath) IsPathStale = true; 
        }

        public async void SetDestination(Vector2 destination)
        {
            ResetPath();

            int count = await GetPath((Vector2)transform.position, destination, _waypoints);
            if (count <= 1) return;

            _wayPointCount = count;
            _wayPointIndex = 1; //0번은 이미 출발지점이라 내가 있다.
            IsMoving = true;
        }

        private void ResetPath()
        {
            if(_isCalculating) _cts?.Cancel();
            IsMoving = false;
            _wayPointCount = 0;
            _wayPointIndex = 0;
            HasPath = false;
            IsPathStale = false;
            IsPartialPath = false;
        }

        private void Update()
        {
            if (!IsMoving || _wayPointIndex >= _wayPointCount) return;
            
            Vector2 current = transform.position;
            float step = Speed * Time.deltaTime;
            
            //정밀 정지 : 이동하기전에 남은 거리를 보고 StoppingDistance 경계를 넘지 않도록 이번 프레임의 이동량을 제한

            if (stoppingDistance > 0f)
            {
                float allowed = RemainingDistance - stoppingDistance; //더 갈 수 있는 거리
                if (allowed <= 0f)
                {
                    IsMoving = false;
                    return;
                }

                if (step >= allowed) //이번 프레임에 도달 예정
                {
                    Vector2 dir = (_waypoints[_wayPointIndex] - current).normalized;
                    transform.position = current + dir * allowed; //허가된 만큼만 이동
                    IsMoving = false;
                    return;
                }
            }
            
            Vector2 target = _waypoints[_wayPointIndex];
            Vector2 next = Vector2.MoveTowards(current, target, step);
            transform.position = next;

            if ((next - target).sqrMagnitude < 0.001f)
            {
                _wayPointIndex++;
                if (_wayPointIndex >= _wayPointCount)
                {
                    IsMoving = false;
                }
            }
        }

        // private void LateUpdate()
        // {
        //     if(IsMoving && stoppingDistance > 0f && RemainingDistance <= stoppingDistance)
        //         IsMoving = false;
        // }

        #region Compute path
        
        //Phase1 : AstarPolygon을 구하는 전체 뼈대 작성
        private (List<NavPolygon> path, bool isPartial) AstarPolygons(NavPolygon start, NavPolygon end,
            CancellationToken ct)
        {
            AStarNode startNode = new AStarNode(start) { G = 0, F = CalcH(start, end) };

            MinHeap<AStarNode> openList = new MinHeap<AStarNode>((a, b) => a.F.CompareTo(b.F));
            HashSet<int> closedSet = new HashSet<int>(); //탐색이 끝난 폴리곤들의 id 저장.
            
            openList.Push(startNode);

            AStarNode bestNode = startNode;
            float bestH = CalcH(start, end);

            while (openList.Count > 0)
            {
                if(ct.IsCancellationRequested) return (null, false);

                AStarNode current = openList.Pop(); //오픈리스트에서 가장 유망한 것을 뽑는다.
                closedSet.Add(current.polygon.id); //방문한 폴리곤은 클로즈 리스트로 들어간다.

                if (current.polygon == end)
                    return (ReconstructPolyPath(current), false); //목적지 도달했다면 경로 만들어서 반납

                float h = CalcH(current.polygon, end);
                if (h < bestH)
                {
                    bestH = h; 
                    bestNode = current;
                }

                foreach (PortalData portal in current.polygon.portals)
                {
                    if(closedSet.Contains(portal.neighborId)) continue; //포탈을 통해 연결된 폴리곤이 이미 방문되어있다면 취소
                    if(!navMeshData.TryGetPolygon(portal.neighborId, out NavPolygon neighbor))
                        continue; //해당 폴리곤이 존재하지 않으면 취소(근데 이럴 일은 없다.)

                    //실제 이동경로 재계산
                    float newG = current.G + Vector2.Distance(current.polygon.center, neighbor.center);
                    
                    //이미 존재한다면
                    AStarNode existing = openList.Find(n => n.polygon.id == neighbor.id);
                    if (existing != null)
                    {
                        if (newG < existing.G)
                        {
                            existing.G = newG;
                            existing.F = newG + CalcH(neighbor, end);
                            existing.parent = current;
                            openList.DecreaseKey(existing); //힙을 재계산.
                        }
                    }
                    else
                    {
                        openList.Push(new AStarNode(neighbor)
                        {
                            G = newG, F = newG + CalcH(neighbor, end), parent = current
                        });
                    }
                }
            }
            //오픈리스트를 전부 소진했음에도 end에 도달하지 못한경우야. 그럼 bestNode까지의 경로만 반환하고
            //partial을 true설정한다.
            return (ReconstructPolyPath(bestNode), true);
        }

        private List<NavPolygon> ReconstructPolyPath(AStarNode end)
        {
            List<NavPolygon> path = new List<NavPolygon>();
            for(AStarNode n = end; n != null; n = n.parent)
                path.Add(n.polygon);
            path.Reverse();
            return path;
        }

        private float CalcH(NavPolygon a, NavPolygon b)
            => Vector2.Distance(a.center, b.center);
        
        
        //Phase2 : 포탈을 방향성을 가지도록 추출한다. 

        private static List<(Vector2 left, Vector2 right)> ExtractOrientedPortals(List<NavPolygon> polyPath)
        {
            var portals = new List<(Vector2 left, Vector2 right)>(polyPath.Count - 1);

            for (int i = 0; i < polyPath.Count - 1; i++)
            {
                NavPolygon from = polyPath[i];
                NavPolygon to = polyPath[i + 1]; //경로를 이용해 방향을 구한다.

                PortalData? found = null;
                foreach (PortalData p in from.portals)
                {
                    if (p.neighborId == to.id)
                    {
                        found = p;
                        break;
                    }
                }
                if(found == null) continue;

                Vector2 pA = found.Value.pointA;
                Vector2 pB = found.Value.pointB;
                Vector2 dir = to.center - from.center;
                Vector2 mid = (pA + pB) * 0.5f;
                
                float cross = dir.x * (pA.y - mid.y) - dir.y * (pA.x - mid.x);
                portals.Add(cross >= 0f ? (pA, pB) : (pB, pA));
            }

            return portals;
        }
        
        public enum PathStatus {Complete, Partial, Invalid}
        
        //비동기 경로 탐색을 시작합니다. waypoint를 pointArr에 채우고 갯수를 반환하는 함수
        public async Task<int> GetPath(Vector2 start, Vector2 destination, Vector2[] pointArr)
        {
            if(_isCalculating)
                _cts?.Cancel(); //기존 계산중이던거 취소 날리고

            if (HasPath && (destination - _lastDestination).sqrMagnitude > 0.001f)
                IsPathStale = true;

            if (_cts is null or { IsCancellationRequested: true })
                _cts = new CancellationTokenSource(); 
            
            CancellationToken token = _cts.Token;

            try
            {
                _isCalculating = true;
                //WebGL 싱글 스레드 
                Task<(List<Vector2>, PathStatus)> pathTask;

#if UNITY_WEBGL
                    pathTask = Task.FromResult(CalculatePath(start, destination, token));
#else
                pathTask = Task.Run(() => CalculatePath(start, destination, token));
#endif

                (List<Vector2> waypoints, PathStatus status) = await pathTask;

                int count = 0;
                if (status != PathStatus.Invalid)
                {
                    for (int i = 0; i < waypoints.Count && i < pointArr.Length; i++, count++)
                    {
                        pointArr[i] = waypoints[i];
                    }

                    HasPath = true;
                    IsPathStale = false;
                    IsPartialPath = status == PathStatus.Partial;
                    _lastDestination = destination;
                }
                else
                {
                    HasPath = false;
                    IsPathStale = false;
                    IsPartialPath = false;
                }

                return status != PathStatus.Invalid ? count : -1;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                HasPath = false;
                IsPathStale = false;
                return -1;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        private (List<Vector2> path, PathStatus status) CalculatePath(Vector2 start, Vector2 end, CancellationToken ct)
        {
            bool startOutSide = !navMeshData.GetPolygonAt(start, out NavPolygon startPoly);
            if (startOutSide)
            {
                if(!navMeshData.GetNearestPolygon(start, out startPoly))
                    return (null, PathStatus.Invalid);
            }

            bool destOutside = !navMeshData.GetPolygonAt(end, out NavPolygon endPoly);
            if (destOutside)
            {
                if(!navMeshData.GetNearestPolygon(end, out endPoly))
                    return (null, PathStatus.Invalid);
            }

            //시작 폴리곤과 도착 폴리곤이 같다면 이건 같은 폴리곤안에 있으니 직선이동 하면 그만.
            if (startPoly == endPoly)
            {
                Vector2 effectiveEnd = destOutside ? ClosestPointOnPolygon(endPoly.vertices, end) : end;
                PathStatus samePolyStatus = destOutside ? PathStatus.Partial : PathStatus.Complete;
                return (new List<Vector2> {start, effectiveEnd }, samePolyStatus);
            }
            
            //Phase 1, 폴리곤 경로를 뽑는다.
            (List<NavPolygon> polyPath, bool isPartialAStar) = AstarPolygons(startPoly, endPoly, ct);
            //Phase 2, 포탈 추출
            List<(Vector2, Vector2)> portals = ExtractOrientedPortals(polyPath); //구해진 경로 기반으로 방향성을 지닌 포탈 생성
            
            //부분경로가 나왔다면 목적지에 가장 가까운 점을 종착점으로 변경한다.
            bool isPartial = isPartialAStar || destOutside;
            Vector2 funnelEnd = isPartial ? ClosestPointOnPolygon(polyPath[^1].vertices, end) : end;
            
            //Phase3 Funnel 적용하여 실제적인 wayPoint 계산
            List<Vector2> wayPoints = Funnel.StringPull(start, funnelEnd, portals);
            return (wayPoints, isPartial ? PathStatus.Partial : PathStatus.Complete);
        }
        //쉬는 시간 40분까지. 

        private Vector2 ClosestPointOnPolygon(Vector2[] verts, Vector2 point)
        {
            bool inside = true;
            for (int i = 0, j = verts.Length - 1; i < verts.Length; j = i++)
            {
                float cross = (verts[i].x - verts[j].x) * (point.y - verts[j].y)
                    - (verts[i].y - verts[j].y) * (point.x - verts[j].x); //외적식

                if (cross < 0f)
                {
                    inside = false;
                    break;
                }
            }

            if (inside) return point; //점이 폴리곤 안에 있다면 걍 그점으로 가면 돼.

            float bestSqr = float.MaxValue;
            Vector2 best = verts[0];
            for (int i = 0, j = verts.Length - 1; i < verts.Length; j = i++)
            {
                Vector2 closest = ClosetPointOnSegment(verts[j], verts[i], point);
                float sqr = (closest - point).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    best = closest;
                }
            }

            return best;
        }

        private Vector2 ClosetPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 ab = b - a;
            float t = Vector2.Dot(p - a, ab) / Vector2.Dot(ab, ab);
            t = Mathf.Clamp01(t);
            return a + ab * t;
        }

        #endregion
    }
}