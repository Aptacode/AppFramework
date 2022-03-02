using System;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components.Primitives;

public class PointViewModel : PrimitiveViewModel<Point>
{
    #region Ctor

    public PointViewModel(Point point) : base(point)
    {
    }

    #endregion

    #region Canvas

    public override async Task CustomDraw(BlazorCanvasInterop ctx)
    {
        ctx.BeginPath();
        ctx.Ellipse((int)Primitive.Position.X * SceneScale.Value, (int)Primitive.Position.Y * SceneScale.Value,
            1 * SceneScale.Value, 1 * SceneScale.Value, 0, 0, 2 * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }

    #endregion

    #region Collision

    public override void UpdateBounds()
    {
        if (Primitive == null) Primitive = Point.Zero;

        BoundingRectangle = GetChildrenBoundingRectangle().Combine(Primitive.BoundingRectangle);

        if (Margin > Constants.Tolerance)
            BoundingPrimitive = Polygon.Create(Primitive.Vertices.ToConvexHull(Margin).Vertices);
        else
            BoundingPrimitive = PolyLine.Create(Primitive.Vertices.Vertices.ToArray());
    }

    #endregion
}