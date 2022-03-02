using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolylineViewModel : PrimitiveViewModel<PolyLine>
{
    #region Ctor

    public PolylineViewModel(PolyLine polyLine) : base(polyLine)
    {
    }

    #endregion


    #region Canvas

    public override async Task CustomDraw(BlazorCanvasInterop ctx)
    {
        var vertices = new Vector2[Primitive.Vertices.Length];
        for (var i = 0; i < Primitive.Vertices.Length; i++) vertices[i] = Primitive.Vertices[i] * SceneScale.Value;

        ctx.PolyLine(vertices);
        ctx.Stroke();
    }

    public override void UpdateBounds()
    {
        if (Primitive == null) Primitive = PolyLine.Zero;

        BoundingRectangle = GetChildrenBoundingRectangle().Combine(Primitive.BoundingRectangle);

        if (Margin > Constants.Tolerance)
            BoundingPrimitive = Polygon.Create(Primitive.Vertices.ToConvexHull(Margin).Vertices);
        else
            BoundingPrimitive = PolyLine.Create(Primitive.Vertices.Vertices.ToArray());
    }

    #endregion
}