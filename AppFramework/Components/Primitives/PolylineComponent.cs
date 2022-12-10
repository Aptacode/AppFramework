using System.Numerics;
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

    public override void CustomDraw(BlazorCanvas.BlazorCanvas ctx)
    {
        var vertices = new double[PolyLine.Vertices.Length * 2];
        var vIndex = 0;
        for (var i = 0; i < PolyLine.Vertices.Length; i++)
        {
            var v = PolyLine.Vertices[i];
            vertices[vIndex++] = v.X;
            vertices[vIndex++] = v.Y;
        }

        ctx.PolyLine(vertices);
        ctx.Stroke();
    }

    #endregion

    #region Methods

    public void Update(PolyLine primitive)
    {
        Primitive = primitive;
    }

    #endregion
}