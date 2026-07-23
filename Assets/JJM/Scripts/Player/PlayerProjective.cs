using System.Collections;
using DevLib.ObjectPool.Runtime;
using UnityEngine;

namespace JJM.Scripts.Players
{
    public class PlayerProjective : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public float Speed { get; set; } = 10f;
        [field: SerializeField] public float Damage { get; set; } = 3f;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float lifeTime = 10f;

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