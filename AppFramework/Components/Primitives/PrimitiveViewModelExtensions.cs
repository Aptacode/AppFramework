using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public static class PrimitiveViewModelExtensions
{
    public static EllipseViewModel ToViewModel(this Ellipse ellipse)
    {
        return new EllipseViewModel(ellipse);
    }

    public static PolygonViewModel ToViewModel(this Polygon polygon)
    {
        return new PolygonViewModel(polygon);
    }

    public static PolylineViewModel ToViewModel(this PolyLine polyline)
    {
        return new PolylineViewModel(polyline);
    }

    public static PointViewModel ToViewModel(this Point point)
    {
        return new PointViewModel(point);
    }
}