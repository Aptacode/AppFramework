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

        var rectangle = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 10), new Vector2(35, 20)).ToViewModel()
            .AddDragToMove(Scene).AddVelocity(Scene);
        Scene.Add(rectangle);

        var rectangle2 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 70), new Vector2(35, 60)).ToViewModel()
            .AddDragToMove(Scene).AddVelocity(Scene);
        rectangle.Add(rectangle2);


        var rectangle3 = Polygon.Rectangle.FromTwoPoints(new Vector2(45, 30), new Vector2(35, 40)).ToViewModel()
            .AddDragToMove(Scene).AddVelocity(Scene);
        Scene.Add(rectangle3);

        await base.OnInitializedAsync();
    }
}