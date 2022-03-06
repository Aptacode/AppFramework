using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Behaviours;
using Aptacode.AppFramework.Components.States;
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
        Scene = new SceneBuilder().SetWidth(100).SetHeight(100).AddPhysics().Build();

        SceneController = new SceneController(BlazorCanvas, Scene)
        {
            ShowGrid = true
        };

        var ball = Ellipse.Create(new Vector2(50, 50), 5).ToComponent().AddDragToMove(Scene);
        ball.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        ball.FillColor = Color.LightSlateGray;
        ball.BorderColor = Color.DarkSlateGray;

        Scene.Add(ball);

        var ball2 = Ellipse.Create(new Vector2(65, 45), 5).ToComponent().AddDragToMove(Scene);
        ball2.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        ball2.FillColor = Color.SlateGray;
        ball2.BorderColor = Color.DarkSlateGray;

        Scene.Add(ball2);

        var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 20), new Vector2(35, 30)).ToComponent().AddDragToMove(Scene);
        rectangle.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        rectangle.FillColor = Color.SlateGray;
        rectangle.BorderColor = Color.DarkSlateGray;

        Scene.Add(rectangle);

        var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 70), new Vector2(35, 60)).ToComponent().AddDragToMove(Scene);
        rectangle2.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        rectangle2.FillColor = Color.SlateGray;
        rectangle2.BorderColor = Color.DarkSlateGray;

        Scene.Add(rectangle2);

        var bottom = Polygon.Create(10, 10, 90, 10, 100, 0, 0, 0).ToComponent();
        bottom.FillColor = Color.SlateGray;
        bottom.BorderColor = Color.SlateGray;

        var right = Polygon.Create(90, 10, 90, 90, 100, 100, 100, 0).ToComponent();
        right.FillColor = Color.SlateGray;
        right.BorderColor = Color.SlateGray;

        var top = Polygon.Create(90, 90, 10, 90, 0, 100, 100, 100).ToComponent();
        top.FillColor = Color.SlateGray;
        top.BorderColor = Color.SlateGray;

        var left = Polygon.Create(10, 90, 10, 10, 0, 0, 0, 100).ToComponent();
        left.FillColor = Color.SlateGray;
        left.BorderColor = Color.SlateGray;

        top.AddPhysics().IsFixed();
        bottom.AddPhysics().IsFixed();
        left.AddPhysics().IsFixed();
        right.AddPhysics().IsFixed();

        Scene.Add(top).Add(right).Add(bottom).Add(left);

        await base.OnInitializedAsync();
    }
}