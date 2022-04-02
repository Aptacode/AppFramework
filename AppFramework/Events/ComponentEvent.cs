using System.Numerics;

namespace Aptacode.AppFramework.Events;

public abstract record ComponentEvent
{
}

public abstract record TransformationEvent : ComponentEvent
{
}

public record TranslateEvent(Vector2 Delta) : TransformationEvent;

public record RotateEvent(Vector2 Point, float Theta) : TransformationEvent;

public record ScaleEvent(Vector2 Point, Vector2 Delta) : TransformationEvent;

public record SkewEvent(Vector2 Delta) : TransformationEvent;