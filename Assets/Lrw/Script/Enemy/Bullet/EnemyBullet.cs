using System;
using System.Collections;
using DevLib.ObjectPool.Runtime;
using JJM.Scripts.Player;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.Bullet
{
    [RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
    public class EnemyBullet : MonoBehaviour,IPoolable
    {
        private Rigidbody2D _rigidbody;
        private float _damage;
        private Vector2 _velocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        [SerializeField] private PoolManagerSO manager;
        public void Init(Vector2 velocity,float damage)
        {
            _damage = damage;
            _velocity = velocity;
            StartCoroutine(DestroyDelay());
        }

        private IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(10f);
            manager.Push(this);
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _velocity;
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
        }
        
        


        [field:SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;
        public void ResetItem()
        {
            
        }
        
    }
}