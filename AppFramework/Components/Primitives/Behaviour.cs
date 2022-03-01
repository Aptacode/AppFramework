using System.Numerics;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Primitives;

public abstract class Behaviour
{
    public abstract bool HandleEvent(ComponentViewModel component, UiEvent uiEvent);
}

public class DragBehaviour : Behaviour
{
    public bool IsDragging { get; set; }
    public bool CanDrag { get; set; } = true;
    public Vector2 LastDragPosition { get; set; }

    public override bool HandleEvent(ComponentViewModel component, UiEvent uiEvent)
    {
        if (uiEvent is not MouseEvent mouseEvent) return false;

        if (IsDragging && mouseEvent is MouseUpEvent) IsDragging = false;

        switch (mouseEvent)
        {
            case MouseMoveEvent mouseMoveEvent:
                if (IsDragging)
                {
                    var delta = mouseMoveEvent.Position - LastDragPosition;
                    component.Translate(delta);
                    LastDragPosition = mouseMoveEvent.Position;
                }

                break;
            case MouseDownEvent mouseDownEvent:
                if (CanDrag && component.CollidesWith(mouseEvent.Position))
                {
                    LastDragPosition = mouseDownEvent.Position;
                    IsDragging = true;
                }

                break;
        }

        return true;
    }
}