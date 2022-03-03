using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Behaviours.Tick;

public class PhysicsBehaviour : TickBehaviour
{
    private static readonly PolyLine top = PolyLine.Create(0, 0, 100, 0);
    private static readonly PolyLine right = PolyLine.Create(100, 0, 100, 100);
    private static readonly PolyLine bottom = PolyLine.Create(100, 100, 0, 100);
    private static readonly PolyLine left = PolyLine.Create(0, 100, 0, 0);

    private readonly float resistance = 0.99f;

    public PhysicsBehaviour(Scene.Scene scene, ComponentViewModel component) : base(scene, component)
    {
    }

    public float Mass { get; set; } = 0.1f;
    public Vector2 Velocity { get; set; } = new(0, 0);
    public Vector2 Acceleration { get; set; } = new(0.0f, 0.001f);

    public override bool HandleEvent(float deltaT)
    {
        deltaT /= 5;//Slow down time

        Velocity += Acceleration * deltaT;
        Velocity *= resistance;

        if (Component.CollidesWith(top) || Component.CollidesWith(bottom)) Velocity *= new Vector2(1, -1);
        if (Component.CollidesWith(left) || Component.CollidesWith(right)) Velocity *= new Vector2(-1, 1);

        var distance = Velocity * deltaT;
        Component.Translate(distance);

        return true;
    }
}