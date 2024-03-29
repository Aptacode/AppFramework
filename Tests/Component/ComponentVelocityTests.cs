﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;
using Aptacode.AppFramework.Demo.Pages.Physics.States;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;
using Xunit;

namespace Tests.Component;

public class ComponentVelocityTests
{
    private readonly CircleComponent _component;
    private readonly PhysicsState _componentPhysics;
    private readonly Aptacode.AppFramework.Scene _scene;
    private readonly ScenePhysicsBehaviour _scenePhysics;

    public ComponentVelocityTests()
    {
        //_scene = new Aptacode.AppFramework.Scene
        //{
        //    Size = new Vector2(100)
        //};
        _scenePhysics = _scene.AddPhysics();
        _scenePhysics.Gravity = false;
        _scenePhysics.Friction = false;

        _component = new CircleComponent(new Vector2(50, 50), 10);
        var physicsBuilder = _component.AddPhysics(_scene);
        _componentPhysics = physicsBuilder.State;

        _scene.Add(_component);
    }

    public static IEnumerable<object[]> GetForces()
    {
        yield return new object[] { new Vector2(0, 0) };
        yield return new object[] { new Vector2(0, 1) };
        yield return new object[] { new Vector2(1, 0) };
        yield return new object[] { new Vector2(1, 1) };

        yield return new object[] { new Vector2(0, -1) };
        yield return new object[] { new Vector2(-1, 0) };
        yield return new object[] { new Vector2(-1, -1) };

        yield return new object[] { new Vector2(1, -1) };
        yield return new object[] { new Vector2(-1, 1) };
    }

    [Theory]
    [MemberData(nameof(GetForces))]
    public void ComponentMovesUnderForce(Vector2 force)
    {
        //Arrange
        var start = _component.Circle.Position;

        //Act
        _componentPhysics.ApplyForce(force);
        _scene.Loop(10);

        //Assert

        //If force has an X component
        if (Math.Abs(force.X) > Constants.Tolerance)
        {
            if (force.X > 0)
            {
                //Assert c moved right
                Assert.True(_component.Circle.Position.X > start.X);
            }
            else
            {
                //Assert c moved left
                Assert.True(_component.Circle.Position.X < start.X);
            }
        }
        else
        {
            //Assert c didnt move horizontally
            Assert.True(_component.Circle.Position.X - start.X < Constants.Tolerance);
        }

        //If force has an Y component
        if (Math.Abs(force.Y) > Constants.Tolerance)
        {
            if (force.Y > 0)
            {
                //Assert c moved up
                Assert.True(_component.Circle.Position.Y > start.Y);
            }
            else
            {
                //Assert c moved down
                Assert.True(_component.Circle.Position.Y < start.Y);
            }
        }
        else
        {
            //Assert c didnt move vertically
            Assert.True(_component.Circle.Position.Y - start.Y < Constants.Tolerance);
        }
    }
}