using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Behaviours;
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
        Scene = new SceneBuilder().SetWidth(100).SetHeight(100).Build();

        SceneController = new DemoSceneController(Scene)
        {
            ShowGrid = true
        };

        var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 10), new Vector2(35, 20)).ToViewModel();
        rectangle.AddDragToMove(Scene).AddVelocity(Scene);
        Scene.Add(rectangle);

        var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 70), new Vector2(35, 60)).ToViewModel();
        rectangle2.AddDragToMove(Scene).AddVelocity(Scene);
        Scene.Add(rectangle2);

        await base.OnInitializedAsync();
    }
}