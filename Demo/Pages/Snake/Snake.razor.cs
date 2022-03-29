using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Components.States.Scene;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages.Snake;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public sealed class SnakeState : SceneState
{
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
}

public sealed class SnakeBodyComponent : PolygonComponent
{
    public SnakeBodyComponent(Polygon primitive) : base(primitive)
    {
    }

    public Direction Direction { get; set; }
}

public sealed class SnakeFoodComponent : PolygonComponent
{
    public SnakeFoodComponent(Polygon primitive) : base(primitive)
    {
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
        Scene = new SceneBuilder().SetWidth(600).SetHeight(600).Build();

        SceneController = new SceneController(BlazorCanvas, Scene)
        {
            ShowGrid = true
        };

        var snakeHead =
            new SnakeBodyComponent(Polygon.Rectangle.FromTwoPoints(new Vector2(250, 250), new Vector2(300, 300)));
        snakeHead.FillColor = Color.LightSlateGray;
        snakeHead.BorderColor = Color.DarkSlateGray;
        snakeHead.Direction = Direction.Up;
        Scene.Add(snakeHead);

        var snakeFood =
            new SnakeFoodComponent(Polygon.Rectangle.FromTwoPoints(new Vector2(200, 200), new Vector2(250, 250)));
        snakeFood.FillColor = Color.Red;
        snakeFood.BorderColor = Color.DarkSlateGray;
        Scene.Add(snakeFood);

        var snakeDirection = new SnakeControlBehaviour(Scene);
        Scene.Add(snakeDirection);

        var snakeState = new SnakeState(Scene);
        snakeState.SnakeHead = snakeHead;
        snakeState.SnakeFood = snakeFood;
        Scene.AddState(snakeState);

        var snakeBehavioru = new SnakeMovementBehaviour(Scene);
        Scene.Add(snakeBehavioru);

        await base.OnInitializedAsync();
    }
}