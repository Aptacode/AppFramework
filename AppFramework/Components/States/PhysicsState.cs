using System.Numerics;
using Aptacode.Geometry;

namespace Aptacode.AppFramework.Components.States;

public class PhysicsState : ComponentState
{
    public PhysicsState(Component component) : base(component)
    {
    }

    #region Constants
    public static readonly Vector2 Gravity = new(0.0f, -0.01f);
    #endregion

    #region Properties
    public float Mass { get; set; } = 0.1f;
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public Vector2 Acceleration { get; set; } = Vector2.Zero;

    public bool IsFixed = false;
    #endregion

    #region Forces

    public PhysicsState ApplyForce(Vector2 force)
    {
        force /= Mass;
        Acceleration += force;
        return this;
    }

    public PhysicsState ApplyGravity()
    {
        Acceleration += Gravity;
        return this;
    }

    public PhysicsState ApplyFriction()
    {
        var absoulte = Vector2.Abs(Velocity);
        if (absoulte.X + absoulte.Y < Constants.Tolerance) return this;

        var c = 0.001f;
        var normal = 1;
        var frictionMag = c * normal;
        Acceleration += Vector2.Normalize(Velocity * -1) * frictionMag;
        return this;
    }

    #endregion

    #region Distance
    public Vector2 Distance(float deltaT)
    {
        if (IsFixed)
            return Vector2.Zero;

        deltaT /= 5; //Slow down time

        Velocity += Acceleration * deltaT;
        Acceleration = Vector2.Zero;

        var distance = Velocity * deltaT;
        return distance;
    }

    #endregion
}