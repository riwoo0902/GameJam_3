using System;
using DevLib.AnimatorSystem;
using DevLib.ModuleSystem;
using DevLib.PolyNavMesh;
using Lrw.Script.Agent.StatSystem;
using Publics.Agent;
using UnityEngine;

namespace Lrw.Script.Enemy.MoveSystem
{
    public class EnemyMoveModule : Module,IAfterInitModule, IEnemyMoveModule
    {
        [SerializeField] private StatDataSo moveSpeedStat;

        [SerializeField] private HashDataSO velocityX;
        [SerializeField] private HashDataSO velocityY;
        
        public INavAgent2D NavAgent { get; private set; }
        
        private IStatModule _statModule;
        private IRenderer _render;
        
        private Stat _moveSpeedStat;

        private bool _active;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null,"StatModule is not found"); 
            NavAgent = owner.GetComponent<INavAgent2D>();
            Debug.Assert(NavAgent != null, "NavAgent is not found");
            _render = owner.GetModule<IRenderer>();
            Debug.Assert(_render != null, "Renderer is not found");
        }

        public void AfterInit()
        {
            _moveSpeedStat = _statModule.GetStat(moveSpeedStat);
            Debug.Assert(_moveSpeedStat != null,"MoveSpeedStat is not found");
            
            NavAgent.Speed = _moveSpeedStat.Value;
            _moveSpeedStat.OnValueChanged += MoveSpeedStatOnOnValueChanged;
        }

        private void OnDestroy()
        {
            _moveSpeedStat.OnValueChanged -= MoveSpeedStatOnOnValueChanged;
        }
        
        private void LateUpdate()
        {
            Vector2 dir = NavAgent.MoveDir.normalized;
            _render.SetFloat(velocityX.HashValue,dir.x);
            _render.SetFloat(velocityY.HashValue,dir.x);
        }

        private void MoveSpeedStatOnOnValueChanged(float currentValue, float prevValue)
        {
            NavAgent.Speed = currentValue;
        }

        public void SetDestination(Vector2 targetPos)
        {
            if(!_active) return;
            NavAgent.SetDestination(targetPos);
        }

        public void SetActive(bool active)
        {
            _active = active; 
            if(!_active) NavAgent.ResetPath();
        }
        
        
        
    }
}