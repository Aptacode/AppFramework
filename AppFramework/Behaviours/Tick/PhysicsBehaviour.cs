using System.Numerics;
using Aptacode.AppFramework.Components;

namespace Aptacode.AppFramework.Behaviours.Tick;

public class PhysicsBehaviour : TickBehaviour
{
    public Vector2 Velocity { get; set; } = new(0.01f, 0.01f);
    public Vector2 Acceleration { get; set; } = new(0.001f, 0.001f);
    public PhysicsBehaviour(Scene.Scene scene, ComponentViewModel component) : base(scene, component, nameof(PhysicsBehaviour))
    {

    }

    public override bool HandleEvent(float time)
    {
        Velocity += Acceleration;
        Component.Translate(Velocity, true);
        return true;
    }
}