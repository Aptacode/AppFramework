using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components;
using Aptacode.BlazorCanvas;
using Aptacode.CSharp.Common.Utilities.Mvvm;
using Aptacode.Geometry.Collision;

namespace Aptacode.AppFramework.Scene
{
    public class SceneController : BindableBase
    {
        #region Ctor

        public SceneController(Scene scene)
        {
            Scene = scene;
            CollisionDetector = new HybridCollisionDetector();
            UserInteractionController = new SceneInteractionController();
        }

        #endregion

        #region Movement

        public void Translate(
            ComponentViewModel component,
            Vector2 delta,
            List<ComponentViewModel> movingComponents,
            CancellationTokenSource cancellationToken)
        {
            var unselectedItems = Scene.Components.Except(movingComponents).Where(c => c.CollisionDetectionEnabled);

            component.Translate(delta);

            var collidingItems = unselectedItems
                .Where(i => i.CollidesWith(component)).ToList();

            movingComponents.AddRange(collidingItems);

            foreach (var collidingItem in collidingItems)
            {
                Translate(collidingItem, delta, movingComponents, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                component.Translate(-delta);
            }
        }

        #endregion

        public void Setup(BlazorCanvasInterop canvas)
        {
            Renderer = new SceneRenderer(canvas, Scene);
        }

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
        public Scene Scene { get; }
        public SceneInteractionController UserInteractionController { get; }
        public CollisionDetector CollisionDetector { get; }

        public string Cursor { get; set; }

        #endregion
    }
}