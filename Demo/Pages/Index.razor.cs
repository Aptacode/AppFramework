using System.Numerics;
using System.Threading.Tasks;
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
        Scene.Add(rectangle);

        var ellipse = Ellipse.Circle.Create(new Vector2(10, 10), 10).ToViewModel();
        Scene.Add(ellipse);

        SceneController.Add(Scene);

        await base.OnInitializedAsync();
    }
}