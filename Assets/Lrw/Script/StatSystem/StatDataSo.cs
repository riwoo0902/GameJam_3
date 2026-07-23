using UnityEngine;

namespace Lrw.Script.StatSystem
{
    [CreateAssetMenu(fileName = "StatSo", menuName = "Agent/StatSystem/", order = 0)]
    public class StatDataSo : ScriptableObject
    {
        [field:SerializeField] public Sprite StatIcon{get; private set;}
        
        [field:SerializeField] public float MinValue{get; private set;}
        [field:SerializeField] public float MaxValue{get; private set;}
        
        
    }
}