using System.Numerics;
using Aptacode.Geometry.Collision.Rectangles;

namespace Aptacode.AppFramework.Extensions;

public static class ComponentExtensions
{
    public static BoundingRectangle Combine(this BoundingRectangle rectangle1, BoundingRectangle rectangle2)
    {
        var minX = rectangle1.TopLeft.X < rectangle2.TopLeft.X ? rectangle1.TopLeft.X : rectangle2.TopLeft.X;
        var minY = rectangle1.TopLeft.Y < rectangle2.TopLeft.Y ? rectangle1.TopLeft.Y : rectangle2.TopLeft.Y;
        var maxX = rectangle1.BottomRight.X > rectangle2.BottomRight.X
            ? rectangle1.BottomRight.X
            : rectangle2.BottomRight.X;
        var maxY = rectangle1.BottomRight.Y > rectangle2.BottomRight.Y
            ? rectangle1.BottomRight.Y
            : rectangle2.BottomRight.Y;

        return new BoundingRectangle(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }
}