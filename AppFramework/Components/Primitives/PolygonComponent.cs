using System.Numerics;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolygonComponent : Component
{
    #region Canvas

    public PolygonComponent(Polygon polygon) : base(polygon)
    {
    }

    #endregion

    public Polygon Polygon => (Polygon)Primitive;

    #region Ctor

    public override void CustomDraw(BlazorCanvasInterop ctx)
    {
        var vertices = new Vector2[Polygon.Vertices.Length];
        for (var i = 0; i < Polygon.Vertices.Length; i++) vertices[i] = Polygon.Vertices[i] * SceneScale.Value;

        ctx.Polygon(vertices);

        ctx.Fill();
        ctx.Stroke();
    }

    #endregion
}