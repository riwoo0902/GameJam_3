using System.Collections.Generic;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    [CreateAssetMenu(fileName = "baked data", menuName = "Lib/PolyNav/Baked data", order = 5)]
    public class NavMeshBakeDataSo : ScriptableObject
    {
        //이 에이전트에 맞춤으로 베이킹을 한다.
        [field: SerializeField] public NavAgentDataSO AgentData { get; private set; }
        
        public List<PolygonData> polygons = new();

        //런타임에 빠르게 찾기 위한 딕셔너리 구조
        private Dictionary<int, NavPolygon> _runtimeMap;

        private void OnEnable() => BuildRuntimeMap();

        /// <summary>
        /// 직렬화 데이터로부터 런타임에 딕셔너리로 빌드한다. 
        /// </summary>
        public void BuildRuntimeMap()
        {
            _runtimeMap = new Dictionary<int, NavPolygon>();

            foreach (PolygonData data in polygons)
            {
                _runtimeMap[data.id] = new NavPolygon()
                {
                    id = data.id,
                    vertices = data.vertices,
                    center = data.center,
                    portals = data.portals
                };
            }
            Debug.Log($"[NavMesh] 런타임 맵 빌딩 완료 : {polygons.Count} 개의 폴리곤 생성");
        }

        /// <summary>
        ///  지정된 월드 좌표가 있는 폴리곤을 반환한다(선형탐색 함수)
        /// </summary>
        public bool GetPolygonAt(Vector2 worldPoint, out NavPolygon polygon)
        {
            foreach (NavPolygon p in _runtimeMap.Values)
            {
                if (ContainPoint(p.vertices, worldPoint))
                {
                    polygon = p;
                    return true;
                }
            }

            polygon = null;
            return false;
        }

        public bool TryGetPolygon(int id, out NavPolygon polygon)
        {
            polygon = null;
            return _runtimeMap != null && _runtimeMap.TryGetValue(id, out polygon);
        }

        public bool GetNearestPolygon(Vector2 worldPoint, out NavPolygon polygon)
        {
            polygon = null;
            if (_runtimeMap == null || _runtimeMap.Count == 0) return false;
            
            float bestSqr = float.MaxValue;
            foreach (NavPolygon p in _runtimeMap.Values)
            {
                float sqr = (p.center - worldPoint).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    polygon = p;
                }
            }
            return polygon != null;
        }
        
        public void Clear() => polygons.Clear();
        
        private bool ContainPoint(Vector2[] verts, Vector2 wp)
        {
            for (int i = 0, j = verts.Length - 1; i < verts.Length; j = i++)
            {
                Vector2 a = verts[j], b = verts[i];
                // Cross = (b-a) x (wp-a)
                float cross = (b.x - a.x) * (wp.y - a.y) - (b.y - a.y) * (wp.x - a.x);
                if (cross < 0) return false;
            }
            return true;
        }
    }
}