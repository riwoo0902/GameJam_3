using System;
using System.Collections.Generic;
using Lrw.Script.CoreSystem;
using UnityEngine;

namespace Lrw.Script.StatSystem
{
    public class Stat
    {
        public StatDataSo StatDataSo { get; private set; }
        
        public float Value => Mathf.Clamp(_baseValue + _modifierValue,StatDataSo.MinValue, StatDataSo.MaxValue);
        
        private readonly Dictionary<KeySo,float> _modifiers = new();
        
        private readonly float _baseValue;
        private float _modifierValue;
        
        public Stat(StatDataSo dataSo, int baseValue)
        {
            if(dataSo == null) throw new Exception("StatSo cannot be null");
            StatDataSo = dataSo;
            _baseValue = baseValue;
        }
        
        public void AddModifier(KeySo modifier, float value)
        {
            if(modifier == null) return;
            if(!_modifiers.TryAdd(modifier, value)) return;
            _modifierValue += value;
        }

        public void RemoveModifier(KeySo modifier)
        {
            if(modifier == null) return;
            if (!_modifiers.TryGetValue(modifier, out float value)) return;
            _modifierValue -= value;
            _modifiers.Remove(modifier);
        }
        
        
    }
}