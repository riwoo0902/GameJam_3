using DevLib.ObjectPool.Runtime;
using JJM.Scripts.Player;
using JJM.Scripts.Players;
using UnityEngine;

namespace JJM.Scripts.NewEnemys
{
    public class EnemyProjectiveAttackEvent : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolItemSO projectile;

        [Header("Spawn")]
        [SerializeField] private Transform firePoint;
        [SerializeField, Min(1)] private int projectileCount = 3;
        [SerializeField, Min(0f)] private float spreadAngle = 30f;

        [Header("Projectile Stat")]
        [SerializeField, Min(0f)] private float projectileSpeed = 8f;
        [SerializeField, Min(0f)] private float projectileDamage = 10f;
        private void Awake()
        {
            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        public void F()
        {
            if (!TryInitialize())
            {
                return;
            }

            Vector2 direction =
                PlayerManager.Instance.Player.transform.position - firePoint.position;

            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            float targetAngle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            FireSpread(targetAngle);
        }

        private bool TryInitialize()
        {
            if (poolManager == null)
            {
                Debug.LogError($"{name}: PoolManager가 없습니다.");
                return false;
            }

            if (projectile == null)
            {
                Debug.LogError($"{name}: Projectile PoolItem이 없습니다.");
                return false;
            }
            

            return true;
        }

        private void FireSpread(float centerAngle)
        {
            if (projectileCount == 1)
            {
                SpawnProjectile(centerAngle);
                return;
            }

            float startAngle =
                centerAngle - spreadAngle * 0.5f;

            float angleStep =
                spreadAngle / (projectileCount - 1);

            for (int i = 0; i < projectileCount; i++)
            {
                float angle =
                    startAngle + angleStep * i;

                SpawnProjectile(angle);
            }
        }

        private void SpawnProjectile(float angle)
        {
            PlayerProjective projectileInstance =
                poolManager.Pop<PlayerProjective>(projectile);

            if (projectileInstance == null)
            {
                return;
            }

            projectileInstance.Speed = projectileSpeed;
            projectileInstance.Damage = projectileDamage;

            projectileInstance.transform.SetPositionAndRotation(
                firePoint.position,
                Quaternion.Euler(0f, 0f, angle)
            );
        }
    }
}