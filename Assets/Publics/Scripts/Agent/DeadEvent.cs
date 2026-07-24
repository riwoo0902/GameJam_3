using UnityEngine;
using System.Collections;

namespace Publics.Scripts.Agent
{
    public class DeadEvent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private bool destroy = true;
        
        public void DeadEventPlay()
        {
            GameObject p = transform.parent.parent.gameObject;
            
            transform.SetParent(null);
            particles.Play();
            StartCoroutine(LifetimeCoolDown());
            if (destroy) Destroy(p);
            p.gameObject.SetActive(false);
        }

        private IEnumerator LifetimeCoolDown()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}