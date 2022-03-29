using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Behaviours;
using Aptacode.AppFramework.Components.Behaviours.Ui;
using Aptacode.AppFramework.Components.States.Component;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages;

public class PhysicsBase : ComponentBase
{
    [Inject] public BlazorCanvasInterop BlazorCanvas { get; set; }
    public Scene.Scene Scene { get; set; }
    public SceneController SceneController { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //Scene
        Scene = new SceneBuilder().SetWidth(1000).SetHeight(1000).AddPhysics().Build();

        SceneController = new SceneController(BlazorCanvas, Scene)
        {
            ShowGrid = true
        };

        var ball = Ellipse.Create(new Vector2(500, 500), 50).ToComponent().AddDragToMove(Scene);
        ball.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        ball.FillColor = Color.LightSlateGray;
        ball.BorderColor = Color.DarkSlateGray;

        Scene.Add(ball);

        var ball2 = Ellipse.Create(new Vector2(650, 450), 50).ToComponent().AddDragToMove(Scene);
        ball2.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        ball2.FillColor = Color.SlateGray;
        ball2.BorderColor = Color.DarkSlateGray;

        Scene.Add(ball2);

        var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(450, 200), new Vector2(350, 300)).ToComponent()
            .AddDragToMove(Scene);
        rectangle.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        rectangle.AddUiBehaviour(new ArrowKeyBehaviour(Scene, rectangle));

        rectangle.FillColor = Color.SlateGray;
        rectangle.BorderColor = Color.DarkSlateGray;

        Scene.Add(rectangle);

        var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(450, 700), new Vector2(350, 600)).ToComponent()
            .AddDragToMove(Scene);
        rectangle2.AddPhysics().SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        rectangle2.FillColor = Color.SlateGray;
        rectangle2.BorderColor = Color.DarkSlateGray;

        Scene.Add(rectangle2);

        var bottom = Polygon.Create(100, 100, 900, 100, 1000, 0, 0, 0).ToComponent();
        bottom.FillColor = Color.SlateGray;
        bottom.BorderColor = Color.SlateGray;

        var right = Polygon.Create(900, 100, 900, 900, 1000, 1000, 1000, 0).ToComponent();
        right.FillColor = Color.SlateGray;
        right.BorderColor = Color.SlateGray;

        var top = Polygon.Create(900, 900, 100, 900, 0, 1000, 1000, 1000).ToComponent();
        top.FillColor = Color.SlateGray;
        top.BorderColor = Color.SlateGray;

        var left = Polygon.Create(100, 900, 100, 100, 0, 0, 0, 1000).ToComponent();
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