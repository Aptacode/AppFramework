using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public static class ComponentExtensions
{
    public static EllipseComponent ToComponent(this Ellipse ellipse)
    {
        return new EllipseComponent(ellipse);
    }

    public static PolygonComponent ToComponent(this Polygon polygon)
    {
        return new PolygonComponent(polygon);
    }

    public static PolylineComponent ToComponent(this PolyLine polyline)
    {
        return new PolylineComponent(polyline);
    }

    public static PointComponent ToComponent(this Point point)
    {
        return new PointComponent(point);
    }
}