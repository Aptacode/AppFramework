using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Utilities;
using Aptacode.Geometry.Primitives.Polygons;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class IndexBase : ComponentBase
    {
        public DemoSceneController SceneController { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Scene
            var scene = new SceneBuilder().SetWidth(200).SetHeight(100).Build();
            SceneController = new DemoSceneController(scene);

            var componentBuilder = new ComponentBuilder();
            var button = componentBuilder
                .SetBase(new ButtonViewModel(Rectangle.Create(2, 2, 10, 5)))
                .SetBorderThickness(0.5f)
                .SetText("Click Me")
                .Build();

            var button2 = componentBuilder
                .SetBase(new ButtonViewModel(Rectangle.Create(20, 2, 10, 5)))
                .SetBorderThickness(0.5f)
                .SetText("")
                .Build();

            scene.Add(button);
            scene.Add(button2);

            button.OnMouseUp += delegate { button2.Text = "Up"; };

            button.OnMouseClick += delegate { button2.Text = "Click"; };

            button.OnMouseDown += delegate { button2.Text = "Down"; };

            await base.OnInitializedAsync();
        }
    }
}