using Aptacode.AppFramework.Behaviours.Tick;
using Aptacode.AppFramework.Behaviours.Transformation;
using Aptacode.AppFramework.Behaviours.Ui;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Behaviours;

public static class BehaviourFactory
{
    public static ComponentViewModel AddCollisions(this ComponentViewModel component, Scene.Scene scene)
    {
        component.AddTransformationBehaviour(new CollisionBehaviour(scene, component));
        return component;
    }

    public static ComponentViewModel AddDragToMove(this ComponentViewModel component, Scene.Scene scene)
    {
        component.AddUiBehaviour(new DragBehaviour(scene, component));
        return component;
    }

    public static ComponentViewModel AddVelocity(this ComponentViewModel component, Scene.Scene scene)
    {
        component.AddTickBehaviour(new PhysicsBehaviour(scene, component));
        return component;
    }
}