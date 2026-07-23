using UnityEngine;

namespace DevLib.PolyNavMesh
{
    public interface INavAgent2D
    {
        float Speed { get; set; }
        void SetDestination(Vector2 destination);
    }
}