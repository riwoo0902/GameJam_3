using UnityEngine;
using System.Collections;

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
            Destroy(owner);
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