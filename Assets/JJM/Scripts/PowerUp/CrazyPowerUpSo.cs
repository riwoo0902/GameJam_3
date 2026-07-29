using JJM.Scripts.Players.Stats;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace JJM.Scripts.PowerUp
{
    [CreateAssetMenu(
        fileName = "crazy power up",
        menuName = "PowerUp/Crazy",
        order = 0)]
    public class CrazyPowerUpSo : AbstractPowerUpSo
    {
        [SerializeField] private float triggerHealth = 50f;
        [SerializeField] private float plusAttack = 0.75f;
        [SerializeField] private float plusAttackSpeed = 0.5f;

        private HealthModule _health;

        private bool _isSubscribed;

        private bool _isCrazy;

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

            _health.OnHealthChanged += HandleHalfHealthTrigger;
            _isSubscribed = true;
        }

        private bool TryInitializeModules()
        {
            if (_health == null)
            {
                _health = Player.GetModule<HealthModule>();
            }
            
            if (_health == null)
            {
                Debug.LogError($"{name}: HealthModule 모듈을 찾지 못했습니다.");
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
            if (_isSubscribed && _health != null)
            {
                _health.OnHealthChanged -= HandleHalfHealthTrigger;
            }

            _isSubscribed = false;
            _health = null;
        }

        private void HandleHalfHealthTrigger(
            float currentHealth,
            float prevHealth,
            float maxHealth)
        {
            if (Player == null || _health == null)
            {
                Unsubscribe();
                return;
            }

            PlayerStatManager statManager = PlayerStatManager.Instance;

            if (statManager == null)
            {
                return;
            }

            if (currentHealth <= triggerHealth && !_isCrazy)
            {
                statManager.ATK += plusAttack;
                statManager.ATS += plusAttackSpeed;

                _isCrazy = true;
            }
            else if (currentHealth > triggerHealth && _isCrazy)
            {
                statManager.ATK -= plusAttack;
                statManager.ATS -= plusAttackSpeed;

                _isCrazy = false;
            }
        }
    }
}