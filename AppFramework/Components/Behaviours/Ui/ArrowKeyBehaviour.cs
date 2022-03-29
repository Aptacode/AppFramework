using System.Numerics;
using Aptacode.AppFramework.Components.States.Component;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Behaviours.Ui;

public class ArrowKeyBehaviour : UiBehaviour
{
    public ArrowKeyBehaviour(AppFramework.Scene.Scene scene, Component component) : base(scene, component)
    {
    }

    public override bool HandleEvent(UiEvent uiEvent)
    {
        if (uiEvent is not KeyDownEvent keyEvent)
        {
            return false;
        }

        var physicsState = Component.GetState<PhysicsState>();
        if (physicsState == null)
        {
            return false;
        }

        switch (keyEvent.Key)
        {
            case "ArrowUp":
                physicsState.ApplyForce(0.01f);
                break;
            case "ArrowDown":
                physicsState.ApplyForce(-0.01f);
                break;
            case "ArrowLeft":
                physicsState.ApplyForce(new Vector2(-0.01f, 0));
                break;
            case "ArrowRight":
                physicsState.ApplyForce(new Vector2(0.01f, 0));
                break;
        }

        return false;
    }

    #region Properties

    public bool IsDragging { get; set; }
    public Vector2 LastDragPosition { get; set; }

    #endregion
}