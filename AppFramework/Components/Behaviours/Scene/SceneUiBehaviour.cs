using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Behaviours.Scene;

public abstract class SceneUiBehaviour
{
    protected SceneUiBehaviour(AppFramework.Scene.Scene scene)
    {
        Scene = scene;
    }

    protected AppFramework.Scene.Scene Scene { get; }
    public bool Enabled { get; set; } = true;
    public abstract bool Handle(UiEvent uiEvent);
}