using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Behaviours.Transformation;

public abstract class TransformationBehaviour
{
    protected TransformationBehaviour(Scene.Scene scene, ComponentViewModel component, string name)
    {
        Scene = scene;
        Component = component;
        Name = name;
    }

    public abstract bool HandleEvent(TransformationEvent transformationEvent);

    #region Properties

    public Scene.Scene Scene { get; init; }
    public ComponentViewModel Component { get; init; }
    public bool Enabled { get; set; } = true;
    public string Name { get; init; }

    #endregion
}