using System.Collections;
using DevLib.ObjectPool.Runtime;
using UnityEngine;

namespace JJM.Scripts.Player
{
    public class PlayerProjective : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private Coroutine _lifeTimeCoroutine;

        public PoolItemSO PoolItem { get; set; }

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
                transform.right * (speed * Time.fixedDeltaTime);
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