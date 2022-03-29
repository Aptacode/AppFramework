namespace Aptacode.AppFramework.Components.States.Component;

public static class StateFactory
{
    public static PhysicsStateBuilder AddPhysics(this Components.Component component)
    {
        var state = new PhysicsState(component);
        component.AddState(state);
        return new PhysicsStateBuilder(state);
    }
}