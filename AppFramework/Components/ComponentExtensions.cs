using Aptacode.AppFramework.Components.Primitives;
using Aptacode.Geometry.Primitives;
using System.Numerics;

namespace Aptacode.AppFramework.Components;

public static class ComponentExtensions
{
    public static PolygonComponent ToComponent(this Polygon polygon)
    {
        return new PolygonComponent(polygon);
    }

    public static PolylineComponent ToComponent(this PolyLine polyline)
    {
        return new PolylineComponent(polyline);
    }

    public static void Center(this Primitive p)
    {
        var center = p.GetCentroid();
        p.Translate(-center);
    }
}