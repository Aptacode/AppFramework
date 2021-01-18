using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Collision;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components.Primitives
{
    public class PolygonViewModel : ComponentViewModel
    {
        #region Canvas

        public PolygonViewModel(Polygon polygon)
        {
            Polygon = polygon;
            OldBoundingRectangle = BoundingRectangle =
                Children.ToBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
        }

        #endregion

        #region Ctor

        public override async Task CustomDraw(BlazorCanvasInterop ctx)
        {
            var vertices = new Vector2[Polygon.Vertices.Length];
            for (var i = 0; i < Polygon.Vertices.Length; i++)
            {
                vertices[i] = Polygon.Vertices[i] * SceneScale.Value;
            }

            ctx.Polygon(vertices);

            ctx.Fill();
            ctx.Stroke();
        }

        #endregion

        #region Props

        private Polygon _polygon;

        public Polygon Polygon
        {
            get => _polygon;
            set
            {
                _polygon = value;
                UpdateMargin();
                Invalidated = true;
            }
        }

        public override void UpdateMargin()
        {
            if (_polygon == null)
            {
                return;
            }
            BoundingPrimitive = Polygon.Create(_polygon.Vertices.Vertices.ToArray());
return;

            if (Margin > Constants.Tolerance)
            {
                BoundingPrimitive = new Polygon(_polygon.Vertices.ToConvexHull(Margin));
            }
            else
            {
                BoundingPrimitive = Polygon.Create(_polygon.Vertices.Vertices.ToArray());
            }
        }

        #endregion

        #region Transformations

        public override void Translate(Vector2 delta)
        {
            Polygon.Translate(delta);
            BoundingPrimitive.Translate(delta); 
            UpdateMargin();

            base.Translate(delta);
        }

        public override void Scale(Vector2 delta)
        {
            Polygon.Scale(delta);
            UpdateMargin();

            base.Scale(delta);
        }

        public override void Rotate(float theta)
        {
            Polygon.Rotate(theta);
            UpdateMargin();

            base.Rotate(theta);
        }

        public override void Rotate(Vector2 rotationCenter, float theta)
        {
            Polygon.Rotate(rotationCenter, theta);
            UpdateMargin();

            base.Rotate(rotationCenter, theta);
        }

        public override void Skew(Vector2 delta)
        {
            Polygon.Skew(delta);
            UpdateMargin();
            base.Skew(delta);
        }

        #endregion

        #region Collision

        public override BoundingRectangle UpdateBoundingRectangle()
        {
            BoundingRectangle = base.UpdateBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
            return BoundingRectangle;
        }

        public override bool CollidesWith(ComponentViewModel component)
        {
            return component.CollidesWith(Polygon) || base.CollidesWith(component);
        }

        public override bool CollidesWith(Point point)
        {
            return Polygon.HybridCollidesWith(point) || base.CollidesWith(point);
        }
        public override bool CollidesWith(PolyLine polyLine)
        {
            return Polygon.HybridCollidesWith(polyLine) || base.CollidesWith(polyLine);
        }
        public override bool CollidesWith(Ellipse ellipse)
        {
            return Polygon.HybridCollidesWith(ellipse) || base.CollidesWith(ellipse);
        }
        public override bool CollidesWith(Polygon polygon)
        {
            return Polygon.HybridCollidesWith(polygon) || base.CollidesWith(polygon);
        }

        public override bool CollidesWith(Vector2 point)
        {
            return Polygon.HybridCollidesWith(point) || base.CollidesWith(point);
        }

        #endregion
    }
}