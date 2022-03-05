namespace Aptacode.AppFramework.Components.Behaviours.Scene;

public abstract class SceneBehavior
{
    protected SceneBehavior(AppFramework.Scene.Scene scene)
    {
        Scene = scene;
    }

    protected AppFramework.Scene.Scene Scene { get; }

    public abstract void Handle(float deltaT);
}