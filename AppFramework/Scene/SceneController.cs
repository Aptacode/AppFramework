using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.BlazorCanvas;
using Aptacode.CSharp.Common.Utilities.Mvvm;

namespace Aptacode.AppFramework.Scene
{
    public class SceneController : BindableBase
    {
        public void Setup(BlazorCanvasInterop canvas)
        {
            Renderer = new SceneRenderer(canvas, this);
        }

        #region Ctor

        public SceneController(Vector2 size)
        {
            Size = size;
            Scenes = new List<Scene>();
            UserInteractionController = new SceneInteractionController();
            UserInteractionController.OnMouseEvent += UserInteractionControllerOnOnMouseEvent;
        }

        private void UserInteractionControllerOnOnMouseEvent(object? sender, MouseEvent e)
        {
            foreach (var scene in Scenes)
            {
                foreach (var componentViewModel in scene.Components)
                {
                    componentViewModel.HandleMouseEvent(e);
                }
            }
        }

        #endregion

        #region Events

        private DateTime _lastTick = DateTime.Now;

        public virtual async Task Tick()
        {
            var currentTime = DateTime.Now;
            var delta = currentTime - _lastTick;
            var frameRate = 1.0f / delta.TotalSeconds;
            _lastTick = currentTime;
            //Console.WriteLine($"{frameRate}fps");

            await Renderer.Redraw();
        }

        #endregion

        #region Properties

        public SceneRenderer Renderer { get; private set; }
        public SceneInteractionController UserInteractionController { get; }
        public List<Scene> Scenes { get; set; }

        public string Cursor { get; set; }
        public bool ShowGrid { get; set; }
        public Vector2 Size { get; }

        #endregion

        #region Scene

        public void Add(Scene scene)
        {
            Scenes.Add(scene);
        }

        public bool Remove(Scene scene)
        {
            return Scenes.Remove(scene);
        }

        #endregion
    }
}