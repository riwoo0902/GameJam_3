using System.Collections.Generic;

namespace Lrw.Script.CoreSystem
{
    public static class StaticDataManager
    {
        private static Dictionary<object,object> _objects = new();
        
        public static void Binding<T>(object key, T value)
        {
            _objects[key] = value;
        }

        public static T Get<T>(object key)
        {
            if (_objects.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return default;
        }
        
    }
}