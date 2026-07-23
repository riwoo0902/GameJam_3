using DevLib.ModuleSystem;
using Lrw.Script.Enemy.MoveSystem;
using Unity.Behavior;
using UnityEngine;

namespace Lrw.Script.Enemy
{
    public class Enemy : ModuleOwner
    {
        private readonly string _taget = "Target";
        private readonly string _enemy = "Enemy";
        
        public IEnemyMoveModule MoveModule { get; private set; }
        public ISkillModule SkillModule { get; private set; }
        private BehaviorGraphAgent _behaviorGraphAgent;
        
        
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            MoveModule = GetModule<IEnemyMoveModule>();
            SkillModule = GetModule<ISkillModule>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(_behaviorGraphAgent != null,"behaviorGraphAgent should not be null");
            _behaviorGraphAgent.SetVariableValue(_taget, EnemyManager.Target);
            _behaviorGraphAgent.SetVariableValue(_enemy, this);
        }
        
        
        
    }
}