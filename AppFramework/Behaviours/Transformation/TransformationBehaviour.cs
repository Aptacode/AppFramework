using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Behaviours.Transformation;

public abstract class TransformationBehaviour
{
    protected TransformationBehaviour(Scene.Scene scene, Component component)
    {
        Scene = scene;
        Component = component;
    }

    public abstract bool HandleEvent(TransformationEvent transformationEvent);

    #region Properties

    public Scene.Scene Scene { get; init; }
    public Component Component { get; init; }
    public bool Enabled { get; set; } = true;

    #endregion
}