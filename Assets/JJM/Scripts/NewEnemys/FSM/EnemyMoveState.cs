using Agents;
using JJM.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace JJM.Scripts.NewEnemys.FSM
{
    public class EnemyMoveState : AbstractEnemyState
    {
        private NavMeshAgent _navMeshAgent;
        
        public EnemyMoveState(Agent agent, int stateClipHash, int layerIndex) : base(agent, stateClipHash, layerIndex)
        {
            _navMeshAgent = _agent.GetComponent<NavMeshAgent>();
            Debug.Assert(_navMeshAgent != null, "NavMeshAgent != null");
        }

        public override void Update()
        {
            base.Update();
            
            Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
            
            _navMeshAgent.SetDestination(playerPos);

            if (Enemy.PlayerRecognitionDistance > Vector2.Distance(playerPos,
                    _agent.transform.position))
            {
                Enemy.ChangeState(EnemyStateEnum.ATTACK);
            }
        }
    }
}