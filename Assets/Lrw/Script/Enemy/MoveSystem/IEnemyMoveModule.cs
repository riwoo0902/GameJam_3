using DevLib.PolyNavMesh;
using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public interface IEnemyMoveModule
    {
        INavAgent2D NavAgent { get; }
        void SetDestination(Vector2 targetPos);
    }
}