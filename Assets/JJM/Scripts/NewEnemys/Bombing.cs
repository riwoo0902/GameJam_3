using UnityEngine;
using System.Collections;
using Lrw.Script.Agent.HealthSystem;

namespace JJM.Scripts.NewEnemys
{
    public class Bombing : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private GameObject caster;
        [SerializeField] private GameObject owner;
        
        public void Explosion()
        {
            transform.SetParent(null);
            particleSystem.Play();
            StartCoroutine(Bomb());
            owner.GetComponentInChildren<HealthModule>().CurrentHealth = 0;
        }

        private IEnumerator Bomb()
        {
            caster.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            caster.SetActive(false);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}