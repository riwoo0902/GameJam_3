using DevLib.BattleSystem;
using DevLib.ModuleSystem;

namespace Agents
{
    public abstract class Agent : ModuleOwner
    {
        /*public bool IsDead { get; set; }

        public UnityEvent OnHit;
        public UnityEvent OnDeath;

        public HealthModule Health { get; private set; }
        public ActionDataModule ActionData { get; private set; }

        public override void InitializeModules()
        {
            base.InitializeModules();
            Health = GetModule<HealthModule>();
            ActionData = GetModule<ActionDataModule>();
        }

        public override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
//            Health.OnDead += HandleDeath;
            OnHit.AddListener(HandleHitEvent);
        }
        
        protected virtual void HandleHitEvent() { }

        protected virtual void OnDestroy()
        {
  //          Health.OnDead -= HandleDeath;
            OnHit.RemoveListener(HandleHitEvent);
        }

        protected virtual void HandleDeath()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }

        public void ApplyDamage(DamageData damageData, Vector2 hitPoint, Vector2 hitNormal, bool isCritical = false)
        {
            if (IsDead) return;
            if (ActionData != null)
            {
                ActionData.HitPoint = hitPoint;
                ActionData.HitNormal = hitNormal;
                ActionData.Attacker = damageData.Dealer;
            }

            if (Health != null)
            {
                Health.TakeDamage(damageData.DamageAmount);
            }

            OnHit?.Invoke();
        }*/
    }
}