using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Behaviours;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages;

public class IndexBase : ComponentBase
{
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }
    public Scene.Scene Scene { get; set; }
    public SceneController SceneController { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //Scene
        Scene = new SceneBuilder().SetWidth(100).SetHeight(100).Build();

        SceneController = new SceneController(BlazorCanvas, Scene)
        {
            ShowGrid = true
        };

        //var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 20), new Vector2(35, 30)).ToComponent()
        //    .AddPhysics(Scene);
        //Scene.Add(rectangle);

        var ball = Ellipse.Create(new Vector2(50, 50), 5).ToComponent().AddPhysics(Scene).AddDragToMove(Scene);
        Scene.Add(ball);

        var ball2 = Ellipse.Create(new Vector2(60, 50), 5).ToComponent().AddPhysics(Scene).AddDragToMove(Scene);
        Scene.Add(ball2);

        var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 70), new Vector2(35, 60)).ToComponent()
            .AddDragToMove(Scene).AddPhysics(Scene);
        Scene.Add(rectangle2);


        var rectangle3 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 30), new Vector2(35, 40)).ToComponent()
            .AddDragToMove(Scene).AddPhysics(Scene);
        Scene.Add(rectangle3);

        var bottom = Polygon.Create(10, 10, 90, 10, 100, 0, 0, 0).ToComponent();
        var right = Polygon.Create(90, 10, 90, 90, 100, 100, 100, 0).ToComponent();
        var top = Polygon.Create(90, 90, 10, 90, 0, 100, 100, 100).ToComponent();
        var left = Polygon.Create(10, 90, 10, 10, 0, 0, 0, 100).ToComponent();
        Scene.Add(top).Add(right).Add(bottom).Add(left);

        await base.OnInitializedAsync();
    }
}