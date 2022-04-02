using System.Numerics;

namespace Aptacode.AppFramework.Events;

public abstract record MouseEvent(Vector2 Position) : UiEvent
{
    public readonly Vector2 Position = Position;

    public override string ToString()
    {
        return $"{GetType()} ({Position.X},{Position.Y})";
    }
}

public record MouseDownEvent(Vector2 Position) : MouseEvent(Position);

public record MouseUpEvent(Vector2 Position) : MouseEvent(Position);

public record MouseClickEvent(Vector2 Position) : MouseEvent(Position);

public record MouseMoveEvent(Vector2 Position) : MouseEvent(Position);

public record MouseDoubleClickEvent(Vector2 Position) : MouseEvent(Position);

public record MouseEnterEvent(Vector2 Position) : MouseEvent(Position);

public record MouseLeaveEvent(Vector2 Position) : MouseEvent(Position);