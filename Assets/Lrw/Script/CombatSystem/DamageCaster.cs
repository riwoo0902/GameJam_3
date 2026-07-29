using System.Linq;
using DevLib.ModuleSystem;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace Lrw.Script.CombatSystem
{
    public abstract class DamageCaster : MonoBehaviour,IDamageCaster
    {
        protected abstract Collider2D[] GetColliders();

        public void Cast(float power)
        {
            Collider2D[] arr = GetColliders();
            IHealthModule[] healthArr = arr
                .Where(x => x != null)
                .Select(x => x.GetComponent<ModuleOwner>())
                .Where(x => x != null)
                .Select(x => x.GetModule<IHealthModule>())
                .ToArray();

            foreach (IHealthModule health in healthArr)
            {
                health.TakeDamage(power);
            }
        }
        
        
    }
}