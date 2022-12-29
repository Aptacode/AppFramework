using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Events;

namespace Aptacode.AppFramework.Plugins.Behaviours;

public abstract class PrimitiveComponentPlugin : Plugin
{
    protected PrimitiveComponentPlugin(Scene scene, PrimitiveComponent component) : base(scene)
    {
        Scene = scene;
        Component = component;
    }

    #region Properties

    public PrimitiveComponent Component { get; init; }

    #endregion

    public virtual bool Handle(TransformationEvent transformationEvent)
    {
        return false;
    }
}