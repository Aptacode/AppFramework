using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;

public static class BehaviourFactory
{
    public static Component AddDragToMove(this Component component, Scene scene)
    {
        component.Plugins.Add(new DragBehaviour(scene, component));
        return component;
    }

    public static ScenePhysicsBehaviour AddPhysics(this Scene scene)
    {
        var behaviour = new ScenePhysicsBehaviour(scene);
        scene.Plugins.Add(behaviour);
        return behaviour;
    }
}