using System;
using UnityEngine;

namespace Lrw.Script.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [field: SerializeField] public StatDataSo statSo;
        [field: SerializeField] public int value;
        
        
    }
}