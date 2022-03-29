using System.Numerics;
using Aptacode.Geometry;

namespace Aptacode.AppFramework.Components.States.Component;

public class PhysicsState : ComponentState
{
    #region Constants

    public static readonly Vector2 Gravity = new(0.0f, -0.01f);

    #endregion

    public PhysicsState(Components.Component component) : base(component)
    {
    }

    #region Distance

    public Vector2 CalculateDistance(float deltaT)
    {
        if (IsFixed)
        {
            return Vector2.Zero;
        }

        deltaT /= 2; //Slow down time

        Velocity += Acceleration * deltaT;
        Acceleration = Vector2.Zero;

        var distance = Velocity * deltaT;
        return distance;
    }

    #endregion

    #region Linear Momentum

    public float Mass { get; set; } = 0.1f;
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public Vector2 Acceleration { get; set; } = Vector2.Zero;

    public bool IsFixed = false;

    #endregion

    #region Angular Momentum

    public float AngularAcceleration { get; set; }
    public float AngularVelocity { get; set; }
    public float Angle { get; set; }
    public float MomentOfInertia { get; set; } = 0.01f;

    #endregion

    #region Angular Forces

    public PhysicsState ApplyForce(float torque)
    {
        AngularAcceleration += torque / MomentOfInertia;
        return this;
    }

    public float CalculateRotation(float deltaT)
    {
        if (IsFixed)
        {
            return 0.0f;
        }

        AngularVelocity += AngularAcceleration * deltaT;
        AngularAcceleration = 0.0f;

        return AngularVelocity * deltaT;
    }

    public PhysicsState ApplyDamping()
    {
        //If there is no movement there is no friction
        if (AngularVelocity < Constants.Tolerance)
        {
            return this;
        }

        var c = 0.001f;
        var normal = 1;
        var frictionMag = c * normal;
        AngularAcceleration += -AngularVelocity * frictionMag;
        return this;
    }

    #endregion

    #region Forces

    public PhysicsState ApplyForce(Vector2 force)
    {
        Acceleration += force / Mass;
        return this;
    }

    public PhysicsState ApplyGravity()
    {
        Acceleration += Gravity;
        return this;
    }

    public PhysicsState ApplyFriction()
    {
        var absolute = Vector2.Abs(Velocity);

        //If there is no movement there is no friction
        if (absolute.X + absolute.Y < Constants.Tolerance)
        {
            return this;
        }

        var c = 0.001f;
        var normal = 1;
        var frictionMag = c * normal;
        Acceleration += Vector2.Normalize(Velocity * -1) * frictionMag;
        return this;
    }

    #endregion
}