using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Utilities;
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
            SceneController = new DemoSceneController(new Vector2(200,200));

            var componentBuilder = new ComponentBuilder();
            var button = componentBuilder
                .SetBase(new ButtonViewModel(new Vector2(10,10), new Vector2(20,20)))
                .SetBorderThickness(0.0f)
                .SetMargin(0.0f)
                .SetText("Click Me")
                .Build();

            var button2 = componentBuilder
                .SetBase(new ButtonViewModel(new Vector2(50, 30), new Vector2(10, 5)))
                .SetBorderThickness(0.0f)
                .SetMargin(0.0f)
                .SetText("")
                .Build();

            scene.Add(button);
            scene.Add(button2);
            button.OnMouseDown += delegate { button.BorderColor = Color.AliceBlue; };
            button.OnMouseUp += delegate { button.BorderColor = Color.Green; };

            SceneController.Add(scene);

            await base.OnInitializedAsync();
        }
    }
}