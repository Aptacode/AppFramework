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

public class SnakeBase : ComponentBase
{
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }
    public Scene.Scene Scene { get; set; }
    public SceneController SceneController { get; set; }
    public SnakeState SnakeState { get; set; }

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

        SnakeState = new SnakeState(Scene)
        {
            SnakeHead = snakeHead,
            SnakeFood = snakeFood
        };
        Scene.Plugins.State.Add(SnakeState);

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
        SnakeState.Walls.Add(top);
        SnakeState.Walls.Add(right);
        SnakeState.Walls.Add(bottom);
        SnakeState.Walls.Add(left);

        SnakeState.GameOver += GameOver;
        SnakeState.ScoreChanged += ScoreChanged;

        await base.OnInitializedAsync();
    }

    private void ScoreChanged(object? sender, int e)
    {
        StateHasChanged();
    }

    private void GameOver(object? sender, int e)
    {
        StateHasChanged();
    }
}