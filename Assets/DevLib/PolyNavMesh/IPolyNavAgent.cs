using System.Threading.Tasks;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    public interface IPolyNavAgent
    {
        bool PathPending { get; }
        bool HasPath { get; }
        bool IsPathStale { get; }

        /// <summary>목적지에 완전히 도달할 수 없어 최근접 지점까지만 경로를 찾은 경우 true.</summary>
        bool IsPartialPath { get; }

        /// <summary>
        /// 비동기 경로 탐색. 결과 waypoint를 pointArr에 채우고 포인트 수를 반환한다.
        /// 실패 또는 취소 시 -1 반환.
        /// </summary>
        Task<int> GetPath(Vector2 start, Vector2 destination, Vector2[] pointArr);
    }
}