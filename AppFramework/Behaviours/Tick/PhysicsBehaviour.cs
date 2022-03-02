using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Behaviours.Tick;

public abstract class GlobalBehavior
{
    protected GlobalBehavior(Scene.Scene scene)
    {
        Scene = scene;
    }

    protected Scene.Scene Scene { get; }

    public abstract void Handle(float timeSpan);
}

public class GlobalPhysicsBehaviour : GlobalBehavior
{
    public GlobalPhysicsBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public override void Handle(float timeSpan)
    {
        var physicsBehaviours = Scene.Components.Select(c => c.GetTickBehaviour<PhysicsBehaviour>())
            .Where(c => c != null).ToList();

        for (var i = 0; i < physicsBehaviours.Count(); i++)
        {
            var a = physicsBehaviours[i];

            for (var j = i + 1; j < physicsBehaviours.Count(); j++)
            {
                var b = physicsBehaviours[j];

                if (a.Component.CollidesWith(b.Component))
                {
                    var m1 = a.Mass;
                    var u1 = a.Velocity;

                    var m2 = b.Mass;
                    var u2 = b.Velocity;

                    a.Velocity = (u1 * (m1 - m2) + 2 * m2 * u2) / (m1 + m2);
                    b.Velocity = (u2 * (m2 - m1) + 2 * m1 * u1) / (m1 + m2);
                }
            }
        }
    }
}

public class PhysicsBehaviour : TickBehaviour
{
    private static readonly PolyLine top = PolyLine.Create(0, 0, 100, 0);
    private static readonly PolyLine right = PolyLine.Create(100, 0, 100, 100);
    private static readonly PolyLine bottom = PolyLine.Create(100, 100, 0, 100);
    private static readonly PolyLine left = PolyLine.Create(0, 100, 0, 0);

    private float lastTimeStamp = -1;
    private readonly float resistance = 0.99f;

    public PhysicsBehaviour(Scene.Scene scene, ComponentViewModel component) : base(scene, component,
        nameof(PhysicsBehaviour))
    {
    }

    public float Mass { get; set; } = 0.1f;
    public Vector2 Velocity { get; set; } = new(0, 0);
    public Vector2 Acceleration { get; set; } = new(0.0f, 0.001f);

    public override bool HandleEvent(float timeStamp)
    {
        if (lastTimeStamp < 0)
        {
            lastTimeStamp = timeStamp;
            return true;
        }

        var deltaT = (timeStamp - lastTimeStamp) / 5;
        lastTimeStamp = timeStamp;

        Velocity += Acceleration * deltaT;
        Velocity *= resistance;

        if (Component.CollidesWith(top) || Component.CollidesWith(bottom)) Velocity *= new Vector2(1, -1);
        if (Component.CollidesWith(left) || Component.CollidesWith(right)) Velocity *= new Vector2(-1, 1);

        var distance = Velocity * deltaT;
        Component.Translate(distance, true);

        return true;
    }
}