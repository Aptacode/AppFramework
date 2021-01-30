using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class Image : PolygonViewModel
    {
        private bool _isLoaded;
        private bool _isLoading;

        private string _path = string.Empty;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                Invalidated = true;
            }
        }

        public override async Task CustomDraw(BlazorCanvasInterop ctx)
        {
            await base.CustomDraw(ctx);

            if (Path == string.Empty)
            {
                return;
            }

            if (!_isLoaded && !_isLoading)
            {
                _isLoading = true;
                _ = new TaskFactory().StartNew(async () =>
                {
                    Console.WriteLine($"Loading: {Path}");
                    await ctx.LoadImage(Path);
                    Console.WriteLine($"Loaded: {Path}");
                    _isLoaded = true;
                });

            }

            if (_isLoaded)
            {
                ctx.DrawImage(Path, BoundingRectangle.X * SceneScale.Value, BoundingRectangle.Y * SceneScale.Value, BoundingRectangle.Width * SceneScale.Value, BoundingRectangle.Height * SceneScale.Value);
            }
        }

        #region Ctor

        public Image(ComponentViewModel parent, Polygon polygon, string path) : base(parent, polygon)
        {
            Path = path;
            OnMouseDown += Handle_OnMouseDown;
            OnMouseUp += Handle_OnMouseUp;
        }

        public static Image FromPositionAndSize(ComponentViewModel parent, Vector2 position, Vector2 size, string path)
        {
            return new(parent, Polygon.Rectangle.FromPositionAndSize( position, size), path);
        }

        public static Image FromTwoPoints(ComponentViewModel parent, Vector2 topLeft, Vector2 bottomRight, string path)
        {
            return new(parent, Polygon.Rectangle.FromTwoPoints( topLeft, bottomRight), path);
        }

        #endregion

        #region Events

        private void Handle_OnMouseDown(object? sender, MouseDownEvent e)
        {
            BorderColor = Color.Green;
        }

        private void Handle_OnMouseUp(object? sender, MouseUpEvent e)
        {
            BorderColor = Color.Black;
        }

        #endregion
    }
}