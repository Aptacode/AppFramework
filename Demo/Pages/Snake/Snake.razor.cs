using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Demo.Pages.Snake.Behaviours;
using Aptacode.AppFramework.Demo.Pages.Snake.Components;
using Aptacode.AppFramework.Demo.Pages.Snake.States;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages.Snake;

public static class SnakeGameConfig
{
    public static readonly Random _rand = new();
    public static readonly Vector2 BoardSize = new(500, 500);
    public static readonly Vector2 CellSize = new(25, 25);

    public static readonly Vector2 CenterCell = new(CellSize.X * HorizontalCells / 2, CellSize.Y * VerticalCells / 2);

    public static readonly int HorizontalCells = (int)(BoardSize / CellSize).X;
    public static readonly int VerticalCells = (int)(BoardSize / CellSize).X;

    public static Vector2 RandomCell()
    {
        return new Vector2(_rand.Next(1, HorizontalCells - 1) * CellSize.X,
            _rand.Next(1, VerticalCells - 1) * CellSize.Y);
    }

    public static Vector2 GetMovement(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Vector2(0, CellSize.Y),
            Direction.Down => new Vector2(0, -CellSize.Y),
            Direction.Left => new Vector2(-CellSize.X, 0),
            Direction.Right => new Vector2(CellSize.X, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction Reverse(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Left,
            Direction.Right => Direction.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}

public class SnakeBase : ComponentBase
{
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }
    public Scene.Scene Scene { get; set; }
    public SceneController SceneController { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //Scene
        Scene = new SceneBuilder().SetWidth(SnakeGameConfig.BoardSize.X)
            .SetHeight(SnakeGameConfig.BoardSize.Y).Build();

        SceneController = new SceneController(BlazorCanvas, Scene)
        {
            ShowGrid = true
        };

        var snakeHead =
            new SnakeBodyComponent(Polygon.Rectangle.FromTwoPoints(SnakeGameConfig.CenterCell + new Vector2(2, 2),
                SnakeGameConfig.CenterCell + SnakeGameConfig.CellSize - new Vector2(4, 4)));
        snakeHead.FillColor = Color.LightSlateGray;
        snakeHead.BorderColor = Color.DarkSlateGray;
        snakeHead.Direction = Direction.Up;
        Scene.Add(snakeHead);

        var foodPosition = SnakeGameConfig.RandomCell();
        var snakeFood =
            new SnakeFoodComponent(Polygon.Rectangle.FromTwoPoints(foodPosition + new Vector2(2, 2),
                foodPosition + SnakeGameConfig.CellSize - new Vector2(4, 4)));
        snakeFood.FillColor = Color.Red;
        snakeFood.BorderColor = Color.DarkSlateGray;
        Scene.Add(snakeFood);

        var snakeDirection = new SnakeControlBehaviour(Scene);
        Scene.Plugins.Ui.Add(snakeDirection);

        var snakeState = new SnakeState(Scene);
        snakeState.SnakeHead = snakeHead;
        snakeState.SnakeFood = snakeFood;
        Scene.Plugins.State.Add(snakeState);

        var snakeBehavioru = new SnakeMovementBehaviour(Scene);
        Scene.Plugins.Tick.Add(snakeBehavioru);

        var thickness = 10.0f;
        var bottom = Polygon.Create(thickness, thickness, SnakeGameConfig.BoardSize.X - thickness, thickness,
            SnakeGameConfig.BoardSize.X, 0, 0, 0).ToComponent();
        bottom.FillColor = Color.SlateGray;
        bottom.BorderColor = Color.SlateGray;

        var right = Polygon.Create(SnakeGameConfig.BoardSize.X - thickness, thickness,
            SnakeGameConfig.BoardSize.X - thickness, SnakeGameConfig.BoardSize.Y - thickness,
            SnakeGameConfig.BoardSize.X, SnakeGameConfig.BoardSize.Y, SnakeGameConfig.BoardSize.X, 0).ToComponent();
        right.FillColor = Color.SlateGray;
        right.BorderColor = Color.SlateGray;

        var top = Polygon.Create(SnakeGameConfig.BoardSize.X - thickness, SnakeGameConfig.BoardSize.X - thickness,
            thickness, SnakeGameConfig.BoardSize.X - thickness, 0, SnakeGameConfig.BoardSize.Y,
            SnakeGameConfig.BoardSize.X, SnakeGameConfig.BoardSize.Y).ToComponent();
        top.FillColor = Color.SlateGray;
        top.BorderColor = Color.SlateGray;

        var left = Polygon.Create(thickness, SnakeGameConfig.BoardSize.X - thickness, thickness, thickness, 0, 0, 0,
            SnakeGameConfig.BoardSize.Y).ToComponent();
        left.FillColor = Color.SlateGray;
        left.BorderColor = Color.SlateGray;

        Scene.Add(top).Add(right).Add(bottom).Add(left);
        snakeState.Walls.Add(top);
        snakeState.Walls.Add(right);
        snakeState.Walls.Add(bottom);
        snakeState.Walls.Add(left);

        snakeState.GameOver += GameOver;

        await base.OnInitializedAsync();
    }

    private void GameOver(object? sender, int e)
    {
    }
}