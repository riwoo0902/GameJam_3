using System.Collections.Generic;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    /// <summary>
    /// 런타임에서 사용하는 폴리곤 데이터 (AstarNode와 대응된다.)
    /// </summary>
    public class NavPolygon
    {
        public int id;
        public Vector2 center;
        public Vector2[] vertices;
        public List<PortalData> portals;

        public override bool Equals(object obj) => obj is NavPolygon p && id == p.id;
        public override int GetHashCode() => id; //id를 고유 해시코드로 사용한다.

        public static bool operator ==(NavPolygon a, NavPolygon b)
        {
            if(a is null ) return b is null;
            return a.Equals(b);
        }

        public static bool operator !=(NavPolygon a, NavPolygon b) => !(a == b);

    }
}