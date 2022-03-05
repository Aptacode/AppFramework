using System;
using System.Numerics;

namespace Aptacode.AppFramework.Components.States;

public class PhysicsStateBuilder
{
    private readonly PhysicsState _state;
    private readonly Random r = Random.Shared;
    public PhysicsStateBuilder(PhysicsState state)
    {
        this._state = state;
    }

    public PhysicsStateBuilder SetVelocity(float x, float y)
    {
        _state.Velocity = new Vector2(x, y);
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(float x)
    {
        _state.Velocity = new Vector2(x, _state.Velocity.Y);
        return this;
    }

    public PhysicsStateBuilder SetHorizontalVelocity(int minX, int maxX)
    {
        _state.Velocity = new Vector2(r.Next(minX * 10, maxX * 10)/10.0f, _state.Velocity.Y);
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(float y)
    {
        _state.Velocity = new Vector2(_state.Velocity.X, y);
        return this;
    }

    public PhysicsStateBuilder SetVerticalVelocity(int minY, int maxY)
    {
        _state.Velocity = new Vector2(_state.Velocity.X, r.Next(minY * 10, maxY * 10) / 10.0f);
        return this;
    }

    public PhysicsStateBuilder IsFixed()
    {
        _state.IsFixed = true;
        _state.Mass = 1000;
        return this;
    }
}