using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components;

public abstract class UiBehaviour
{
    public abstract bool HandleEvent(Scene.Scene scene, ComponentViewModel component, UiEvent uiEvent);
}

public abstract class TransformationBehaviour
{
    public abstract bool HandleEvent(Scene.Scene scene, ComponentViewModel component, TransformationEvent transformationEvent);
}

public class DragBehaviour : UiBehaviour
{
    public bool IsDragging { get; set; }
    public bool CanDrag { get; set; } = true;
    public Vector2 LastDragPosition { get; set; }

    public override bool HandleEvent(Scene.Scene scene, ComponentViewModel component, UiEvent uiEvent)
    {
        if (uiEvent is not MouseEvent mouseEvent) return false;

        switch (mouseEvent)
        {
            case MouseUpEvent mouseUpEvent:
                if (IsDragging)
                {
                    IsDragging = false;
                    return true;
                }
                break;
            case MouseMoveEvent mouseMoveEvent:
                if (IsDragging)
                {
                    var delta = mouseMoveEvent.Position - LastDragPosition;
                    component.Translate(scene, delta, true);

                    LastDragPosition = mouseMoveEvent.Position;
                    return true;
                }
                break;

            case MouseDownEvent mouseDownEvent:
                if (CanDrag && component.CollidesWith(mouseEvent.Position))
                {
                    LastDragPosition = mouseDownEvent.Position;
                    IsDragging = true;
                    return true;
                }
                break;
        }

        return false;
    }
}

public class CollisionBehaviour : TransformationBehaviour
{
    public bool CollisionDetectionEnabled { get; set; } = true;

    public override bool HandleEvent(Scene.Scene scene, ComponentViewModel component, TransformationEvent transformationEvent)
    {
        if (transformationEvent is not TranslateEvent translationEvent) return false;

        if (!translationEvent.Source) return false;

        scene.Translated(component, translationEvent.Delta, new List<ComponentViewModel>(){ component }, new CancellationTokenSource());
        return true;
    }
}