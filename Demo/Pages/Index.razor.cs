using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Behaviours;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Utilities;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages;

public class IndexBase : ComponentBase
{
    public DemoSceneController SceneController { get; set; }
    public Scene.Scene Scene { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //Scene
        Scene = new SceneBuilder().SetWidth(200).SetHeight(200).Build();
        SceneController = new DemoSceneController(new Vector2(200, 200))
        {
            ShowGrid = true
        };

        var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(10, 10), new Vector2(20, 20)).ToViewModel();
        rectangle.AddDragToMove(Scene).AddCollisions(Scene).AddVelocity(Scene);
        Scene.Add(rectangle);

        var ellipse1 = Ellipse.Circle.Create(new Vector2(30,30), 10).ToViewModel();
        ellipse1.AddDragToMove(Scene).AddCollisions(Scene);
        Scene.Add(ellipse1);

        var ellipse2 = Ellipse.Circle.Create(new Vector2(45,45), 5).ToViewModel();
        ellipse2.AddDragToMove(Scene).AddCollisions(Scene);
        Scene.Add(ellipse2);

        SceneController.Add(Scene);

        await base.OnInitializedAsync();
    }
}