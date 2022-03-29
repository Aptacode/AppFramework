namespace Aptacode.AppFramework.Components.States.Component;

public class ComponentState
{
    public ComponentState(Components.Component component)
    {
        Component = component;
    }

    public Components.Component Component { get; init; }
}