using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Behaviours.Transformation;

public abstract class TransformationBehaviour
{
    protected TransformationBehaviour(AppFramework.Scene.Scene scene, Component component)
    {
        Scene = scene;
        Component = component;
    }

    public abstract bool HandleEvent(TransformationEvent transformationEvent);

    #region Properties

    public AppFramework.Scene.Scene Scene { get; init; }
    public Component Component { get; init; }
    public bool Enabled { get; set; } = true;

    #endregion
}