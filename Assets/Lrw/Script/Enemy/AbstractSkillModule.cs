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
        public bool SkillEnd { get; private set; }

        private IStatModule _statModule;
        protected IEnemyMoveModule EnemyMoveModule { get; private set; }

        protected Stat Stat { get; private set; }
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            EnemyMoveModule = owner.GetModule<IEnemyMoveModule>();
            _statModule = owner.GetModule<IStatModule>();
        }
        
        public virtual void AfterInit()
        {
            EnemyMoveModule.NavAgent.StoppingDistance = Mathf.Max(AttackDistance - 0.1f,0f);
            Stat = _statModule.GetStat(statDataSo);
        }

        public abstract bool CanUse(Transform target = null);

        public void SkillUse(Transform target = null)
        {
            SkillEnd = false;
            Use(target);
        }
        protected abstract void Use(Transform target = null);

        protected void SetSkillEnd() => SkillEnd = true;  

        
    }
}