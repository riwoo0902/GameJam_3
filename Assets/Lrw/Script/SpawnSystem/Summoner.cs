using System;
using System.Collections;
using DevLib.ObjectPool.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lrw.Script.SpawnSystem
{
    public class Summoner : MonoBehaviour
    {
        [SerializeField] private SummonDataSo[] summonDataList;
        
        private void Awake()
        {
            Summon();
        }

        private void Summon()
        {
            foreach (SummonDataSo summonData in summonDataList)
            {
                StartCoroutine(SummonCoroutine(summonData));
            }
        }

        private IEnumerator SummonCoroutine(SummonDataSo summonData)
        {
            yield return new WaitForSeconds(summonData.StartDelay);

            do
            {
                for (int i = 0; i < summonData.Count; i++)
                {
                    GameObject go = Instantiate(summonData.Prefab);
                    go.transform.position = (Vector2)(Vector3)summonData.Pos + Random.insideUnitCircle * summonData.RandomRange;
                }
                yield return new WaitForSeconds(summonData.LoopDelay);
            } while (summonData.Loop);
        }


        private void OnDrawGizmosSelected()
        {
            if(summonDataList == null) return;
            
            Gizmos.color = Color.green;
            
            foreach (SummonDataSo summonData in summonDataList) Gizmos.DrawWireSphere(summonData.Pos, Mathf.Max(summonData.RandomRange,0.3f));
        }
        
        
        
    }
}