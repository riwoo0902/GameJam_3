using System;
using UnityEngine;

namespace Lrw.Script.Agent.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [field: SerializeField] public StatDataSo StatSo { get; private set; }
        [field: SerializeField] public int BaseValue { get; private set; }
        
        
    }
}