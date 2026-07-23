using DevLib.ModuleSystem;
using DevLib.PolyNavMesh;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public class EnemyMoveModule : Module,IAfterInitModule, IEnemyMoveModule
    {
        [SerializeField] private StatDataSo moveSpeedStat;

        private INavAgent2D _navAgent;
        
        private IStatModule _statModule;
        
        private Stat _moveSpeedStat;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null,"StatModule is not found"); 
            _navAgent = owner.GetModule<INavAgent2D>();
            Debug.Assert(_navAgent != null, "NavAgent is not found");
        }

        public void AfterInit()
        {
            _moveSpeedStat = _statModule.GetStat(moveSpeedStat);
            Debug.Assert(_moveSpeedStat != null,"MoveSpeedStat is not found");
            
            _navAgent.Speed = _moveSpeedStat.Value;
            _moveSpeedStat.OnValueChanged += MoveSpeedStatOnOnValueChanged;
        }

        private void OnDestroy()
        {
            _moveSpeedStat.OnValueChanged -= MoveSpeedStatOnOnValueChanged;
        }

        private void MoveSpeedStatOnOnValueChanged(float currentValue, float prevValue)
        {
            _navAgent.Speed = currentValue;
        }

        public void SetDestination(Vector2 targetPos)
        {
            _navAgent.SetDestination(targetPos);
        }
        
        
        
    }
}