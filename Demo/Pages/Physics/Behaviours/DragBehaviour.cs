using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Events;
using Aptacode.AppFramework.Plugins.Behaviours;

namespace Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;

public class DragBehaviour : PrimitiveComponentPlugin
{
    public static string BehaviourName = "DragForce";

    public DragBehaviour(Scene scene, PrimitiveComponent component) : base(scene, component)
    {
    }

    public override bool Handle(UiEvent uiEvent)
    {
        if (uiEvent is not MouseEvent mouseEvent)
        {
            return false;
        }

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
                    Component.AddTranslation(delta);

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

    public override string Name()
    {
        return BehaviourName;
    }

    #region Properties

    public bool IsDragging { get; set; }
    public Vector2 LastDragPosition { get; set; }

    #endregion
}