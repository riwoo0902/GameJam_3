using DevLib.ModuleSystem;
using DevLib.PolyNavMesh;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public class EnemyMoveModule : Module,IAfterInitModule
    {
        
        [SerializeField] private StatDataSo moveSpeedStat;

        private IPolyNavAgent _navAgent;
        
        private IStatModule _statModule;
        
        private Stat _moveSpeedStat;
        
        protected float MoveSpeed => _moveSpeedStat.Value;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null,"StatModule is not found"); 
            _navAgent = owner.GetModule<IPolyNavAgent>();
            Debug.Assert(_navAgent != null, "NavAgent is not found");
            
        }


        public void AfterInit()
        {
            _moveSpeedStat = _statModule.GetStat(moveSpeedStat);
            Debug.Assert(_moveSpeedStat != null,"MoveSpeedStat is not found");
            
        }
        
        
        
        
    }
}