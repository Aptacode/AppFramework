namespace Aptacode.AppFramework.Components.States.Scene;

public class SceneState
{
    public SceneState(AppFramework.Scene.Scene scene)
    {
        Scene = scene;
    }

    public AppFramework.Scene.Scene Scene { get; init; }
}