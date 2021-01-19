using System.Numerics;

namespace Aptacode.AppFramework.Scene.Events
{
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
}