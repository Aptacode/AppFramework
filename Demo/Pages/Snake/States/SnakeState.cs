using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Demo.Pages.Snake.Components;
using Aptacode.AppFramework.Plugins;

namespace Aptacode.AppFramework.Demo.Pages.Snake.States;

public sealed class SnakeState : Plugin
{
    public static string StateName = "SnakeState";
    private static readonly TimeSpan InitialTickSpeed = TimeSpan.FromMilliseconds(100);

    public SnakeState(Scene scene) : base(scene)
    {
    }

    public SnakeBodyComponent SnakeHead { get; set; }
    public SnakeFoodComponent SnakeFood { get; set; }
    public List<SnakeBodyComponent> SnakeBody { get; set; } = new();

    public List<PrimitiveComponent> Walls { get; set; } = new();

    public bool Running { get; set; } = true;

    public EventHandler<int> GameOver { get; set; }
    public EventHandler<int> ScoreChanged { get; set; }
    public TimeSpan TickSpeed { get; set; } = InitialTickSpeed;

    public int Score => SnakeBody.Count;

    public void EndGame()
    {
        Running = false;
        var score = SnakeBody.Count;
        Reset();
        GameOver?.Invoke(this, score);
    }

    public void Reset()
    {
        TickSpeed = InitialTickSpeed;
        SnakeHead.SetTranslation(SnakeGameConfig.RandomCell());
        foreach (var snakeBodyComponent in SnakeBody)
        {
            Scene.Remove(snakeBodyComponent);
        }

        SnakeBody.Clear();
        ScoreChanged?.Invoke(this, SnakeBody.Count);

        SnakeFood.SetTranslation(SnakeGameConfig.RandomCell());
        var foodPos = SnakeFood.Polygon.Copy().Transform(SnakeFood.TranslationMatrix);

        while (SnakeHead.CollidesWith(foodPos) || SnakeBody.Any(b => b.CollidesWith(foodPos)))
        {
            SnakeFood.TranslationMatrix = Matrix3x2.CreateTranslation(SnakeGameConfig.RandomCell());
            foodPos = SnakeFood.Polygon.Copy().Transform(SnakeFood.TranslationMatrix);
        }
    }

    public void Grow()
    {
        ScoreChanged?.Invoke(this, SnakeBody.Count);

        TickSpeed = TimeSpan.FromMilliseconds(Math.Clamp(InitialTickSpeed.Milliseconds - 20 * SnakeBody.Count, 10,
            InitialTickSpeed.Milliseconds));

        var lastSnakeComponent = SnakeBody.Count > 0 ? SnakeBody.Last() : SnakeHead;

        var snakeBodyComponent = new SnakeBodyComponent(lastSnakeComponent.Polygon.Copy());

        var cell = SnakeGameConfig.GetMovement(SnakeGameConfig.Reverse(snakeBodyComponent.Direction));
        snakeBodyComponent.TranslationMatrix = lastSnakeComponent.TranslationMatrix * Matrix3x2.CreateTranslation(cell);

        SnakeBody.Add(snakeBodyComponent);
        Scene.Add(snakeBodyComponent);
    }

    public override string Name()
    {
        return StateName;
    }
}