using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Plugins.States;

public abstract class ComponentState : Plugin
{
    protected ComponentState(Component component)
    {
        Component = component;
    }

    public Component Component { get; init; }
}