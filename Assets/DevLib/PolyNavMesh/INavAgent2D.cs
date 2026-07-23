using UnityEngine;

namespace DevLib.PolyNavMesh
{
    public interface INavAgent2D
    {
        float Speed { get; set; }
        float StoppingDistance {get;set;}
        float RemainingDistance { get; }
        Vector2 MoveDir { get; set; }
        void SetDestination(Vector2 destination);
    }
}