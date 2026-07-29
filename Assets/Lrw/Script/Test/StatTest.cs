using System;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;

namespace Lrw.Script.Test
{
    public class StatTest : MonoBehaviour
    {
        [SerializeField] private StatModule statModule;
        [SerializeField] private StatDataSo targetStat;
        [SerializeField] private float value = 10;
        
        
        [ContextMenu("AddValue")]
        public void AddValue()
        {
            statModule.GetStat(targetStat).AddModifier(this,value);
        }
        
        [ContextMenu("RemoveValue")]
        public void RemoveValue()
        {
            statModule.GetStat(targetStat).RemoveModifier(this);
        }


        [Header("Debug")] 
        [SerializeField] private float debugValue;

        private void Update()
        {
            debugValue = statModule.GetStat(targetStat).Value;
        }
        
        
    }
}