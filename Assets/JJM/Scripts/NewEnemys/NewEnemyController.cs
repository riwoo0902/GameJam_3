using System;
using Agents;
using Agents.FSM;
using DevLib.AnimatorSystem;
using JJM.Scripts.NewEnemys.FSM;
using Publics.Agent;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace JJM.Scripts.NewEnemys
{
    public class NewEnemyController : Agent
    {
        [SerializeField] private StateListSO stateList;
        [SerializeField] private float playerRecognitionDistance;
        [SerializeField] private float attackSpeed = 2f;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private bool isAttackStop = true;
        public UnityEvent attackEvent;
        [SerializeField] private HashDataSO hashDataX;
        [SerializeField] private HashDataSO hashDataY;
        public float PlayerRecognitionDistance => playerRecognitionDistance;
        public float AttackSpeed => attackSpeed;
        public bool IsAttackStop => isAttackStop;
        private StateMachine _stateMachine;
        
        private Animator _animator;
        
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, stateList.states);
            ChangeState(EnemyStateEnum.MOVE);
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            
            _animator = GetModule<IRenderer>().Animator;
        }

        private void Update()
        {
            _animator.SetFloat(hashDataX.HashValue, agent.velocity.x);
            _animator.SetFloat(hashDataY.HashValue, agent.velocity.y);
            
            _stateMachine.CurrentState.Update();
        }

        public void ChangeState(EnemyStateEnum state)
        {
            _stateMachine.ChangeState((int)state, 0.1f);
        }
    }
}