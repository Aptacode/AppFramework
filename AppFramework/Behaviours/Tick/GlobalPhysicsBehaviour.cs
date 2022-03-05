using System.Linq;

namespace Aptacode.AppFramework.Behaviours.Tick;

public class GlobalPhysicsBehaviour : GlobalBehavior
{
    public GlobalPhysicsBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public override void Handle(float deltaT)
    {
        var physicsBehaviours = Scene.Components.Select(c => c.GetTickBehaviour<PhysicsBehaviour>())
            .Where(c => c != null).ToList();

        for (var i = 0; i < physicsBehaviours.Count(); i++)
        {
            var a = physicsBehaviours[i];

            a.ApplyGravity().ApplyFriction();

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