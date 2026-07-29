using System;
using DevLib.ModuleSystem;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace Lrw.Script.CombatSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageObject : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out ModuleOwner moduleOwner))
            {
                if (moduleOwner.TryGetModule(out IHealthModule health))
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}