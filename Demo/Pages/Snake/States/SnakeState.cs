using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Demo.Pages.Snake.Components;
using Aptacode.AppFramework.Plugins.States;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Demo.Pages.Snake.States;

public sealed class SnakeState : SceneState
{
    public static string StateName = "SnakeState";

    public SnakeState(Scene.Scene scene) : base(scene)
    {
    }

    public SnakeBodyComponent SnakeHead { get; set; }
    public SnakeFoodComponent SnakeFood { get; set; }
    public List<SnakeBodyComponent> SnakeBody { get; set; } = new();

    public void Grow()
    {
        var lastSnakeComponent = SnakeBody.Count > 0 ? SnakeBody.Last() : SnakeHead;

        var snakeBodyComponent =
            new SnakeBodyComponent(Polygon.Create(lastSnakeComponent.Polygon.Vertices.Vertices.ToArray()));
        snakeBodyComponent.FillColor = Color.LightSlateGray;
        snakeBodyComponent.BorderColor = Color.DarkSlateGray;
        switch (snakeBodyComponent.Direction)
        {
            case Direction.Up:
                snakeBodyComponent.Translate(new Vector2(0, -50));
                break;
            case Direction.Down:
                snakeBodyComponent.Translate(new Vector2(0, 50));
                break;
            case Direction.Left:
                snakeBodyComponent.Translate(new Vector2(50, 0));
                break;
            case Direction.Right:
                snakeBodyComponent.Translate(new Vector2(-50, 0));
                break;
        }

        SnakeBody.Add(snakeBodyComponent);
        Scene.Add(snakeBodyComponent);
    }

    public override string Name()
    {
        return StateName;
    }
}