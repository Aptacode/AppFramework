using System;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Behaviours.Tick;

public class PhysicsBehaviour : TickBehaviour
{
    private static readonly PolyLine top = PolyLine.Create(10, 10, 90, 10);
    private static readonly PolyLine right = PolyLine.Create(90, 10, 90, 90);
    private static readonly PolyLine bottom = PolyLine.Create(90, 90, 10, 90);
    private static readonly PolyLine left = PolyLine.Create(10, 90, 10, 10);
    public static readonly Vector2 Gravity = new(0.0f, 0.01f);

    public PhysicsBehaviour(Scene.Scene scene, Component component) : base(scene, component)
    {
        var r = Random.Shared;
        Velocity = new Vector2(r.Next(-1, 1), r.Next(-1, 1)) * 0.5f;
    }

    public float Mass { get; set; } = 0.1f;
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public Vector2 Acceleration { get; set; } = Vector2.Zero;

    public float AngularMomentum { get; set; } = 0.01f;

    public PhysicsBehaviour ApplyForce(Vector2 force)
    {
        force /= Mass;
        Acceleration += force;
        return this;
    }

    public PhysicsBehaviour ApplyGravity()
    {
        Acceleration += Gravity;
        return this;
    }

    public PhysicsBehaviour ApplyFriction()
    {
        var absoulte = Vector2.Abs(Velocity);
        if (absoulte.X + absoulte.Y < Constants.Tolerance)
        {
            return this;
        }

        var c = 0.001f;
        var normal = 1;
        var frictionMag = c * normal;
        Acceleration += Vector2.Normalize(Velocity * -1) * frictionMag;
        return this;
    }

    public override bool HandleEvent(float deltaT)
    {
        deltaT /= 5;//Slow down time

        Velocity += Acceleration * deltaT;
        Acceleration = Vector2.Zero;

        var distance = Velocity * deltaT;

            Component.Translate(distance);
            var collidesWithTop = Component.CollidesWith(top);
            var collidesWithBottom = Component.CollidesWith(bottom);
            var collidesWithLeft = Component.CollidesWith(left);
            var collidesWithRight = Component.CollidesWith(right);

            if (collidesWithTop || collidesWithBottom)
            {
                Velocity = new Vector2(Velocity.X, -Velocity.Y);
                Component.Translate(new Vector2(0, -distance.Y));
            }

            if (collidesWithLeft || collidesWithRight)
            {
                Velocity *= new Vector2(-1, 1);
                Component.Translate(new Vector2(-distance.X, 0));
            }
        

        //if (AngularMomentum > Constants.Tolerance)
        //{
        //    Component.Rotate(AngularMomentum);
        //}

        return true;
    }
}