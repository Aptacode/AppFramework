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

namespace Aptacode.AppFramework.Components.Layouts
{
    public abstract class Layout : PolygonViewModel
    {
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
            var xPos = child.BoundingRectangle.TopLeft.X;
            var yPos = child.BoundingRectangle.TopLeft.Y;
            var xSize = child.BoundingRectangle.Size.X;
            var ySize = child.BoundingRectangle.Size.Y;

            switch (EnforceVerticalAlignment ? VerticalAlignment : child.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    ySize = cellSize.Y;
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Top:
                    yPos = cellPosition.Y;
                    break;
                case VerticalAlignment.Center:
                    yPos = cellPosition.Y + (cellSize.Y - child.BoundingRectangle.Size.Y) / 2.0f;
                    break;
                case VerticalAlignment.Bottom:
                    ySize = Math.Min(cellSize.Y, child.BoundingRectangle.Size.Y);
                    yPos = cellPosition.Y + (cellSize.Y - child.BoundingRectangle.Size.Y);
                    break;
            }

            switch (EnforceHorizontalAlignment ? HorizontalAlignment : child.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    xSize = cellSize.X;
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Left:
                    xPos = cellPosition.X;
                    break;
                case HorizontalAlignment.Center:
                    xPos = cellPosition.X + (cellSize.X - child.BoundingRectangle.Size.X) / 2.0f;
                    break;
                case HorizontalAlignment.Right:
                    xSize = Math.Min(cellSize.X, child.BoundingRectangle.Size.X);
                    xPos = cellPosition.X + (cellSize.X - child.BoundingRectangle.Size.X);
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

        public override async Task Draw(BlazorCanvasInterop ctx)
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

            var containedChildren = Children.Where(c => c.CollidesWith(this));
            for (var i = 0; i < containedChildren.Count() - 1; i++)
            {
                await containedChildren.ElementAt(i).Draw(ctx);
            }

            //Todo use globalCompositeOperation to clip last element with Source-In
            //Needs temp canvas
            await containedChildren.Last().Draw(ctx);
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