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

        private Player _player;
        private PlayerAttack _attack;
        private IPlayerRotation _rotation;

        private bool _isSubscribed;

        public override void PowerUpPlay()
        {
            base.PowerUpPlay();

            if (!TryInitialize())
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

        private bool TryInitialize()
        {
            _player = FindFirstObjectByType<Player>();

            if (_player == null)
            {
                Debug.LogError($"{name}: Player를 찾지 못했습니다.");
                return false;
            }

            _attack = _player.GetModule<PlayerAttack>();
            _rotation = _player.GetModule<IPlayerRotation>();

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

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed)
            {
                return;
            }

            if (_attack != null)
            {
                _attack.OnAttack.RemoveListener(HandleThreeAttack);
            }

            _isSubscribed = false;
            _attack = null;
            _rotation = null;
            _player = null;
        }

        private void HandleThreeAttack()
        {
            if (_player == null || _attack == null || _rotation == null)
            {
                Unsubscribe();
                return;
            }

            float baseAngle =
                PlayerAttack.GetFourDirectionAngle(_rotation.MouseRelativePosition);

            SpawnProjectile(baseAngle - spreadRotation);
            SpawnProjectile(baseAngle + spreadRotation);
        }

        private void SpawnProjectile(float angle)
        {
            PlayerProjective projectileInstance =
                poolManager.Pop<PlayerProjective>(projectile);

            if (projectileInstance == null)
            {
                return;
            }

            PlayerStatManager statManager = PlayerStatManager.Instance;

            projectileInstance.Speed =
                _attack.ProjectiveSpeed * statManager.PSPD;

            projectileInstance.Damage =
                _attack.ProjectiveDamage * statManager.ATK;

            projectileInstance.transform.SetPositionAndRotation(
                _player.transform.position,
                Quaternion.Euler(0f, 0f, angle)
            );
        }
    }
}