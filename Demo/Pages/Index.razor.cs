using System.Drawing;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Microsoft.AspNetCore.Components;
using Rectangle = Aptacode.Geometry.Primitives.Polygons.Rectangle;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class IndexBase : ComponentBase
    {
        public DemoSceneController SceneController { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Scene
            var scene = new SceneBuilder().SetWidth(200).SetHeight(100).Build();
            SceneController = new DemoSceneController();

            var componentBuilder = new ComponentBuilder();
            var button = componentBuilder
                .SetBase(new ButtonViewModel(Rectangle.Create(10,10,10,10)))
                .SetBorderThickness(0.0f)
                .SetMargin(0.0f)
                .SetText("Click Me")
                .Build();

            var button2 = componentBuilder
                .SetBase(new ButtonViewModel(Rectangle.Create(20, 2, 10, 5)))
                .SetBorderThickness(0.0f)
                .SetMargin(0.0f)
                .SetText("")
                .Build();

            scene.Add(button);
            scene.Add(button2);
            button.OnMouseDown += delegate(object? sender, MouseDownEvent e) { button.BorderColor = Color.AliceBlue; };
            button.OnMouseUp += delegate(object? sender, MouseUpEvent e) { button.BorderColor = Color.Green; };

            SceneController.Add(scene);
            
            await base.OnInitializedAsync();
        }

    }
}