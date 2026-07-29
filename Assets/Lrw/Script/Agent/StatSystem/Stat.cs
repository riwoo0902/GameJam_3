using System;
using System.Collections.Generic;
using Lrw.Script.CoreSystem;
using UnityEngine;

namespace Lrw.Script.Agent.StatSystem
{
    public class Stat
    {
        public StatDataSo StatDataSo { get; private set; }
        
        public float Value => Mathf.Clamp(_baseValue + _modifierValue,StatDataSo.MinValue, StatDataSo.MaxValue);
        
        private readonly Dictionary<object,float> _modifiers = new();
        
        private readonly float _baseValue;
        private float _modifierValue;
        
        public delegate void ValueChangeHandler(float currentValue, float prevValue);
        
        public event ValueChangeHandler OnValueChanged;
        
        public Stat(StatDataSo dataSo, float baseValue)
        {
            if(dataSo == null) throw new Exception("StatSo cannot be null");
            StatDataSo = dataSo;
            _baseValue = baseValue;
        }
        
        public void AddModifier(object modifier, float value)
        {
            if(modifier == null) return;
            if(!_modifiers.TryAdd(modifier, value)) return;
            float prevValue = Value;
            _modifierValue += value;
            OnValueChanged?.Invoke(Value, prevValue);
        }

        public void RemoveModifier(object modifier)
        {
            if(modifier == null) return;
            if (!_modifiers.TryGetValue(modifier, out float value)) return;
            _modifiers.Remove(modifier);
            
            float prevValue = Value;
            _modifierValue -= value;
            OnValueChanged?.Invoke(Value, prevValue);
        }
        
        
    }
}