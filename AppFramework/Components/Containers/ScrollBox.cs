
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Containers
{
    public class ScrollBox : PolygonViewModel
    {
        #region Ctor
        
        public ScrollBox(Polygon polygon) : base(polygon)
        {
            ScrollBar = new ScrollBar(
                Polygon.Rectangle.FromPositionAndSize(
                    BoundingRectangle.TopRight - new Vector2(ScrollBarWidth, 0), 
                    new Vector2(ScrollBarWidth, polygon.BoundingRectangle.Height)));
            
            ScrollBar.OnScroll += ScrollBarOnOnScroll;

            base.Add(ScrollBar);
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
            ScrollChildren.Add(child);
            base.Add(child);
        }

        public override void Remove(ComponentViewModel child)
        {
            ScrollChildren.Remove(child);
            base.Remove(child);
        }

        private float lastScrollPosition = 0.0f;
        private void ScrollBarOnOnScroll(object? sender, float e)
        {
            Console.WriteLine(e);

            if(lastScrollPosition > Constants.Tolerance)
            {
                var delta = new Vector2(0, (e - lastScrollPosition) * BoundingRectangle.Height);
                Console.WriteLine(delta);
                foreach (var child in ScrollChildren)
                {
                    child.Translate(delta);
                }
            }
            
            lastScrollPosition = e;
        }

        public static ScrollBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static ScrollBox FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Props

        public ScrollBar ScrollBar { get; set; }
        public List<ComponentViewModel> ScrollChildren { get; set; } = new List<ComponentViewModel>();
        public float ScrollBarWidth { get; set; } = 5;

        #endregion


    }
}
