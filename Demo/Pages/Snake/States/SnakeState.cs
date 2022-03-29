using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Demo.Pages.Snake.Components;
using Aptacode.AppFramework.Plugins.States;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Snake.States;

public sealed class SnakeState : SceneState
{
    public static string StateName = "SnakeState";
    private static readonly TimeSpan InitialTickSpeed = TimeSpan.FromMilliseconds(250);

    public SnakeState(Scene.Scene scene) : base(scene)
    {
    }

    public SnakeBodyComponent SnakeHead { get; set; }
    public SnakeFoodComponent SnakeFood { get; set; }
    public List<SnakeBodyComponent> SnakeBody { get; set; } = new();

    public List<Component> Walls { get; set; } = new();

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
        SnakeHead.SetPosition(SnakeGameConfig.RandomCell(), true);
        foreach (var snakeBodyComponent in SnakeBody)
        {
            Scene.Remove(snakeBodyComponent);
        }

        SnakeBody.Clear();
        ScoreChanged?.Invoke(this, SnakeBody.Count);

        SnakeFood.SetPosition(SnakeGameConfig.RandomCell(), true);
        while (SnakeHead.CollidesWith(SnakeFood))
        {
            SnakeFood.SetPosition(SnakeGameConfig.RandomCell(), true);
        }
    }

    public void Grow()
    {
        ScoreChanged?.Invoke(this, SnakeBody.Count);

        TickSpeed = TimeSpan.FromMilliseconds(Math.Clamp(InitialTickSpeed.Milliseconds - 20 * SnakeBody.Count, 10,
            InitialTickSpeed.Milliseconds));

        var lastSnakeComponent = SnakeBody.Count > 0 ? SnakeBody.Last() : SnakeHead;

        var snakeBodyComponent =
            new SnakeBodyComponent(Polygon.Create(lastSnakeComponent.Polygon.Vertices.Vertices.ToArray()));
        snakeBodyComponent.FillColor = Color.LightSlateGray;
        snakeBodyComponent.BorderColor = Color.DarkSlateGray;
        snakeBodyComponent.Translate(
            SnakeGameConfig.GetMovement(SnakeGameConfig.Reverse(snakeBodyComponent.Direction)));

        SnakeBody.Add(snakeBodyComponent);
        Scene.Add(snakeBodyComponent);
    }

    public override string Name()
    {
        return StateName;
    }
}