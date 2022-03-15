using System.Numerics;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Behaviours.Ui;

public class DragBehaviour : UiBehaviour
{
    public DragBehaviour(AppFramework.Scene.Scene scene, Component component) : base(scene, component)
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
                    Component.Translate(delta);

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