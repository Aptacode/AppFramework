using Aptacode.AppFramework.Events;

namespace Aptacode.AppFramework.Plugins;

public abstract class Plugin
{
    protected Plugin(Scene scene)
    {
        Scene = scene;
    }

    public bool Enabled { get; set; } = true;

    public Scene Scene { get; init; }
    public abstract string Name();

    public virtual bool Handle(float deltaT)
    {
        return false;
    }

    public virtual bool Handle(UiEvent uiEvent)
    {
        return false;
    }

    public virtual bool Handle(ComponentEvent uiEvent)
    {
        return false;
    }
}