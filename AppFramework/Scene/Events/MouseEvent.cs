using System.Numerics;

namespace Aptacode.AppFramework.Scene.Events;

public abstract record MouseEvent : UiEvent
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

public record MouseEnterEvent : MouseEvent
{
    public MouseEnterEvent(Vector2 position) : base(position)
    {
    }
}

public record MouseLeaveEvent : MouseEvent
{
    public MouseLeaveEvent(Vector2 position) : base(position)
    {
    }
}