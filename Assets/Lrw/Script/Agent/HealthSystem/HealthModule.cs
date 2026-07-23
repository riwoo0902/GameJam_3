using DevLib.ModuleSystem;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Lrw.Script.Agent.HealthSystem
{
    public delegate void HealthChangedHandler(float currentHealth,float prevHealth, float maxHealth);
    
    public class HealthModule : Module,IAfterInitModule, IHealthModule
    {
        [SerializeField] private StatDataSo maxHealthStatData;
        
        [SerializeField] private float currentHealth;
        
        private IStatModule _statModule;
        
        private Stat _maxStat;
        
        public float CurrentHealth
        {
            get => currentHealth;
            set => HealthChange(value);
        }
        
        private float MaxHealth => _maxStat.Value;
        
        public event HealthChangedHandler OnHealthChanged;
        public UnityEvent OnTakeDamage;
        public UnityEvent OnDie;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null,"StatModule is not found");
        }

        public void AfterInit()
        {
            _maxStat = _statModule.GetStat(maxHealthStatData);
            Debug.Assert(_maxStat != null, "StatModule is not found");

            currentHealth = MaxHealth;
            _maxStat.OnValueChanged += MaxHealthStatChanged;
        }
        
        private void OnDestroy()
        {
            _maxStat.OnValueChanged -= MaxHealthStatChanged;
        }
        
        private void MaxHealthStatChanged(float currentValue, float prevValue)
        {
            float delta = currentValue - prevValue;
            CurrentHealth = Mathf.Clamp(CurrentHealth + delta,1f,MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }
        
        private void HealthChange(float newHealth)
        { 
            float prevHealth = CurrentHealth;
            currentHealth = Mathf.Clamp(newHealth,0,MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth,prevHealth, MaxHealth);

            if (prevHealth > currentHealth)
            {
                OnTakeDamage?.Invoke();
            }

            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke();
            }
        }
    }
}