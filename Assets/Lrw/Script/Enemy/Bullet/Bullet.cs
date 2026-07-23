using JJM.Scripts.Player;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.Bullet
{
    public class Bullet : MonoBehaviour
    {
        
        
        private float _damage;
        
        
        public void SetDamage(float damage)
        {
            _damage = damage;
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player owner))
            {
                if (owner.TryGetModule(out IHealthModule healthModule))
                {
                    healthModule.TakeDamage(_damage);
                }
            }
            Destroy(gameObject);
        }
        
        
        
    }
}