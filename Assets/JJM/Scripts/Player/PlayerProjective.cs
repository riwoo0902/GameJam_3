using System;
using System.Collections;
using DevLib.ModuleSystem;
using DevLib.ObjectPool.Runtime;
using JJM.Scripts.NewEnemys;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace JJM.Scripts.Players
{
    public class PlayerProjective : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public float Speed { get; set; } = 10f;
        [field: SerializeField] public float Damage { get; set; } = 3f;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float lifeTime = 10f;
        [SerializeField] private LayerMask layerMask;

        private Coroutine _lifeTimeCoroutine;

        [field: SerializeField] public PoolItemSO PoolItem { get; set; }

        public GameObject GameObject => gameObject;

        public void ResetItem()
        {
            transform.localRotation = Quaternion.identity;

            if (_lifeTimeCoroutine != null)
            {
                StopCoroutine(_lifeTimeCoroutine);
            }

            _lifeTimeCoroutine = StartCoroutine(LifeTimeCount());
        }

        private void FixedUpdate()
        {
            transform.position +=
                transform.right * (Speed * Time.fixedDeltaTime);
        }
        

        private void OnTriggerEnter2D(Collider2D other)
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
                healthModule.TakeDamage(Damage);
                poolManager.Push(this);   
            }
        }

        private IEnumerator LifeTimeCount()
        {
            yield return new WaitForSeconds(lifeTime);

            _lifeTimeCoroutine = null;
            poolManager.Push(this);
        }

        private void OnDisable()
        {
            _lifeTimeCoroutine = null;
        }
    }
}