using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Physics.States;

public static class StateFactory
{
    public static PhysicsStateBuilder AddPhysics(this PrimitiveComponent component, Scene scene)
    {
        var state = new PhysicsState(scene, component);
        component.AddPlugin(state);
        return new PhysicsStateBuilder(state);
    }
}