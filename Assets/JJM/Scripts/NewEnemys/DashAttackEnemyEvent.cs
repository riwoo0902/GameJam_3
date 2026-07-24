using System.Collections;
using JJM.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace JJM.Scripts.NewEnemys
{
    public class DashAttackEnemyEvent : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent agent;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashDuration = 0.3f;
        [SerializeField] private bool rotateToDashDirection = true;

        private Coroutine _dashCoroutine;

        private void Awake()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            Debug.Assert(agent != null, $"{name}에 NavMeshAgent가 없습니다.");
        }

        public void DashPlay()
        {
            if (_dashCoroutine != null)
            {
                return;
            }

            if (PlayerManager.Instance == null ||
                PlayerManager.Instance.Player == null)
            {
                return;
            }

            if (agent == null || !agent.isOnNavMesh)
            {
                return;
            }

            Vector3 playerPosition =
                PlayerManager.Instance.Player.transform.position;

            Vector3 dashDirection =
                playerPosition - transform.position;

            dashDirection.y = 0f;

            if (dashDirection.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            dashDirection.Normalize();

            _dashCoroutine = StartCoroutine(
                DashCoroutine(dashDirection));
        }

        private IEnumerator DashCoroutine(Vector3 dashDirection)
        {
            bool previousStoppedState = agent.isStopped;

            agent.isStopped = true;
            agent.ResetPath();

            if (rotateToDashDirection)
            {
                transform.rotation =
                    Quaternion.LookRotation(dashDirection);
            }

            float elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                float deltaTime = Time.deltaTime;

                Vector3 movement =
                    dashDirection * dashSpeed * deltaTime;

                agent.Move(movement);

                elapsedTime += deltaTime;

                yield return null;
            }

            if (agent.isOnNavMesh)
            {
                agent.isStopped = previousStoppedState;
            }

            _dashCoroutine = null;
        }

        private void OnDisable()
        {
            if (_dashCoroutine != null)
            {
                StopCoroutine(_dashCoroutine);
                _dashCoroutine = null;
            }
        }
    }
}