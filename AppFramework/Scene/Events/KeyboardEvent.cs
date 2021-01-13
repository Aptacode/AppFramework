namespace Aptacode.AppFramework.Scene.Events
{
    public abstract record KeyboardEvent : UIEvent
    {
        public readonly string Key;

        protected KeyboardEvent(string key)
        {
            Key = key;
        }
    }

    public record KeyDownEvent : KeyboardEvent
    {
        public KeyDownEvent(string key) : base(key)
        {
        }
    }

    public record KeyUpEvent : KeyboardEvent
    {
        public KeyUpEvent(string key) : base(key)
        {
        }
    }
}