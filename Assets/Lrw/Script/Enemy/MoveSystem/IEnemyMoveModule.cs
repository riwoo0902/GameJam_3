using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public interface IEnemyMoveModule
    {
        void SetDestination(Vector2 targetPos);
    }
}