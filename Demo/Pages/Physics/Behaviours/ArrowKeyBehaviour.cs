using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Demo.Pages.Physics.States;
using Aptacode.AppFramework.Events;
using Aptacode.AppFramework.Plugins.Behaviours;

namespace Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;

public class ArrowKeyBehaviour : PrimitiveComponentPlugin
{
    public static string BehaviourName = "ArrowForce";

    public ArrowKeyBehaviour(Scene scene, PrimitiveComponent component) : base(scene, component)
    {
    }

    public override bool Handle(UiEvent uiEvent)
    {
        if (uiEvent is not KeyDownEvent keyEvent)
        {
            return false;
        }

        var physicsState = Component.Plugins.Get<PhysicsState>(PhysicsState.StateName);
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

    public override string Name()
    {
        return BehaviourName;
    }

    #region Properties

    public bool IsDragging { get; set; }
    public Vector2 LastDragPosition { get; set; }

    #endregion
}