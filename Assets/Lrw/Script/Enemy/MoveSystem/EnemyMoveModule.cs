using DevLib.ModuleSystem;
using Lrw.Script.StatSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public class EnemyMoveModule : Module,IAfterInitModule
    {
        
        [SerializeField] private StatDataSo statDataSo;

        private IStatModule statModule;
        
        private Stat _moveSpeedStat;
        
        protected float MoveSpeed => _moveSpeedStat.Value;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            statModule = owner.GetModule<IStatModule>();
            Debug.Assert(statModule != null,"StatModule is not found");
        }


        public void AfterInit()
        {
            
        }
        
        
    }
}