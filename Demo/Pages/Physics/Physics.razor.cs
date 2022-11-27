using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Demo.Pages.Physics.Behaviours;
using Aptacode.AppFramework.Demo.Pages.Physics.States;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages.Physics;

public class PhysicsBase : ComponentBase
{
    public Scene Scene { get; set; } =
    new()
    {
        Size = new Vector2(1000)
    };

protected override async Task OnInitializedAsync()
    {
        //Scene
        Scene.Plugins.Add(new ScenePhysicsBehaviour(Scene){ Gravity = false, Friction = false});

        var random = new Random();
        int size = 10;
        for (int i = 0; i < 300; i++)
        {
            var posX = random.Next(0 + size, 1000 - size);
            var posY = random.Next(0 + size, 1000 - size);
            var ball = Ellipse.Create(new Vector2(posX, posY), size).ToComponent();
            ball.AddPhysics(Scene).SetHorizontalVelocity(-2,2).SetVerticalVelocity(-2, 2);
            ball.FillColor = Color.LightSlateGray;
            ball.BorderColor = Color.DarkSlateGray;

            Scene.Add(ball);

        }
        //var ball = Ellipse.Create(new Vector2(500, 500), 50).ToComponent().AddDragToMove(Scene);
        //ball.AddPhysics(Scene).SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        //ball.FillColor = Color.LightSlateGray;
        //ball.BorderColor = Color.DarkSlateGray;

        //Scene.Add(ball);

        //var ball2 = Ellipse.Create(new Vector2(650, 450), 50).ToComponent().AddDragToMove(Scene);
        //ball2.AddPhysics(Scene).SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        //ball2.FillColor = Color.SlateGray;
        //ball2.BorderColor = Color.DarkSlateGray;

        //Scene.Add(ball2);

        //var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(450, 200), new Vector2(350, 300)).ToComponent()
        //    .AddDragToMove(Scene);
        //rectangle.AddPhysics(Scene).SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        //rectangle.Plugins.Add(new ArrowKeyBehaviour(Scene, rectangle));

        //rectangle.FillColor = Color.SlateGray;
        //rectangle.BorderColor = Color.DarkSlateGray;

        //Scene.Add(rectangle);

        //var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(450, 700), new Vector2(350, 600)).ToComponent()
        //    .AddDragToMove(Scene);
        //rectangle2.AddPhysics(Scene).SetHorizontalVelocity(-1, 1).SetVerticalVelocity(-1, 1);
        //rectangle2.FillColor = Color.SlateGray;
        //rectangle2.BorderColor = Color.DarkSlateGray;

        //Scene.Add(rectangle2);

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

        top.AddPhysics(Scene).IsFixed();
        bottom.AddPhysics(Scene).IsFixed();
        left.AddPhysics(Scene).IsFixed();
        right.AddPhysics(Scene).IsFixed();

        Scene.Add(top).Add(right).Add(bottom).Add(left);

        await base.OnInitializedAsync();
    }
}