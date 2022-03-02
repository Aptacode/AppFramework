using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolygonViewModel : PrimitiveViewModel<Polygon>
{
    #region Canvas

    public PolygonViewModel(Polygon polygon) : base(polygon)
    {
    }

    #endregion

    #region Ctor

    public override async Task CustomDraw(BlazorCanvasInterop ctx)
    {
        var vertices = new Vector2[Primitive.Vertices.Length];
        for (var i = 0; i < Primitive.Vertices.Length; i++) vertices[i] = Primitive.Vertices[i] * SceneScale.Value;

        ctx.Polygon(vertices);

        ctx.Fill();
        ctx.Stroke();
    }

    #endregion

    #region Collision

    public override void UpdateBounds()
    {
        if (Primitive == null) Primitive = Polygon.Create(0, 0, 0, 0, 0, 0);

        if (Margin > Constants.Tolerance)
            BoundingPrimitive = Primitive.GetBoundingPrimitive(Margin);
        else
            BoundingPrimitive = PolyLine.Create(Primitive.Vertices.Vertices.ToArray());

        BoundingRectangle = GetChildrenBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
    }

    public override bool CollidesWith(ComponentViewModel component)
    {
        return (component.CollidesWith(Primitive) || base.CollidesWith(component));
    }

    #endregion
}