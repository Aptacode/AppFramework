using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Components.Layouts;
using Aptacode.AppFramework.Enums;
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

            scene.Add(GetGrid());
            
            SceneController.Add(scene);

            await base.OnInitializedAsync();
        }

        private ComponentViewModel GetGrid()
        {
            var componentBuilder = new ComponentBuilder();
            var layout = (GridLayout)componentBuilder
                .SetBase(new GridLayout(new Vector2(20, 20), new Vector2(160, 80)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.LightGray)
                .SetText("")
                .Build();

            layout.Rows = 3;
            layout.Columns = 3;
            layout.HorizontalAlignment = HorizontalAlignment.Stretch;
            layout.VerticalAlignment = VerticalAlignment.Center;

            var rand = new Random();
            var colors = new[]
            {
                Color.Green,Color.Blue,Color.Yellow,Color.Red,Color.Chocolate,Color.DarkSlateBlue,Color.LawnGreen, Color.DarkTurquoise
            };
            for (int i = 0; i < 8; i++)
            {
                var button = componentBuilder
                    .SetBase(Button.FromPositionAndSize(new Vector2(i * 10, i * 10), new Vector2(10, 10)))
                    .SetBorderThickness(.5f)
                    .SetMargin(1.0f)
                    .SetFillColor(colors[i])
                    .SetText($"Button{i + 1}")
                    .Build();

                layout.Add(button);
            }

            return layout;
        }
    }
}