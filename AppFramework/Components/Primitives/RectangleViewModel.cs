using System.Numerics;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Primitives.Polygons;

namespace Aptacode.AppFramework.Components.Primitives
{
    public class RectangleViewModel : PolygonViewModel
    {
        #region Ctor

        public RectangleViewModel(Rectangle rectangle) : base(rectangle)
        {
        }

        #endregion

        #region Props

        public Rectangle Rectangle
        {
            get => (Rectangle) Polygon;
            set
            {
                Polygon = value;
                UpdateBoundingRectangle();
                Invalidated = true;
            }
        }

        #endregion

        #region CollisionDetection

        public override bool CollidesWith(ComponentViewModel component)
        {
            return CollisionDetectionEnabled && (component.CollidesWith(Polygon) || base.CollidesWith(component));
        }

        public override bool CollidesWith(Point point)
        {
            return CollisionDetectionEnabled && (Rectangle.HybridCollidesWith(point) || base.CollidesWith(point));
        }

        public override bool CollidesWith(PolyLine polyLine)
        {
            return CollisionDetectionEnabled && (Rectangle.HybridCollidesWith(polyLine) || base.CollidesWith(polyLine));
        }

        public override bool CollidesWith(Ellipse ellipse)
        {
            return CollisionDetectionEnabled && (Rectangle.HybridCollidesWith(ellipse) || base.CollidesWith(ellipse));
        }

        public override bool CollidesWith(Polygon polygon)
        {
            return CollisionDetectionEnabled && (Rectangle.HybridCollidesWith(polygon) || base.CollidesWith(polygon));
        }

        public override bool CollidesWith(Vector2 point)
        {
            return CollisionDetectionEnabled && (Rectangle.HybridCollidesWith(point) || base.CollidesWith(point));
        }

        #endregion
    }
}