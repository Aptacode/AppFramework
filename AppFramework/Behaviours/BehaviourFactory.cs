using Aptacode.AppFramework.Behaviours.Tick;
using Aptacode.AppFramework.Behaviours.Ui;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Behaviours;

public static class BehaviourFactory
{
    public static Component AddDragToMove(this Component component, Scene.Scene scene)
    {
        component.AddUiBehaviour(new DragBehaviour(scene, component));
        return component;
    }

    public static Component AddPhysics(this Component component, Scene.Scene scene)
    {
        component.AddTickBehaviour(new PhysicsBehaviour(scene, component));
        return component;
    }
}