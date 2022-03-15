using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Behaviours.Ui;

public abstract class UiBehaviour
{
    protected UiBehaviour(AppFramework.Scene.Scene scene, Component component)
    {
        Scene = scene;
        Component = component;
    }

    public abstract bool HandleEvent(UiEvent uiEvent);

    #region Properties

    public AppFramework.Scene.Scene Scene { get; init; }
    public Component Component { get; init; }
    public bool Enabled { get; set; } = true;

    #endregion
}