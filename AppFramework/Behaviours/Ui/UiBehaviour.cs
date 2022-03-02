using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Behaviours.Ui;

public abstract class UiBehaviour
{
    protected UiBehaviour(Scene.Scene scene, ComponentViewModel component)
    {
        Scene = scene;
        Component = component;
    }

    public abstract bool HandleEvent(UiEvent uiEvent);

    #region Properties

    public Scene.Scene Scene { get; init; }
    public ComponentViewModel Component { get; init; }
    public bool Enabled { get; set; } = true;

    #endregion
}