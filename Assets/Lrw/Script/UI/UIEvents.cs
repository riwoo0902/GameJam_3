namespace Lrw.Script.UI
{
    public static class UIEvents
    {
        public static readonly MenuEvent  Menu = new();
        
        
    }

    public class MenuEvent
    {
        public bool Active { get; private set; }

        public MenuEvent Init(bool active)
        {
            Active = active;
            return this;
        }
        
    }
}