using System.Numerics;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolylineComponent : Component
{
    #region Ctor

    public PolylineComponent(PolyLine polyLine) : base(polyLine)
    {
    }

    #endregion

    public PolyLine PolyLine => (PolyLine)Primitive;

    #region Canvas

    public override void CustomDraw(BlazorCanvasInterop ctx)
    {
        var vertices = new Vector2[PolyLine.Vertices.Length];
        for (var i = 0; i < PolyLine.Vertices.Length; i++) vertices[i] = PolyLine.Vertices[i] * SceneScale.Value;

        ctx.PolyLine(vertices);
        ctx.Stroke();
    }

    #endregion
}