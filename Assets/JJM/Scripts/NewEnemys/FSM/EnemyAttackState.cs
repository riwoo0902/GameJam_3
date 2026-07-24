using Agents;
using UnityEngine;
using System.Collections;
using JJM.Scripts.Player;
using UnityEngine.AI;

namespace JJM.Scripts.NewEnemys.FSM
{
    public class EnemyAttackState : AbstractEnemyState
    {
        private bool _canAttack = true;
        private NavMeshAgent _navMeshAgent;
        private bool _stopMoveAttack;

        public EnemyAttackState(Agent agent, int stateClipHash, int layerIndex) : base(agent, stateClipHash, layerIndex)
        {
            _navMeshAgent = _agent.GetComponent<NavMeshAgent>();
            Debug.Assert(_navMeshAgent != null, "NavMeshAgent != null");
            _stopMoveAttack = Enemy.IsAttackStop;
        }

        public override void Enter(float transitionDuration = 0.1f)
        {
            base.Enter(transitionDuration);
            if (_stopMoveAttack) _navMeshAgent.isStopped = true;
        }

        public override void Update()
        {
            base.Update();
            if (!_stopMoveAttack)
            {
                Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
                            
                _navMeshAgent.SetDestination(playerPos);
            }
            if (!_canAttack) return;
            Attack();
            Enemy.StartCoroutine(AttackCoolDown());
        }

        private void Attack()
        {
            Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
            if (Enemy.PlayerRecognitionDistance < Vector2.Distance(playerPos,
                    _agent.transform.position))
            {
                Enemy.ChangeState(EnemyStateEnum.MOVE);
                if (_stopMoveAttack) _navMeshAgent.isStopped = false;
                return;
            }
            
            Enemy.attackEvent?.Invoke();
        }

        private IEnumerator AttackCoolDown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(Enemy.AttackSpeed);
            _canAttack = true;
        }
    }
}