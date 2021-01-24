using System;
using System.Collections.Generic;
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
             scene.Add(CreateDragBox());

            SceneController.Add(scene);

            await base.OnInitializedAsync();
        }

        private DragBox CreateDragBox()
        {
            var componentBuilder = new ComponentBuilder();

            var component = (DragBox)componentBuilder
                .SetBase(DragBox.FromPositionAndSize(new Vector2(10, 10), new Vector2(50, 100)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(100, 100,40,20))
                .SetText("")
                .Build();


            foreach (var button in GenerateButtons(8))
            {
                component.Add(button);
            }

            return component;
        }
        
        private ComponentViewModel GetGrid()
        {
            var componentBuilder = new ComponentBuilder();

            var layout = (GridLayout) componentBuilder
                .SetBase(new GridLayout(new Vector2(100, 10), new Vector2(50, 100)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.LightGray)
                .SetText("")
                .Build();

            layout.Rows = 3;
            layout.Columns = 3;
            layout.HorizontalAlignment = HorizontalAlignment.Stretch;
            layout.VerticalAlignment = VerticalAlignment.Stretch;
            layout.EnforceHorizontalAlignment = true;
            layout.EnforceVerticalAlignment = true;
            foreach (var button in GenerateButtons(8))
            {
                layout.Add(button);
            }

            return layout;
        }
        
        public List<Button> GenerateButtons(int buttonCount)
        {
            List<Button> buttons = new();
            var componentBuilder = new ComponentBuilder();

            var rand = new Random();
            for (var i = 0; i < 8; i++)
            {
                var button = (Button)componentBuilder
                    .SetBase(Button.FromPositionAndSize(new Vector2(i * 10, i * 10), new Vector2(10, 10)))
                    .SetBorderThickness(.5f)
                    .SetMargin(1.0f)
                    .SetFillColor(Color.FromArgb(255, rand.Next(255), rand.Next(255), rand.Next(255)))
                    .SetText($"Button{i + 1}")
                    .Build();

                //button.VerticalAlignment = (VerticalAlignment)(i % 4);
                //button.HorizontalAlignment = (HorizontalAlignment)(i % 4);


                buttons.Add(button);
            }

            return buttons;
        }
    }
}