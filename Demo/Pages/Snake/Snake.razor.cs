using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
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
        Scene.Plugins.Ui.Add(snakeDirection);

        var snakeState = new SnakeState(Scene);
        snakeState.SnakeHead = snakeHead;
        snakeState.SnakeFood = snakeFood;
        Scene.Plugins.State.Add(snakeState);

        var snakeBehavioru = new SnakeMovementBehaviour(Scene);
        Scene.Plugins.Tick.Add(snakeBehavioru);

        await base.OnInitializedAsync();
    }
}