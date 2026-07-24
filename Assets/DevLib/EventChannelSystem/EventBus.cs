using System;

namespace DevLib.EventChannelSystem
{
    public static class EventBus<T>
    {
        public static event Action<T> Event;
        public static void Invoke(T value) => Event?.Invoke(value);
    }
}