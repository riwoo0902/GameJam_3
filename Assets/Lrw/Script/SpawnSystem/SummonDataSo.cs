using System;
using LrwLib.UnityPosition;
using UnityEngine;

namespace Lrw.Script.SpawnSystem
{
    [CreateAssetMenu(fileName = "Spawn Data", menuName = "Spawn Data", order = 0)]
    public class SummonDataSo : ScriptableObject
    {
        [field:SerializeField] public UnityPos Pos { get; private set; }
        [field:SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int Count { get; private set; } = 1;

        [field:SerializeField] public float RandomRange { get; private set; } = 1;
        [field:SerializeField] public float StartDelay { get; private set; } = 0;
        
        [field: SerializeField] public int LoopCount { get; private set; } = 0;
        [field: SerializeField] public float LoopDelay { get; private set; } = 0;


        private void OnValidate()
        {
            if (Count < 1) Count = 1;
            
            if (LoopCount < 1) LoopCount = 1;
            
            if(Prefab ==  null) Debug.LogWarning("Prefab is null");
            
            if(RandomRange < 0) RandomRange = 0;

            if (StartDelay < 0) StartDelay = 0;
            
            if(LoopDelay < 0)LoopDelay = 0;


        }
    }
}