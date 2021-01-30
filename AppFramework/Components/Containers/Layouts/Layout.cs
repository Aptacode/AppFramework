using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Enums;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Containers.Layouts
{
    public abstract class Layout : PolygonViewModel
    {
        private string _tempCanvasName = string.Empty;

        #region Ctor

        protected Layout(Polygon polygon) : base(polygon)
        {
        }

        #endregion

        public override void UpdateBounds()
        {
            if (Primitive == null)
            {
                BoundingRectangle = BoundingRectangle.Zero;
            }
            else
            {
                BoundingRectangle = Primitive.BoundingRectangle;
            }
        }

        public abstract void Resize();

        protected void RepositionChild(ComponentViewModel child, Vector2 cellSize, Vector2 cellPosition)
        {
            var xPos = cellPosition.X;
            var yPos = cellPosition.Y;
            var xSize = cellSize.X;
            var ySize = cellSize.Y;

            switch (EnforceVerticalAlignment ? VerticalAlignment : child.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    ySize = cellSize.Y;
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Top:
                    ySize = Math.Min(cellSize.Y, child.BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Center:
                    ySize = Math.Min(cellSize.Y, child.BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y + ySize / 2.0f;
                    break;
                case VerticalAlignment.Bottom:
                    ySize = Math.Min(cellSize.Y, child.BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y + (cellSize.Y - ySize);
                    break;
            }

            switch (EnforceHorizontalAlignment ? HorizontalAlignment : child.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    xSize = cellSize.X;
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Left:
                    xSize = Math.Min(cellSize.X, child.BoundingRectangle.Size.X);
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Center:
                    xSize = Math.Min(cellSize.X, child.BoundingRectangle.Size.X);
                    xPos = cellPosition.X + xSize / 2.0f;
                    break;
                case HorizontalAlignment.Right:
                    xSize = Math.Min(cellSize.X, child.BoundingRectangle.Size.X);
                    xPos = cellPosition.X + xSize;
                    break;
            }

            child.SetPosition(new Vector2(xPos + child.Margin, yPos + child.Margin));
            child.SetSize(new Vector2(xSize - 2 * child.Margin, ySize - 2 * child.Margin));
        }

        public override void Add(ComponentViewModel child)
        {
            child.OnHorizontalAlignmentChanged += ChildOnOnHorizontalAlignmentChanged;
            child.OnVerticalAlignmentChanged += ChildOnOnVerticalAlignmentChanged;
            base.Add(child);
            Resize();
        }

        private void ChildOnOnVerticalAlignmentChanged(object? sender, VerticalAlignment e)
        {
            Resize();
        }

        private void ChildOnOnHorizontalAlignmentChanged(object? sender, HorizontalAlignment e)
        {
            Resize();
        }

        public override void Remove(ComponentViewModel child)
        {
            child.OnHorizontalAlignmentChanged -= ChildOnOnHorizontalAlignmentChanged;
            child.OnVerticalAlignmentChanged -= ChildOnOnVerticalAlignmentChanged;
            base.Remove(child);
            Resize();
        }

        public override async Task Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
        {
            OldBoundingRectangle = BoundingRectangle;
            Invalidated = false;

            if (!IsShown)
            {
                return;
            }

            ctx.FillStyle(FillColorName);

            ctx.StrokeStyle(BorderColorName);

            ctx.LineWidth(BorderThickness * SceneScale.Value);
            await CustomDraw(ctx);

            if (_tempCanvasName == string.Empty)
            {
                _tempCanvasName = scene.Id + "temp";
                await ctx.CreateCanvas(_tempCanvasName, scene.Size.X * SceneScale.Value, scene.Size.Y * SceneScale.Value);
            }

            ctx.SelectCanvas(_tempCanvasName);
            ctx.Save();
            ctx.ClearRect(0, 0, scene.Size.X * SceneScale.Value, scene.Size.Y * SceneScale.Value);

            foreach (var componentViewModel in Children.Where(c => c.CollidesWith(this)))
            {
                await componentViewModel.Draw(scene, ctx);
            }

            ctx.GlobalCompositeOperation(CompositeOperation.DestinationIn);

            ctx.FillRect(BoundingRectangle.X * SceneScale.Value, BoundingRectangle.Y * SceneScale.Value, BoundingRectangle.Width * SceneScale.Value, BoundingRectangle.Height * SceneScale.Value);
            ctx.Restore();

            ctx.SelectCanvas(scene.Id.ToString());
            ctx.DrawCanvas(_tempCanvasName, 0, 0);
        }

        #region IDisposable

        public override void Dispose()
        {
            foreach (var child in Children)
            {
                child.OnHorizontalAlignmentChanged -= ChildOnOnHorizontalAlignmentChanged;
                child.OnVerticalAlignmentChanged -= ChildOnOnVerticalAlignmentChanged;
            }

            base.Dispose();
        }

        #endregion

        #region Events

        #endregion

        #region Props

        public Vector2 Size => Primitive.BoundingRectangle.Size;
        public Vector2 Position => Primitive.BoundingRectangle.TopLeft;

        private bool _enforceVerticalAlignment;

        public bool EnforceVerticalAlignment
        {
            get => _enforceVerticalAlignment;
            set
            {
                _enforceVerticalAlignment = value;
                Resize();
            }
        }

        private bool _enforceHorizontalAlignment;

        public bool EnforceHorizontalAlignment
        {
            get => _enforceHorizontalAlignment;
            set
            {
                _enforceHorizontalAlignment = value;
                Resize();
            }
        }

        #endregion
    }
}