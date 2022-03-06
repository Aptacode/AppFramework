using System;
using System.Numerics;

namespace Aptacode.AppFramework.Components.States;

public class PhysicsStateBuilder
{
    public PhysicsState State { get; }
    private readonly Random r = Random.Shared;
    public PhysicsStateBuilder(PhysicsState state)
    {
        this.State = state;
    }

    public PhysicsStateBuilder SetVelocity(float x, float y)
    {
        State.Velocity = new Vector2(x, y);
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(float x)
    {
        State.Velocity = new Vector2(x, State.Velocity.Y);
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(int minX, int maxX)
    {
        State.Velocity = new Vector2(r.Next(minX * 10, maxX * 10)/10.0f, State.Velocity.Y);
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(float y)
    {
        State.Velocity = new Vector2(State.Velocity.X, y);
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(int minY, int maxY)
    {
        State.Velocity = new Vector2(State.Velocity.X, r.Next(minY * 10, maxY * 10) / 10.0f);
        return this;
    }

    public PhysicsStateBuilder IsFixed()
    {
        State.IsFixed = true;
        State.Mass = 1000;
        return this;
    }
}