using System;
using System.Numerics;

namespace Aptacode.AppFramework.Demo.Pages.Physics.States;

public class PhysicsStateBuilder
{
    private readonly Random r = Random.Shared;

    public PhysicsStateBuilder(PhysicsState state)
    {
        State = state;
    }

    public PhysicsState State { get; }

    public PhysicsStateBuilder SetVelocity(float x, float y)
    {
        State.Velocity = new Vector2(x, y);
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(float x)
    {
        State.Velocity = State.Velocity with { X = x };
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(int minX, int maxX)
    {
        State.Velocity = State.Velocity with { X = r.Next(minX * 10, maxX * 10) / 10.0f };
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(float y)
    {
        State.Velocity = State.Velocity with { Y = y };
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(int minY, int maxY)
    {
        State.Velocity = State.Velocity with { Y = r.Next(minY * 10, maxY * 10) / 10.0f };
        return this;
    }

    public PhysicsStateBuilder IsFixed()
    {
        State.IsFixed = true;
        State.Mass = 1000;
        return this;
    }
}