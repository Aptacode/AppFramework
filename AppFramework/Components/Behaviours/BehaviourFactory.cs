using Aptacode.AppFramework.Components.Behaviours.Scene;
using Aptacode.AppFramework.Components.Behaviours.Tick;
using Aptacode.AppFramework.Components.Behaviours.Ui;

namespace Aptacode.AppFramework.Components.Behaviours;

public static class BehaviourFactory
{
    public static Component AddDragToMove(this Component component, AppFramework.Scene.Scene scene)
    {
        component.AddUiBehaviour(new DragBehaviour(scene, component));
        return component;
    }

    public static ScenePhysicsBehaviour AddPhysics(this AppFramework.Scene.Scene scene)
    {
        var behaviour = new ScenePhysicsBehaviour(scene);
        scene.Add(behaviour);
        return behaviour;
    }
}