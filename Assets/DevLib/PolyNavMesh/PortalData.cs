using System;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    [Serializable]
    public struct PortalData
    {
        public Vector2 pointA;
        public Vector2 pointB;
        public int neighborId; //인접한 이웃의 폴리곤 ID
    }
}