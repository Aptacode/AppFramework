using Aptacode.AppFramework.Components;
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

    public abstract record DragDropEvent : ComponentEvent
    {
    }

    public record DragEvent : DragDropEvent
    {
        public ComponentViewModel Component { get; set; }
        public Vector2 Position { get; set; }
        public DragEvent(ComponentViewModel component, Vector2 position)
        {
            Component = component;
            Position = position;
        }
    }

    public record DropEvent : DragDropEvent
    {
        public ComponentViewModel Component { get; set; }
        public Vector2 Position { get; set; }
        public DropEvent(ComponentViewModel component, Vector2 position)
        {
            Component = component;
            Position = position;
        }
    }

    public record DropFailedEvent : DragDropEvent
    {
        public ComponentViewModel Component { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }

        public DropFailedEvent(ComponentViewModel component, Vector2 startPosition, Vector2 endPosition)
        {
            Component = component;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}