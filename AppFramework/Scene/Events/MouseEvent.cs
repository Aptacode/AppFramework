using System.Numerics;

namespace Aptacode.AppFramework.Scene.Events
{
    public abstract record MouseEvent : UIEvent
    {
        public readonly Vector2 Position;

        protected MouseEvent(Vector2 position)
        {
            Position = position;
        }
    }

    public record MouseDownEvent : MouseEvent
    {
        public MouseDownEvent(Vector2 position) : base(position)
        {
        }
    }

    public record MouseUpEvent : MouseEvent
    {
        public MouseUpEvent(Vector2 position) : base(position)
        {
        }
    }

    public record MouseClickEvent : MouseEvent
    {
        public MouseClickEvent(Vector2 position) : base(position)
        {
        }
    }

    public record MouseMoveEvent : MouseEvent
    {
        public MouseMoveEvent(Vector2 position) : base(position)
        {
        }
    }

    public record MouseDoubleClickEvent : MouseEvent
    {
        public MouseDoubleClickEvent(Vector2 position) : base(position)
        {
        }
    }
}