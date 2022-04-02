using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Events;

namespace Aptacode.AppFramework.Plugins.Behaviours;

public abstract class ComponentPlugin : Plugin
{
    protected ComponentPlugin(Scene scene, Component component) : base(scene)
    {
        Scene = scene;
        Component = component;
    }

    #region Properties

    public Component Component { get; init; }

    #endregion

    public virtual bool Handle(TransformationEvent transformationEvent)
    {
        return false;
    }
}