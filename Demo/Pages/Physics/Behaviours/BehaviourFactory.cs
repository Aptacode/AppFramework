using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;

public static class BehaviourFactory
{
    public static PrimitiveComponent AddDragToMove(this PrimitiveComponent component, Scene scene)
    {
        component.AddPlugin(new DragBehaviour(scene, component));
        return component;
    }

    public static ScenePhysicsBehaviour AddPhysics(this Scene scene)
    {
        var behaviour = new ScenePhysicsBehaviour(scene);
        scene.Plugins.Add(behaviour);
        return behaviour;
    }
}