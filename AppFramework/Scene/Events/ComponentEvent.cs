using System.Numerics;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Scene.Events;

public abstract record ComponentEvent
{
}

public abstract record TransformationEvent : ComponentEvent
{
}

public record TranslateEvent : TransformationEvent
{
    public Vector2 Delta { get; set; }
}

public record RotateEvent : TransformationEvent
{
    public Vector2 Delta { get; set; }
}

public record ScaleEvent : TransformationEvent
{
    public Vector2 Delta { get; set; }
}

public record SkewEvent : TransformationEvent
{
    public Vector2 Delta { get; set; }
}

public abstract record DragDropEvent : ComponentEvent
{
}

public record DragEvent(ComponentViewModel Component, Vector2 Position) : DragDropEvent;

public record DropEvent(ComponentViewModel Component, Vector2 Position) : DragDropEvent;

public record DropFailedEvent
    (ComponentViewModel Component, Vector2 StartPosition, Vector2 EndPosition) : DragDropEvent;