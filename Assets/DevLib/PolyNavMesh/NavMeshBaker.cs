#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DevLib.PolyNavMesh
{
    
    /// <summary>
    /// 축소후 int가 아닌 float형태로 나타낼 사각형
    /// </summary>
    public struct WorldRect
    {
        public float xMin, yMin, xMax, yMax;
    }
    public class NavMeshBaker : MonoBehaviour
    {
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private NavMeshBakeDataSo bakeData;

        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color polygonColor = new Color(0.2f, 0.8f, 0.2f, 0.4f);
        [SerializeField] private Color portalColor = Color.yellow;
        [SerializeField] private Color centerColor = Color.green;

        [ContextMenu("Bake nav mesh")]
        private void Bake()
        {
            Debug.Assert(groundMap != null, "그라운드 맵이 없습니다.");
            Debug.Assert(obstacleMap != null, "장애물 맵이 없습니다.");
            Debug.Assert(bakeData != null, "베이크 데이터 객체가 없습니다.");
            
            bakeData.Clear(); //모든 베이크 데이터 지워주고

            HashSet<Vector3Int> walkable = CollectWalkableCells();
            //2. 워커블들을 Rect로 머지
            List<RectInt> rects = MergeIntoRectangles(walkable);
            //3. 경계에 맞게 다시 Rect를 쪼개기
            rects = SplitRectsAtWallBoundaries(rects, walkable);
            //4. 폴리곤으로 만들면서 Shrink 시키기
            BuildPolygons(rects, walkable);
            //5. 런타임맵 만들기
            bakeData.BuildRuntimeMap();

            Debug.Log($"[PolyNavMesh] 베이크 완료 : {walkable.Count}개의 블록으로부터 {bakeData.polygons.Count}개 폴리곤이 제작됨");
            
            #if UNITY_EDITOR
            EditorUtility.SetDirty(bakeData);
            AssetDatabase.SaveAssets();
            #endif
        }

        private HashSet<Vector3Int> CollectWalkableCells()
        {
            HashSet<Vector3Int> walkable = new HashSet<Vector3Int>();
            groundMap.CompressBounds();
            
            BoundsInt bounds = groundMap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y);
                    if (!obstacleMap.HasTile(cell) && groundMap.HasTile(cell))
                        walkable.Add(cell);
                }
            }

            return walkable;
        }
        
        private List<RectInt> MergeIntoRectangles(HashSet<Vector3Int> walkable)
        {
            if (walkable.Count == 0) return new List<RectInt>();
            
            //워커블 셀들을 순회하면서 각 셀의 x, y축의 상한과 하한을 알아낸다.
            int yMin = int.MaxValue, yMax = int.MinValue;
            int xMin = int.MaxValue, xMax = int.MinValue;
            foreach (Vector3Int cell in walkable)
            {
                if(cell.y < yMin) yMin = cell.y;
                if(cell.y > yMax) yMax = cell.y;
                if(cell.x < xMin) xMin = cell.x;
                if(cell.x > xMax) xMax = cell.x;
            }
            
            List<RectInt> rects = new List<RectInt>();
            //현재 확장중인 사각형: key = (xMin, xMax exclusive), value = 시작 y값
            Dictionary<(int, int), int> active = new Dictionary<(int, int), int>();

            //마지막으로 돌던 사각형도 rect집합에 들어가게 하려면 +1만큼 올려서 돈다.
            for (int y = yMin; y <= yMax + 1; y++) 
            {
                //현재 행의 연속된 walkable 구간을 구한다.-> 가로로 쭉 돌면서 블럭들을 구하는 과정
                HashSet<(int start, int end)> currentSegment = new HashSet<(int , int )>();
                if (y <= yMax)
                {
                    int? segStart = null;
                    for (int x = xMin; x <= xMax + 1; x++)
                    {
                        bool isWalkable = x <= xMax && walkable.Contains(new Vector3Int(x, y));
                        if (isWalkable && segStart == null)
                            segStart = x; //걸을 수 있는 셀이고 이 세그먼트의 시작이면 x를 지정
                        else if (!isWalkable && segStart != null)
                        {
                            currentSegment.Add((segStart.Value, x));
                            segStart = null;
                            //걸을 수 없는 상태고 이미 시작되어 있는 상태라면 현재 세그먼트를 끝으로 해서 current에 저장.
                        }
                    }
                }

                //이전행과 완전히 일치하는 세그먼트는 계속 확장. 아니면 사각형을 확정한다.
                var newActive = new Dictionary<(int start, int end), int>();
                foreach ((int start, int end) seg in currentSegment)
                {
                    //완전 일치하는 사각형이 있다면 해당 사각형의 y값을 가져오고
                    //그렇지 않다면 현재 y값을 넣는다.
                    newActive[seg] = active.GetValueOrDefault(seg, y);
                }
                
                //active중에서 현재 currentSeg에 속하지 못한 것들은 이번행에서 끊긴 사각형이다.
                foreach (KeyValuePair<(int start, int end), int> kv in active)
                {
                    if(!currentSegment.Contains(kv.Key))
                        rects.Add(new RectInt(kv.Key.start, kv.Value, kv.Key.end - kv.Key.start, y - kv.Value));
                }
                active = newActive;
            }
            return rects;
        }
        
        private List<RectInt> SplitRectsAtWallBoundaries(List<RectInt> rects, HashSet<Vector3Int> walkable)
        {
            bool changed = true;
            while (changed)
            {
                changed = false;
                List<RectInt> next = new List<RectInt>();

                foreach (RectInt rect in rects)
                {
                    List<RectInt> split = TrySplitRect(rect, walkable);
                    if (split != null)
                    {
                        next.AddRange(split); //쪼개진 사각형을 넣어준다.
                        changed = true;
                    }else 
                        next.Add(rect); //안쪼개졌다면 기존 rect만 넣는다.
                }
                rects = next; //한번 모두 쪼갠걸 넣는다.
            }
            //더이상 쪼개지지 않을때까지 쪼갰다면 반환.
            return rects;
        }

        private List<RectInt> TrySplitRect(RectInt rect, HashSet<Vector3Int> walkable)
        {
            //상쪽 검사 이후 없으면 하쪽 순서로 x축을 먼저 검사해서. 나오면 
            int? splitX = FindHorizontalSideSplit(rect, -1, walkable)
                ?? FindHorizontalSideSplit(rect, 1, walkable);

            if (splitX.HasValue)
            {
                return new List<RectInt>
                {
                    new RectInt(rect.xMin, rect.yMin, splitX.Value - rect.xMin, rect.height),
                    new RectInt(splitX.Value, rect.yMin, rect.xMax - splitX.Value, rect.height)
                };
            }
            
            int? splitY = FindVerticalSideSplit(rect, -1, walkable)
                          ?? FindVerticalSideSplit(rect, 1, walkable);
            if (splitY.HasValue)
            {
                return new List<RectInt>
                {
                    new RectInt(rect.xMin, rect.yMin, rect.width, splitY.Value - rect.yMin),
                    new RectInt(rect.xMin, splitY.Value, rect.width, rect.yMax - splitY.Value)
                };
            }
 
            return null;
        }

        private void BuildPolygons(List<RectInt> rects, HashSet<Vector3Int> walkable)
        {
            float radius = bakeData.AgentData != null ? bakeData.AgentData.AgentRadius : 0.1f;
            
            WorldRect[] shrunkRects = new WorldRect[rects.Count]; //수축된 사각형들
            for(int i = 0; i < rects.Count; i++)
                shrunkRects[i] = ShrinkRect(rects[i], walkable, radius);
            
            //축소시킨 사각형들을 폴리곤 데이터로 변경하는 작업을 한다.
            for(int i = 0; i < rects.Count; i++)
                bakeData.polygons.Add(RectToPolygon(i, shrunkRects[i]));
            
            //축소된 Rect들을 인접한 것을 찾아서 포탈을 만들어준다.
            for(int i = 0; i < rects.Count; i++)
                for (int j = i + 1; j < rects.Count; j++)
                    TryAddPortal(rects[i], rects[j],
                        shrunkRects[i], shrunkRects[j],
                        bakeData.polygons[i], bakeData.polygons[j]);
        }

        private WorldRect ShrinkRect(RectInt rect, HashSet<Vector3Int> walkable, float radius)
        {
            Vector2 bl = CellCornerToWorld(groundMap, rect.xMin, rect.yMin); //bottom left
            Vector2 tr = CellCornerToWorld(groundMap, rect.xMax, rect.yMax); //top right;
            
            float left = IsSideWall(rect, -1, 0, walkable) ? radius : 0f; //좌변이 압축될 수 있다면 agent radius만큼 압축
            float right = IsSideWall(rect, 1, 0, walkable) ? radius : 0f;
            float bottom = IsSideWall(rect, 0, -1, walkable) ? radius : 0f;
            float top = IsSideWall(rect, 0, 1, walkable) ? radius : 0f;
            
            ClampInSet(ref left, ref right, tr.x - bl.x); //이 너비에서 이 수축이 되기 위한 최대길이
            ClampInSet(ref bottom, ref top, tr.y - bl.y); // 이 높이에서 수축이 되기위한 최대길이.
            
            return new WorldRect
            {
                xMin = bl.x + left,
                yMin = bl.y + bottom,
                xMax = tr.x - right,
                yMax = tr.y - top 
            };
        }

        #region Static Helper method
        //좌하단(코너) -CellToWorld, CellCenterToWorld
        private static Vector2 CellCornerToWorld(Tilemap groundMap, int x, int y)
            => groundMap.CellToWorld(new Vector3Int(x, y, 0));

        //dx, dy 방향 바깥쪽 셀들이 전부 walkable이 아니면 그 변 전체가 벽이니까 수축을 위한 정보로 사용한다.
        private static bool IsSideWall(RectInt rect, int dx, int dy, HashSet<Vector3Int> walkable)
        {
            if (dx != 0)
            {
                int x = dx < 0 ? rect.xMin - 1 : rect.xMax;
                for(int y = rect.yMin; y < rect.yMax; y++)
                    if(walkable.Contains(new Vector3Int(x, y, 0))) return false;
            }
            else //여기는 y축
            {
                int y = dy < 0 ? rect.yMin - 1 : rect.yMax;
                for(int x = rect.xMin; x < rect.xMax; x++)
                    if(walkable.Contains(new Vector3Int(x, y, 0))) return false;
            }
            
            return true;
        }

        private const float MinExtent = 0.05f;

        //마주보는 두 변의 합(a+b) 가 extent를 넘지 않게 비례 축소한다.
        private static void ClampInSet(ref float a, ref float b, float extent)
        {
            float max = extent - MinExtent;
            if (a + b > max && a + b > 0f)
            {
                float scale = max / (a + b);
                a *= scale;
                b *= scale;
            }
        }

        /// <summary>
        /// 수평 변(하: dy=-1, 상: dy=1)을 x방향으로 스캐닝하여 벽이 끊어지는 전환이 발생하는 x 좌표를 반환한다.
        /// </summary>
        private static int? FindHorizontalSideSplit(RectInt rect, int dy, HashSet<Vector3Int> walkable)
        {
            if (rect.width <= 1) return null;

            int checkY = dy < 0 ? rect.yMin - 1 : rect.yMax; //사각형의 y축 아래 또는 y축 위를 저장.

            bool prevWall = !walkable.Contains(new Vector3Int(rect.xMin, checkY));
            for (int x = rect.xMin + 1; x < rect.xMax; x++)
            {
                bool wall = !walkable.Contains(new Vector3Int(x, checkY));
                if (wall != prevWall) return x;
                prevWall = wall;
            }

            return null;
        }

        private static int? FindVerticalSideSplit(RectInt rect, int dx, HashSet<Vector3Int> walkable)
        {
            if (rect.height <= 1) return null;
            int checkX = dx < 0 ? rect.xMin - 1 : rect.xMax;

            bool prevWall = !walkable.Contains(new Vector3Int(checkX, rect.yMin));
            for (int y = rect.yMin + 1; y < rect.yMax; y++)
            {
                bool wall = !walkable.Contains(new Vector3Int(checkX, y));
                if (wall != prevWall) return y;
                prevWall = wall;
            }
            return null;

        }

        private static PolygonData RectToPolygon(int id, WorldRect rect)
        {
            Vector2 bl = new Vector2(rect.xMin, rect.yMin);
            Vector2 br = new Vector2(rect.xMax, rect.yMin);
            Vector2 tr = new Vector2(rect.xMax, rect.yMax);
            Vector2 tl = new Vector2(rect.xMin, rect.yMax);

            return new PolygonData
            {
                id = id,
                center = (bl + br + tr + tl) * 0.25f,
                vertices = new[] { bl, br, tr, tl }, //CCW
                portals = new List<PortalData>()
            };
        }

        /// <summary>
        /// 두 직사각형이 엣지를 공유하면 Portal을 양방향으로 추가해준다.
        /// 공유하는 변은 벽이 아니므로 축소되지 않기 때문에 양 좌표가 일치할 수 밖에 없다.
        /// </summary>
        private static void TryAddPortal(RectInt rectA, RectInt rectB, WorldRect wrA, WorldRect wrB,
            PolygonData polygonA, PolygonData polygonB)
        {
            const float eps = 1e-4f;

            //이렇게 되면 왼쪽 또는 오른쪽으로 붙은 포탈이다.
            if (rectA.xMax == rectB.xMin || rectB.xMax == rectA.xMin)
            {
                float px = (rectA.xMax == rectB.xMin) ? wrA.xMax : wrB.xMax;
                float yMin = Mathf.Max(wrA.yMin, wrB.yMin); //2개의 y최소치중에 큰거
                float yMax = Mathf.Min(wrA.yMax, wrB.yMax); //2개의 y최대치중 작은거.

                if (yMax - yMin <= eps) return;
                
                Vector2 pA = new Vector2(px, yMin);
                Vector2 pB = new Vector2(px, yMax);  //포탈의 아래 위 좌표를 만드는거
                
                //두개의 Rect에 동일한 포탈을 기록하되, 이웃ID만 다르게 준다.
                polygonA.portals.Add(new PortalData{pointA =  pA, pointB = pB, neighborId = polygonB.id});
                polygonB.portals.Add(new PortalData{pointA =  pA, pointB = pB, neighborId = polygonA.id});
            }
            else if (rectA.yMax == rectB.yMin || rectB.yMax == rectA.yMin)
            {
                //2개의 사각형이 수직으로 인접한 경우.
                float   py   = (rectA.yMax == rectB.yMin) ? wrA.yMax : wrB.yMax;
                float   xMin = Mathf.Max(wrA.xMin, wrB.xMin);
                float   xMax = Mathf.Min(wrA.xMax, wrB.xMax);
                if (xMax - xMin <= eps) return;

                Vector2 pA = new Vector2(xMin, py);
                Vector2 pB = new Vector2(xMax, py);
                polygonA.portals.Add(new PortalData { pointA = pA, pointB = pB, neighborId = polygonB.id });
                polygonB.portals.Add(new PortalData { pointA = pA, pointB = pB, neighborId = polygonA.id });

            }
        }
        
        #endregion
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos || bakeData == null) return;
            
            foreach (var polygon in bakeData.polygons)
            {
                //폴리곤 윤곽선
                Gizmos.color = polygonColor;
                Handles.color = polygonColor;
                DrawPolygonGizmo(polygon.vertices);
                
                
                //중심점
                Gizmos.color = centerColor;
                Gizmos.DrawWireSphere(polygon.center, 0.1f);
                
                //포탈
                Gizmos.color = portalColor;
                foreach (PortalData p in polygon.portals)
                {
                    Gizmos.DrawLine(p.pointA, p.pointB);
                    Gizmos.DrawWireSphere( (p.pointA + p.pointB) * 0.5f, 0.2f); //포탈 중심에 원
                }
            }
        }

        private void DrawPolygonGizmo(Vector2[] verts)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Handles.DrawLine(verts[i], verts[(i + 1) % verts.Length], 4f);
            }
        }
#endif
    }
}
//35분