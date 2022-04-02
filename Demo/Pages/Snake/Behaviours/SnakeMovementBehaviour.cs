using System;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.AppFramework.Plugins;

namespace Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;

public class SnakeMovementBehaviour : Plugin
{
    public static string BehaviourName = "SnakeMovement";
    private readonly Random _random = new();

    private DateTimeOffset _lastTick = DateTimeOffset.Now;

    public SnakeMovementBehaviour(Scene scene) : base(scene)
    {
    }

    public override bool Handle(float deltaT)
    {
        var snakeState = Scene.Plugins.Get<SnakeState>(SnakeState.StateName);

        if (DateTimeOffset.Now - _lastTick < snakeState.TickSpeed)
        {
            return false;
        }

        _lastTick = DateTimeOffset.Now;

        var snakeHead = snakeState.SnakeHead;
        var snakeFood = snakeState.SnakeFood;

        snakeHead.Translate(SnakeGameConfig.GetMovement(snakeHead.Direction));

        var lastDirection = snakeHead.Direction;
        for (var i = 0; i < snakeState.SnakeBody.Count; i++)
        {
            var bodyComponent = snakeState.SnakeBody[i];
            bodyComponent.Translate(SnakeGameConfig.GetMovement(bodyComponent.Direction));

            (lastDirection, bodyComponent.Direction) = (bodyComponent.Direction, lastDirection);
        }

        if (snakeState.SnakeBody.Any(b => b.CollidesWith(snakeHead)))
        {
            snakeState.EndGame();
            return true;
        }

        if (snakeState.Walls.Any(b => b.CollidesWith(snakeHead)))
        {
            snakeState.EndGame();
            return true;
        }

        if (snakeHead.CollidesWith(snakeFood))
        {
            snakeState.Grow();
            var foodPosition = SnakeGameConfig.RandomCell() + new Vector2(2, 2);
            snakeFood.Primitive.SetPosition(foodPosition);
            snakeFood.SetPosition(SnakeGameConfig.RandomCell(), true);
            while (snakeHead.CollidesWith(snakeFood) || snakeState.SnakeBody.Any(b => b.CollidesWith(snakeFood)))
            {
                foodPosition = SnakeGameConfig.RandomCell() + new Vector2(2, 2);
                snakeFood.Primitive.SetPosition(foodPosition);
            }
        }

        return true;
    }

    public override string Name()
    {
        return BehaviourName;
    }
}