using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Demo.Pages.Physics.States;

public static class StateFactory
{
    public static PhysicsStateBuilder AddPhysics(this Component component, Scene scene)
    {
        var state = new PhysicsState(scene, component);
        component.Plugins.Add(state);
        return new PhysicsStateBuilder(state);
    }
}