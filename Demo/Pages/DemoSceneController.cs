﻿using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Scene;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;

namespace Aptacode.AppFramework.Demo.Pages
{
    public class DemoSceneController : SceneController
    {
        private readonly ComponentBuilder _componentBuilder = new();

        public DemoSceneController(Scene.Scene scene) : base(scene)
        {
            UserInteractionController.OnMouseEvent += UserInteractionControllerOnOnMouseEvent;
        }

        private void UserInteractionControllerOnOnMouseEvent(object? sender, MouseEvent e)
        {
            foreach (var component in Scene.Components)
            {
                if (component is ButtonViewModel btn)
                {
                    btn.Text = e.Position.ToString();
                }
            }
        }
    }
}