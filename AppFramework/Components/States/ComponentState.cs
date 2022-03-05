namespace Aptacode.AppFramework.Components.States;

public class ComponentState
{
    public Component Component { get; init; }

    public ComponentState(Component component)
    {
        Component = component;  
    }
}