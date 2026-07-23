using System;

namespace Lrw.Script.CoreSystem.StaticDataSystem
{
    public struct StaticData
    {
        public Type Type { get; private set; }
        public object Value { get; private set; }
        
        public StaticData(Type type,object value)
        {
            Type = type;
            Value = value;
        }
        
    }
}