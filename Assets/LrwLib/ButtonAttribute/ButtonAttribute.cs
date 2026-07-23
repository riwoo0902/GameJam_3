using System;

namespace LrwLib.ButtonAttribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ButtonAttribute : Attribute
    {
        public string Label { get; }

        public ButtonAttribute()
        {
        }

        public ButtonAttribute(string label)
        {
            Label = label;
        }
    }
}
