using System.Collections;
using DevLib.ModuleSystem;
using DevLib.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Player
{
    public class PlayerAttack : Module
    {
        [Header("Pool")]
        [SerializeField] private PoolItemSO projectile;
        [SerializeField] private PoolInitializer poolManager;

        [Header("Attack")]
        [SerializeField] private float fireCoolTime = 0.2f;

        private bool _canFire = true;

        private IPlayerRotation _playerRotation;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _playerRotation = owner.GetModule<PlayerRotation>();
        }

        private void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            if (!Mouse.current.leftButton.isPressed)
            {
                return;
            }

            if (!_canFire)
            {
                return;
            }

            Vector2 direction = _playerRotation.MouseRelativePosition;

            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            Fire(direction);
        }

        private void Fire(Vector2 direction)
        {
            _canFire = false;

            PlayerProjective projectileInstance =
                poolManager.Pop<PlayerProjective>(projectile);

            if (projectileInstance == null)
            {
                _canFire = true;
                return;
            }

            float angle = GetFourDirectionAngle(direction);

            projectileInstance.transform.SetPositionAndRotation(
                _owner.transform.position,
                Quaternion.Euler(0f, 0f, angle)
            );

            StartCoroutine(FireCoolDown());
        }

        private IEnumerator FireCoolDown()
        {
            yield return new WaitForSeconds(fireCoolTime);
            _canFire = true;
        }

        private static float GetFourDirectionAngle(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                return direction.x >= 0f ? 0f : 180f;
            }

            return direction.y >= 0f ? 90f : -90f;
        }
    }
}