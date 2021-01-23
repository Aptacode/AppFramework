using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Enums;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;

namespace Aptacode.AppFramework.Components.Controls
{
    public class LinearLayout : PolygonViewModel
    {
        #region Ctor

        public LinearLayout(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) : base(Geometry.Primitives.Polygon.Rectangle.Create(topLeft, topRight, bottomRight, bottomLeft))
        {

        }

        public LinearLayout(Vector2 topLeft, Vector2 bottomRight) : base(Geometry.Primitives.Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight))
        {

        }

        #endregion

        #region Events

        #endregion

        #region Props

        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public Vector2 Size => Primitive.BoundingRectangle.Size;
        public Vector2 Position => Primitive.BoundingRectangle.TopLeft;
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

        public void Resize()
        {
            var position = Position;
            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    var scale = Size.X / child.BoundingRectangle.Size.X;
                    child.Scale(new Vector2(scale, 1));
                    var delta = position - child.BoundingRectangle.TopLeft;
                    child.Translate(delta);
                    position += new Vector2(0, child.BoundingRectangle.Size.Y);
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    var scale = Size.Y / child.BoundingRectangle.Size.Y;
                    child.Scale(new Vector2(1, scale));
                    var delta = position - child.BoundingRectangle.TopLeft;
                    child.Translate(delta);
                    position += new Vector2( child.BoundingRectangle.Size.X, 0);
                }
            }
        }

        public override void Add(ComponentViewModel child)
        {
            base.Add(child);
            Resize();
        }

        public override void Remove(ComponentViewModel child)
        {
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
            for (int i = 0; i < containedChildren.Count() - 1; i++)
            {
                await containedChildren.ElementAt(i).Draw(ctx);
            }
            
            //Todo use globalCompositeOperation to clip last element with Source-In
            //Needs temp canvas
            await containedChildren.Last().Draw(ctx);
        }
    }
}