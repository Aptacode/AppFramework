using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.AppFramework.Components.Containers;
using Aptacode.AppFramework.Components.Containers.Layouts;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Enums;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class IndexBase : ComponentBase
    {
        public DemoSceneController SceneController { get; set; }
        public Scene.Scene Scene { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //Scene
            Scene = new SceneBuilder().SetWidth(200).SetHeight(100).Build();
            SceneController = new DemoSceneController(new Vector2(200, 200));
            SceneController.ShowGrid = true;

            Scene.Add(GetGrid());
            Scene.Add(CreateScrollBox());
            Scene.Add(CreateDragBox());
            Scene.Add(CreateNestedImage());

            SceneController.Add(Scene);

            await base.OnInitializedAsync();
        }

        private ScrollBox CreateScrollBox()
        {
            var componentBuilder = new ComponentBuilder();

            var component = (ScrollBox)componentBuilder
                .SetBase(ScrollBox.FromPositionAndSize( new Vector2(125, 10), new Vector2(50, 70)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(20, 100, 40, 20))
                .SetText("")
                .Build();

            var child = new Button( Polygon.Rectangle.FromPositionAndSize(new Vector2(140, 25), new Vector2(10, 10)));
            component.Add(child);

            return component;
        }

        private Image CreateImage()
        {
            var componentBuilder = new ComponentBuilder();

            var component = (Image) componentBuilder
                .SetBase(Image.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10), "https://raw.githubusercontent.com/Aptacode/AppFramework/Production/Resources/Images/Logo.png"))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(100, 100, 40, 20))
                .SetText("")
                .Build();

            return component;
        }

        private Button CreateNestedImage()
        {
            var componentBuilder = new ComponentBuilder();

            var button = (Button)componentBuilder
                .SetBase(Button.FromPositionAndSize( new Vector2(0, 0), new Vector2(20, 10)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(255, 100, 40, 20))
                .SetText("123")
                .Build();
            
            var image = (Image)componentBuilder
                .SetBase(Image.FromPositionAndSize(new Vector2(10, 10), new Vector2(10, 10), "https://raw.githubusercontent.com/Aptacode/AppFramework/Production/Resources/Images/Logo.png"))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(100, 40, 40, 20))
                .SetText("test")
                .Build();
            
            image.VerticalAlignment = VerticalAlignment.Stretch;
            image.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.Add(image);
            
            return button;
        }

        private DragBox CreateDragBox()
        {
            var componentBuilder = new ComponentBuilder();

            var component = (DragBox) componentBuilder
                .SetBase(DragBox.FromPositionAndSize( new Vector2(10, 10), new Vector2(50, 100)))
                .SetBorderThickness(0.2f)
                .SetMargin(0.0f)
                .SetFillColor(Color.FromArgb(100, 100, 40, 20))
                .SetText("")
                .Build();


            foreach (var button in GenerateButtons(8))
            {
                component.Add(button);
                button.OnMouseClick += ButtonOnOnMouseClick;
            }

            return component;
        }

        private void ButtonOnOnMouseClick(object? sender, MouseClickEvent e)
        {
            Scene.Add(CreateImage());
        }

        private ComponentViewModel GetGrid()
        {
            var componentBuilder = new ComponentBuilder();

            var layout = (GridLayout) componentBuilder
                .SetBase(new GridLayout( new Vector2(65, 10), new Vector2(120, 100)))
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
                layout.AddNextAvailableCell(button);
            }

            return layout;
        }

        public List<Button> GenerateButtons(int buttonCount)
        {
            List<Button> buttons = new();
            var componentBuilder = new ComponentBuilder();

            var rand = new Random();
            for (var i = 0; i < buttonCount; i++)
            {
                var button = (Button) componentBuilder
                    .SetBase(Button.FromPositionAndSize( new Vector2(i * 10, i * 10), new Vector2(10, 10)))
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