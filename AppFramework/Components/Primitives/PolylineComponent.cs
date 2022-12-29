using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolylineComponent : PrimitiveComponent
{
    public PolylineComponent(PolyLine polyLine)
    {
        PolyLine = polyLine;
        transformedPrimitive = PolyLine.Copy();
    }

    public PolyLine PolyLine { get; init; }
    private readonly PolyLine transformedPrimitive;

    public override PolyLine TransformedPrimitive
    {
        get
        {
            if (transformedMatrixChanged)
            {
                transformedMatrixChanged = false;
                PolyLine.CopyAndTransformTo(PolyLine, TransformedMatrix);
            }
            return transformedPrimitive;
        }
    }
    public override void Render(BlazorCanvas.BlazorCanvas ctx)
    {
        var primitive = TransformedPrimitive;
        var vertices = new double[primitive.Vertices.Length * 2];
        var vIndex = 0;
        for (var i = 0; i < primitive.Vertices.Length; i++)
        {
            var v = primitive.Vertices[i];
            vertices[vIndex++] = v.X;
            vertices[vIndex++] = v.Y;
        }

        ctx.PolyLine(vertices);
        ctx.Stroke();
    }
}