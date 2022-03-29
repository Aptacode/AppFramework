namespace Aptacode.AppFramework.Plugins.Behaviours;

public abstract class BehaviourPlugin<T> : Plugin
{
    protected BehaviourPlugin(Scene.Scene scene)
    {
        Scene = scene;
    }

    public Scene.Scene Scene { get; init; }

    public abstract bool Handle(T input);
}