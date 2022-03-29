using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Plugins.Behaviours;

public abstract class ComponentBehaviour<T> : BehaviourPlugin<T>
{
    protected ComponentBehaviour(Scene.Scene scene, Component component) : base(scene)
    {
        Scene = scene;
        Component = component;
    }

    #region Properties

    public Component Component { get; init; }

    #endregion
}