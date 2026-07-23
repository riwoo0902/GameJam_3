using DevLib.ModuleSystem;
using Lrw.Script.Agent.StatSystem;
using Lrw.Script.Enemy.MoveSystem;
using UnityEngine;

namespace Lrw.Script.Enemy
{
    public abstract class AbstractSkillModule : Module,IAfterInitModule, ISkillModule
    {
        [SerializeField] private StatDataSo statDataSo;
        [field: SerializeField] public float AttackDistance { get; private set; } = 0.5f;

        private IStatModule _statModule;
        private IEnemyMoveModule _enemyMoveModule;

        protected Stat Stat { get; private set; }
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _enemyMoveModule = owner.GetModule<IEnemyMoveModule>();
            _statModule = owner.GetModule<IStatModule>();
        }
        
        public void AfterInit()
        {
            _enemyMoveModule.NavAgent.StoppingDistance = Mathf.Max(AttackDistance - 0.1f,0f);
            Stat = _statModule.GetStat(statDataSo);
        }

        public abstract bool CanUse(Transform target = null);
        
        public abstract void Use(Transform target = null);


        
    }
}