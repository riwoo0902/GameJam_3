namespace Lrw.Script.UI.SoundUI
{
    public enum SoundType
    {
        Master,Bgm,Sfx
    }
    
    public static class SoundUIEvent
    {
        public static SoundValueChangeEvent SoundValueChange = new();
        
    }

    public class SoundValueChangeEvent
    {
        public SoundType Type { get; private set; }
        public float NewValue { get; private set; }

        public SoundValueChangeEvent Init(SoundType type, float newValue)
        {
            this.Type = type;
            NewValue = newValue;
            return this;
        }
    }
    
    
}