using System.Linq;
using Aptacode.AppFramework.Demo.Pages.Physics.States;
using Aptacode.AppFramework.Plugins.Behaviours;

namespace Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;

public class ScenePhysicsBehaviour : BehaviourPlugin<float>
{
    public static string BehaviourName = "ScenePhysics";

    public ScenePhysicsBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public bool Gravity { get; set; } = true;
    public bool Friction { get; set; } = true;

    public override bool Handle(float deltaT)
    {
        var physicsState = Scene.Components.Select(c => c.Plugins.State.Get<PhysicsState>(PhysicsState.StateName))
            .ToList();

        for (var i = 0; i < physicsState.Count(); i++)
        {
            var a = physicsState[i];
            if (a == null || a.IsFixed)
            {
                continue;
            }

            //Apply forces
            if (Gravity)
            {
                a.ApplyGravity();
            }

            if (Friction)
            {
                a.ApplyFriction();
            }

            a.ApplyDamping();

            //Calculate Distance
            var distanceMoved = a.CalculateDistance(deltaT);
            var rotation = a.CalculateRotation(deltaT);

            a.Component.Rotate(rotation);
            //Translate component
            a.Component.Translate(distanceMoved);

            //Check collisions with other components
            var hasCollison = false;
            for (var j = i + 1; j < physicsState.Count(); j++)
            {
                var b = physicsState[j];

                //Check if a and b collide
                if (b == null || !a.Component.CollidesWith(b.Component))
                {
                    continue;
                }

                hasCollison = true;

                //Calculate resultant velocity
                var m1 = a.Mass;
                var u1 = a.Velocity;

                var m2 = b.Mass;
                var u2 = b.Velocity;

                a.Velocity = (u1 * (m1 - m2) + 2 * m2 * u2) / (m1 + m2);
                b.Velocity = (u2 * (m2 - m1) + 2 * m1 * u1) / (m1 + m2);
            }

            //If the component had a collision undo the translation
            if (hasCollison)
            {
                a.Component.Translate(-distanceMoved);
            }
        }

        return true;
    }

    public override string Name()
    {
        return BehaviourName;
    }
}