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
        
        [Header("Loop")]
        [field: SerializeField] public int LoopCount { get; private set; } = 0;
        [field: SerializeField] public int LoopDelay { get; private set; } = 0;
    }
}