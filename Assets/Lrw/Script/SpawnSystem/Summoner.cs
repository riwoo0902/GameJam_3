using System;
using System.Collections;
using System.Collections.Generic;
using DevLib.ModuleSystem;
using DevLib.ObjectPool.Runtime;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Lrw.Script.SpawnSystem
{
    public class Summoner : MonoBehaviour
    {
        [SerializeField] private SummonDataSo[] summonDataList;

        private int allEnemyCount;
        private int currentDieCount = 0;

        public UnityEvent StageClear;
        
        private void Awake()
        {
            Summon();
        }

        private void Summon()
        {
            allEnemyCount = 0;
            foreach (SummonDataSo summonData in summonDataList)
            {
                StartCoroutine(SummonCoroutine(summonData));
                allEnemyCount += summonData.Count * summonData.LoopCount;
            }
        }

        private IEnumerator SummonCoroutine(SummonDataSo summonData)
        {
            yield return new WaitForSeconds(summonData.StartDelay);
            
            for (int loop = 0; loop < summonData.LoopCount; loop++)
            {
                for (int i = 0; i < summonData.Count; i++)
                {
                    CreateGameObject(summonData);
                }
                yield return new WaitForSeconds(summonData.LoopDelay);
            }
        }

        private void CreateGameObject(SummonDataSo data)
        {
            GameObject go = Instantiate(data.Prefab);
            go.transform.position = (Vector2)(Vector3)data.Pos + Random.insideUnitCircle * data.RandomRange;
            if (go.TryGetComponent(out ModuleOwner owner))
            {
                if (owner.TryGetModule(out HealthModule health))
                {
                    health.OnDie.AddListener(EnemyDie);
                }
            }
        }

        private void EnemyDie()
        {
            currentDieCount += 1;
            if (allEnemyCount == currentDieCount)
            {
                StageClear?.Invoke();
            }
        }


        private void OnDrawGizmosSelected()
        {
            if(summonDataList == null) return;
            
            Gizmos.color = Color.green;
            
            foreach (SummonDataSo summonData in summonDataList) Gizmos.DrawWireSphere(summonData.Pos, Mathf.Max(summonData.RandomRange,0.3f));
        }
        
        
        
    }
}