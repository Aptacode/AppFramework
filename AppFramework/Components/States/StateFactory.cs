namespace Aptacode.AppFramework.Components.States;

public static class StateFactory
{
    public static PhysicsStateBuilder AddPhysics(this Component component)
    {
        var state = new PhysicsState(component);
        component.AddState(state);
        return new PhysicsStateBuilder(state);
    }
}