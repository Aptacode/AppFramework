using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Primitives;
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
            for (var i = 0; i < containedChildren.Count() - 1; i++)
            {
                await containedChildren.ElementAt(i).Draw(ctx);
            }

            //Todo use globalCompositeOperation to clip last element with Source-In
            //Needs temp canvas
            await containedChildren.Last().Draw(ctx);
        }

        #region Events

        #endregion

        #region Props
        
        public Vector2 Size => Primitive.BoundingRectangle.Size;
        public Vector2 Position => Primitive.BoundingRectangle.TopLeft;

        #endregion
    }
}