using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Behaviours.Ui;

public class DragBehaviour : UiBehaviour
{
    public DragBehaviour(Scene.Scene scene, ComponentViewModel component) : base(scene, component,
        nameof(DragBehaviour))
    {
    }

    public override bool HandleEvent(UiEvent uiEvent)
    {
        if (uiEvent is not MouseEvent mouseEvent) return false;

        switch (mouseEvent)
        {
            case MouseDownEvent mouseDownEvent:
                if (Enabled && Component.CollidesWith(mouseEvent.Position))
                {
                    LastDragPosition = mouseDownEvent.Position;
                    IsDragging = true;
                    return true;
                }

                break;

            case MouseMoveEvent mouseMoveEvent:
                if (IsDragging)
                {
                    var delta = mouseMoveEvent.Position - LastDragPosition;
                    Component.Translate(delta, true);

                    LastDragPosition = mouseMoveEvent.Position;
                    return true;
                }

                break;
            case MouseUpEvent mouseUpEvent:
                if (IsDragging)
                {
                    IsDragging = false;
                    return true;
                }

                break;
        }

        return false;
    }

    #region Properties

    public bool IsDragging { get; set; }
    public Vector2 LastDragPosition { get; set; }

    #endregion
}