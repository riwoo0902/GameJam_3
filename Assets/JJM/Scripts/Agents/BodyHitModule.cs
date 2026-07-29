using DevLib.ModuleSystem;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;
using UnityEngine.Events;

namespace JJM.Scripts.Agents
{
    public class BodyHitModule : Module
    {
        [SerializeField] private float damage;
        [SerializeField] private LayerMask layerMask;
        public UnityEvent OnAttack;

        private void OnTriggerStay2D(Collider2D other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            if (!other.TryGetComponent(out ModuleOwner player))
            {
                return;
            }

            HealthModule healthModule = player.GetModule<HealthModule>();
            
            if (healthModule != null)
            {
                OnAttack?.Invoke();
                healthModule.TakeDamage(damage);
            }
        }
    }
}