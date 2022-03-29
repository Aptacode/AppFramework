using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Demo.Pages.Physics.States;

public static class StateFactory
{
    public static PhysicsStateBuilder AddPhysics(this Component component)
    {
        var state = new PhysicsState(component);
        component.Plugins.State.Add(state);
        return new PhysicsStateBuilder(state);
    }
}