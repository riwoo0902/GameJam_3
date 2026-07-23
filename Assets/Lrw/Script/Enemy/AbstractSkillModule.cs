using DevLib.ModuleSystem;
using Lrw.Script.Enemy.MoveSystem;
using UnityEngine;

namespace Lrw.Script.Enemy
{
    public abstract class AbstractSkillModule : Module, ISkillModule
    {
        [field: SerializeField] public float AttackDistance { get; private set; } = 0.5f;

        private IEnemyMoveModule _enemyMoveModule;
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _enemyMoveModule.NavAgent.StoppingDistance = Mathf.Max(AttackDistance - 0.1f,0f);
        }

        public abstract bool CanUse(Transform target = null);
        
        public abstract void Use(Transform target = null);
        
        
    }
}