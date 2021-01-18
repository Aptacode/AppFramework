using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components.Primitives
{
    public class PolylineViewModel : ComponentViewModel
    {
        #region Ctor

        public PolylineViewModel(PolyLine polyLine)
        {
            PolyLine = polyLine;
            OldBoundingRectangle = BoundingRectangle =
                Children.ToBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
        }

        #endregion

        #region Canvas

        public override async Task CustomDraw(BlazorCanvasInterop ctx)
        {
            var vertices = new Vector2[_polyLine.Vertices.Length];
            for (var i = 0; i < _polyLine.Vertices.Length; i++)
            {
                vertices[i] = _polyLine.Vertices[i] * SceneScale.Value;
            }

            ctx.PolyLine(vertices);
            ctx.Stroke();
        }

        #endregion

        #region Props

        private PolyLine _polyLine;

        public PolyLine PolyLine
        {
            get => _polyLine;
            set
            {
                _polyLine = value;
                UpdateMargin();
                Invalidated = true;
            }
        }

        public override void UpdateMargin()
        {
            if (_polyLine == null)
            {
                return;
            }

            if (Margin > Constants.Tolerance)
            {
                BoundingPrimitive = new Polygon(_polyLine.Vertices.ToConvexHull(Margin));
            }
            else
            {
                BoundingPrimitive = PolyLine.Create(_polyLine.Vertices.Vertices.ToArray());
            }
        }

        #endregion

        #region Transformations

        public override void Translate(Vector2 delta)
        {
            PolyLine.Translate(delta);
            BoundingPrimitive.Translate(delta);
            UpdateMargin();

            base.Translate(delta);
        }

        public override void Scale(Vector2 delta)
        {
            PolyLine.Scale(delta);
            UpdateMargin();

            base.Scale(delta);
        }

        public override void Rotate(float theta)
        {
            PolyLine.Rotate(theta);
            UpdateMargin();

            base.Rotate(theta);
        }

        public override void Rotate(Vector2 rotationCenter, float theta)
        {
            PolyLine.Rotate(rotationCenter, theta);
            UpdateMargin();

            base.Rotate(rotationCenter, theta);
        }

        public override void Skew(Vector2 delta)
        {
            PolyLine.Skew(delta);
            UpdateMargin();

            base.Skew(delta);
        }

        #endregion

        #region Collision

        public override bool CollidesWith(ComponentViewModel component)
        {
            return component.CollidesWith(PolyLine) || base.CollidesWith(component);
        }

        public override bool CollidesWith(Point point)
        {
            return PolyLine.HybridCollidesWith(point) || base.CollidesWith(point);
        }
        public override bool CollidesWith(PolyLine polyLine)
        {
            return PolyLine.HybridCollidesWith(polyLine) || base.CollidesWith(polyLine);
        }
        public override bool CollidesWith(Ellipse ellipse)
        {
            return PolyLine.HybridCollidesWith(ellipse) || base.CollidesWith(ellipse);
        }
        public override bool CollidesWith(Polygon polygon)
        {
            return PolyLine.HybridCollidesWith(polygon) || base.CollidesWith(polygon);
        }

        public override bool CollidesWith(Vector2 point)
        {
            return PolyLine.HybridCollidesWith(point) || base.CollidesWith(point);
        }

        #endregion
    }
}