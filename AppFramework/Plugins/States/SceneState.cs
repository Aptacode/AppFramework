namespace Aptacode.AppFramework.Plugins.States;

public abstract class SceneState : Plugin
{
    protected SceneState(Scene.Scene scene)
    {
        Scene = scene;
    }

    public Scene.Scene Scene { get; init; }
}