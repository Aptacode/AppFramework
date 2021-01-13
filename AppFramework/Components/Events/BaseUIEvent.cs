using System.Numerics;

namespace Aptacode.AppFramework.Components.Events
{
    public abstract class BaseUIEvent
    {
    }

    public abstract class BaseMouseEvent : BaseUIEvent
    {
        public BaseMouseEvent(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; set; }
    }

    public class MouseDownEvent : BaseMouseEvent
    {
        public MouseDownEvent(Vector2 position) : base(position)
        {
        }
    }

    public class MouseUpEvent : BaseMouseEvent
    {
        public MouseUpEvent(Vector2 position) : base(position)
        {
        }
    }

    public class MouseClickEvent : BaseMouseEvent
    {
        public MouseClickEvent(Vector2 position) : base(position)
        {
        }
    }
}