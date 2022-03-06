using System.Linq;
using Aptacode.AppFramework.Components.States;

namespace Aptacode.AppFramework.Components.Behaviours.Scene;

public class ScenePhysicsBehaviour : SceneBehavior
{
    public bool Gravity { get; set; } = true;
    public bool Friction { get; set; } = true;
    public ScenePhysicsBehaviour(AppFramework.Scene.Scene scene) : base(scene)
    {
    }

    public override void Handle(float deltaT)
    {
        var physicsState = Scene.Components.Select(c => c.GetState<PhysicsState>()).ToList();

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

            //Calculate Distance
            var distanceMoved = a.Distance(deltaT);

            //Translate component
            a.Component.Translate(distanceMoved);

            //Check collisions with other components
            var hasCollison = false;
            for (var j = i + 1; j < physicsState.Count(); j++)
            {
                var b = physicsState[j];

                //Check if a and b collide
                if (b == null || !a.Component.CollidesWith(b.Component)) 
                    continue;

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
    }
}