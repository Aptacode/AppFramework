using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class DragBox : PolygonViewModel
    {
        #region Ctor

        public DragBox(Polygon polygon) : base(polygon)
        {
            OnMouseMoveBubbled += Handle_OnMouseMove;
            OnMouseUpBubbled += Handle_OnMouseUp;
        }

        public static DragBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static DragBox FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
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

        public override void Add(ComponentViewModel child)
        {
            child.OnMouseDown += ChildOnOnMouseDown;

            //Ensure child is within the bounds of the DragBox
            var position = GetChildPositionWithinBounds(child);
            child.SetPosition(position);
            
            base.Add(child);
        }

        protected Vector2 GetChildPositionWithinBounds(ComponentViewModel child)
        {
            var xPos = child.BoundingRectangle.TopLeft.X;
            var yPos = child.BoundingRectangle.TopLeft.Y;
            
            if (child.BoundingRectangle.TopLeft.X < BoundingRectangle.TopLeft.X)
            {
                xPos = BoundingRectangle.TopLeft.X;
            }
            else if (child.BoundingRectangle.TopRight.X > BoundingRectangle.TopRight.X)
            {
                xPos = BoundingRectangle.TopRight.X - child.BoundingRectangle.Size.X;
            }

            if (child.BoundingRectangle.TopLeft.Y < BoundingRectangle.TopLeft.Y)
            {
                yPos = BoundingRectangle.TopLeft.Y;
            }
            else if (child.BoundingRectangle.BottomRight.Y > BoundingRectangle.BottomRight.Y)
            {
                yPos = BoundingRectangle.BottomRight.Y - child.BoundingRectangle.Size.Y;
            }

            return new Vector2(xPos, yPos);
        }

        protected bool IsChildPositionWithinBounds(ComponentViewModel child)
        {
            var xPos = child.BoundingRectangle.TopLeft.X;
            var yPos = child.BoundingRectangle.TopLeft.Y;

            if (child.BoundingRectangle.TopLeft.X < BoundingRectangle.TopLeft.X)
            {
                return false;
            }
            else if (child.BoundingRectangle.TopRight.X > BoundingRectangle.TopRight.X)
            {
                return false;
            }

            if (child.BoundingRectangle.TopLeft.Y < BoundingRectangle.TopLeft.Y)
            {
                return false;
            }
            else if (child.BoundingRectangle.BottomRight.Y > BoundingRectangle.BottomRight.Y)
            {
                return false;
            }

            return true;
        }

        public override void Remove(ComponentViewModel child)
        {
            child.OnMouseDown -= ChildOnOnMouseDown;
            base.Remove(child);
        }

        private void ChildOnOnMouseDown(object? sender, MouseDownEvent e)
        {
            IsDragging = true;
            SelectedChild = (ComponentViewModel) sender;
            LastDragPosition = e.Position;
        }

        private void Handle_OnMouseUp(object? sender, MouseUpEvent e)
        {
            IsDragging = false;
            SelectedChild = null;
        }

        #region props

        public bool IsDragging { get; set; }
        public Vector2 LastDragPosition { get; set; }
        public ComponentViewModel? SelectedChild { get; set; }

        #endregion
        
        
        #region Events

        private void Handle_OnMouseMove(object? sender, MouseMoveEvent e)
        {
            if (IsDragging && SelectedChild != null)
            {
                var delta = e.Position - LastDragPosition;
                SelectedChild.Translate(delta);
                if (!IsChildPositionWithinBounds(SelectedChild))
                {
                    SelectedChild.Translate(-delta);
                }
            }
            
            LastDragPosition = e.Position;
        }

        #endregion


        #region IDisposable

        public override void Dispose()
        {
            foreach (var child in Children)
            {
                child.OnMouseDown -= ChildOnOnMouseDown;
            }

            base.Dispose();
        }

        #endregion
    }
}