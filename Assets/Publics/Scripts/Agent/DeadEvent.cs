using UnityEngine;
using System.Collections;

namespace Publics.Scripts.Agent
{
    public class DeadEvent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        
        public void DeadEventPlay()
        {
            GameObject p = transform.parent.parent.gameObject;
            
            transform.SetParent(null);
            particles.Play();
            StartCoroutine(LifetimeCoolDown());
            Destroy(p);
        }

        private IEnumerator LifetimeCoolDown()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}