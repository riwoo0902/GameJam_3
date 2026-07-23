using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    [Serializable]
    public class PolygonData
    {
        public int id;
        public Vector2 center;
        public Vector2[] vertices; //CCW로 각 꼭지점을 가지고 있다.
        public List<PortalData> portals = new List<PortalData>(); //인접된 폴리곤 연결 포탈 목록
    }
}