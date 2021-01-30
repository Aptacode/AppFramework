using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives
{
    public static class PrimitiveViewModelExtensions
    {
        public static EllipseViewModel ToViewModel(this Ellipse ellipse, ComponentViewModel parent)
        {
            return new(parent, ellipse);
        }

        public static PolygonViewModel ToViewModel(this Polygon polygon, ComponentViewModel parent)
        {
            return new(parent, polygon);
        }

        public static PolylineViewModel ToViewModel(this PolyLine polyline, ComponentViewModel parent)
        {
            return new(parent, polyline);
        }

        public static PointViewModel ToViewModel(this Point point, ComponentViewModel parent)
        {
            return new(parent, point);
        }
    }
}