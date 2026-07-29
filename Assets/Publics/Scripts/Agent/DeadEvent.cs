using UnityEngine;
using System.Collections;
using JJM.Scripts.Players.Stats;

namespace Publics.Scripts.Agent
{
    public class DeadEvent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private bool destroy = true;
        
        public void DeadEventPlay()
        {
            PlayerStatManager.Instance.PlayerHealthModule.CurrentHealth += 5;
            GameObject p = transform.parent.parent.gameObject;
            
            transform.SetParent(null);
            particles.Play();
            StartCoroutine(LifetimeCoolDown(p));
            
        }

        private IEnumerator LifetimeCoolDown(GameObject p)
        {
            yield return null;
            if (destroy) Destroy(p);
            else p.SetActive(false);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}