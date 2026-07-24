using DevLib.ObjectPool.Runtime;
using JJM.Scripts.Players;
using JJM.Scripts.Players.Stats;
using UnityEngine;

namespace JJM.Scripts.PowerUp
{
    [CreateAssetMenu(
        fileName = "three attack power up",
        menuName = "PowerUp/Three Attack",
        order = 0)]
    public class ThreeAttackPowerUpSo : AbstractPowerUpSo
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float spreadRotation = 20f;
        [SerializeField] private PoolItemSO projectile;

        private PlayerAttack _attack;
        private IPlayerRotation _rotation;

        private bool _isSubscribed;

        public override void PowerUpPlay()
        {
            base.PowerUpPlay();

            if (!TryInitializePlayer())
            {
                return;
            }

            if (!TryInitializeModules())
            {
                return;
            }

            if (_isSubscribed)
            {
                return;
            }

            _attack.OnAttack.AddListener(HandleThreeAttack);
            _isSubscribed = true;
        }

        private bool TryInitializeModules()
        {
            if (_attack == null)
            {
                _attack = Player.GetModule<PlayerAttack>();
            }

            if (_rotation == null)
            {
                _rotation = Player.GetModule<IPlayerRotation>();
            }

            if (_attack == null)
            {
                Debug.LogError($"{name}: PlayerAttack 모듈을 찾지 못했습니다.");
                return false;
            }

            if (_rotation == null)
            {
                Debug.LogError($"{name}: IPlayerRotation 모듈을 찾지 못했습니다.");
                return false;
            }

            return true;
        }

        protected override void OnDisable()
        {
            Unsubscribe();
            base.OnDisable();
        }

        private void Unsubscribe()
        {
            if (_isSubscribed && _attack != null)
            {
                _attack.OnAttack.RemoveListener(HandleThreeAttack);
            }

            _isSubscribed = false;
            _attack = null;
            _rotation = null;
        }

        private void HandleThreeAttack()
        {
            if (Player == null || _attack == null || _rotation == null)
            {
                Unsubscribe();
                return;
            }

            float baseAngle =
                PlayerAttack.GetFourDirectionAngle(
                    _rotation.MouseRelativePosition);

            SpawnProjectile(baseAngle - spreadRotation);
            SpawnProjectile(baseAngle + spreadRotation);
        }

        private void SpawnProjectile(float angle)
        {
            if (poolManager == null || projectile == null)
            {
                Debug.LogError($"{name}: 풀 또는 투사체가 설정되지 않았습니다.");
                return;
            }

            PlayerProjective projectileInstance =
                poolManager.Pop<PlayerProjective>(projectile);

            if (projectileInstance == null)
            {
                return;
            }

            PlayerStatManager statManager = PlayerStatManager.Instance;

            if (statManager == null)
            {
                Debug.LogError("PlayerStatManager.Instance가 존재하지 않습니다.");
                return;
            }

            projectileInstance.Speed =
                _attack.ProjectiveSpeed * statManager.PSPD;

            projectileInstance.Damage =
                _attack.ProjectiveDamage * statManager.ATK;

            projectileInstance.transform.SetPositionAndRotation(
                Player.transform.position,
                Quaternion.Euler(0f, 0f, angle)
            );
        }
    }
}