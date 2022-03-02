using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Behaviours.Tick;

public abstract class TickBehaviour
{
    protected TickBehaviour(Scene.Scene scene, ComponentViewModel component, string name)
    {
        Scene = scene;
        Component = component;
        Name = name;
    }

    public abstract bool HandleEvent(float time);

    #region Properties

    public Scene.Scene Scene { get; init; }
    public ComponentViewModel Component { get; init; }
    public bool Enabled { get; set; } = true;
    public string Name { get; init; }

    #endregion
}