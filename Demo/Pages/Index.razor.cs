using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Components.Layouts;
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
            SceneController = new DemoSceneController(new Vector2(200, 200));

            var componentBuilder = new ComponentBuilder();
            var button1 = componentBuilder
                .SetBase(Button.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10)))
                .SetBorderThickness(.2f)
                .SetMargin(1.0f)
                .SetFillColor(Color.Gray)
                .SetText("Click Me")
                .Build();

            var button2 = componentBuilder
                .SetBase(Button.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10)))
                .SetBorderThickness(0.2f)
                .SetMargin(1.0f)
                .SetFillColor(Color.Yellow)
                .SetText("Button 2")
                .Build();

            var button3 = componentBuilder
                .SetBase(Button.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10)))
                .SetBorderThickness(0.2f)
                .SetMargin(1.0f)
                .SetFillColor(Color.Green)
                .SetText("Button 2")
                .Build();

            var button4 = componentBuilder
                .SetBase(Button.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10)))
                .SetBorderThickness(0.2f)
                .SetMargin(1.0f)
                .SetFillColor(Color.BlanchedAlmond)
                .SetText("Button 2")
                .Build();

            var button5 = componentBuilder
                .SetBase(Button.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10)))
                .SetBorderThickness(0.2f)
                .SetMargin(1.0f)
                .SetFillColor(Color.Brown)
                .SetText("Button 2")
                .Build();

            var layout = componentBuilder
                .SetBase(new LinearLayout(new Vector2(20, 20), new Vector2(50, 80)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.Aquamarine)
                .SetText("")
                .Build();

            layout.Add(button1);
            layout.Add(button2);
            layout.Add(button3);
            layout.Add(button4);
            layout.Add(button5);

            scene.Add(layout);


            SceneController.Add(scene);

            await base.OnInitializedAsync();
        }
    }
}