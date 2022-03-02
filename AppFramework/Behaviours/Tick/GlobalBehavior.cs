namespace Aptacode.AppFramework.Behaviours.Tick;

public abstract class GlobalBehavior
{
    protected GlobalBehavior(Scene.Scene scene)
    {
        Scene = scene;
    }

    protected Scene.Scene Scene { get; }

    public abstract void Handle(float deltaT);
}