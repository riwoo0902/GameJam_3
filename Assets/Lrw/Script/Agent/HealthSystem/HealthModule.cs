using DevLib.ModuleSystem;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Lrw.Script.Agent.HealthSystem
{
    public delegate void HealthChangedHandler(
        float currentHealth,
        float prevHealth,
        float maxHealth);

    public class HealthModule : Module, IAfterInitModule, IHealthModule
    {
        [SerializeField] private StatDataSo maxHealthStatData;
        [SerializeField] private float currentHealth;

        [Header("Invincible")]
        [SerializeField] private float invincibleDuration = 0.5f;

        private IStatModule _statModule;
        private Stat _maxStat;

        private double _invincibleUntilTime;

        public float CurrentHealth
        {
            get => currentHealth;
            set => HealthChange(value);
        }

        public bool IsInvincible =>
            Time.timeAsDouble < _invincibleUntilTime;

        private float MaxHealth => _maxStat.Value;

        public event HealthChangedHandler OnHealthChanged;

        public UnityEvent OnTakeDamage;
        public UnityEvent OnDie;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _statModule = owner.GetModule<IStatModule>();

            Debug.Assert(
                _statModule != null,
                "StatModule is not found");
        }

        public void AfterInit()
        {
            _maxStat = _statModule.GetStat(maxHealthStatData);

            Debug.Assert(
                _maxStat != null,
                "MaxHealth Stat is not found");

            currentHealth = MaxHealth;
            _maxStat.OnValueChanged += MaxHealthStatChanged;
        }

        private void OnDestroy()
        {
            if (_maxStat != null)
            {
                _maxStat.OnValueChanged -= MaxHealthStatChanged;
            }
        }

        private void MaxHealthStatChanged(
            float currentValue,
            float prevValue)
        {
            float delta = currentValue - prevValue;

            CurrentHealth = Mathf.Clamp(
                CurrentHealth + delta,
                1f,
                MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0f ||
                IsInvincible ||
                CurrentHealth <= 0f)
            {
                return;
            }

            CurrentHealth -= damage;

            _invincibleUntilTime =
                Time.timeAsDouble + invincibleDuration;
        }

        public void Heal(float heal)
        {
            CurrentHealth += heal;
        }

        private void HealthChange(float newHealth)
        {
            float prevHealth = currentHealth;

            currentHealth = Mathf.Clamp(
                newHealth,
                0f,
                MaxHealth);

            OnHealthChanged?.Invoke(
                currentHealth,
                prevHealth,
                MaxHealth);

            if (prevHealth > currentHealth)
            {
                OnTakeDamage?.Invoke();
            }

            if (currentHealth <= 0f &&
                prevHealth > 0f)
            {
                OnDie?.Invoke();
            }
        }
    }
}