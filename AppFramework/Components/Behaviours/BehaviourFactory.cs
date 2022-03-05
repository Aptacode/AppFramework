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

    public static AppFramework.Scene.Scene AddPhysics(this AppFramework.Scene.Scene scene)
    {
        scene.Add(new ScenePhysicsBehaviour(scene));
        return scene;
    }
}