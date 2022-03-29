using System;
using System.Numerics;
using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.AppFramework.Plugins.Behaviours;

namespace Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;

public class SnakeMovementBehaviour : BehaviourPlugin<float>
{
    public static string BehaviourName = "SnakeMovement";
    private readonly Random _random = new();
    private readonly TimeSpan _tickSpeed = TimeSpan.FromMilliseconds(250);

    private DateTimeOffset _lastTick = DateTimeOffset.Now;

    public SnakeMovementBehaviour(Scene.Scene scene) : base(scene)
    {
    }

    public override bool Handle(float deltaT)
    {
        if (DateTimeOffset.Now - _lastTick < _tickSpeed)
        {
            return false;
        }

        _lastTick = DateTimeOffset.Now;

        var snakeState = Scene.Plugins.State.Get<SnakeState>(SnakeState.StateName);
        var snakeHead = snakeState.SnakeHead;
        var snakeFood = snakeState.SnakeFood;

        switch (snakeHead.Direction)
        {
            case Direction.Up:
                snakeHead.Translate(new Vector2(0, 50));
                break;
            case Direction.Down:
                snakeHead.Translate(new Vector2(0, -50));
                break;
            case Direction.Left:
                snakeHead.Translate(new Vector2(-50, 0));
                break;
            case Direction.Right:
                snakeHead.Translate(new Vector2(50, 0));
                break;
        }

        var lastDirection = snakeHead.Direction;
        for (var i = 0; i < snakeState.SnakeBody.Count; i++)
        {
            var bodyComponent = snakeState.SnakeBody[i];

            switch (bodyComponent.Direction)
            {
                case Direction.Up:
                    bodyComponent.Translate(new Vector2(0, 50));
                    break;
                case Direction.Down:
                    bodyComponent.Translate(new Vector2(0, -50));
                    break;
                case Direction.Left:
                    bodyComponent.Translate(new Vector2(-50, 0));
                    break;
                case Direction.Right:
                    bodyComponent.Translate(new Vector2(50, 0));
                    break;
            }

            (lastDirection, bodyComponent.Direction) = (bodyComponent.Direction, lastDirection);
        }

        if (snakeHead.CollidesWith(snakeFood))
        {
            snakeState.Grow();
            snakeFood.SetPosition(new Vector2(_random.Next(0, 10) * 50, _random.Next(0, 10) * 50), true);
        }

        return true;
    }

    public override string Name()
    {
        return BehaviourName;
    }
}