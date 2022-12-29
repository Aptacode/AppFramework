using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PolygonComponent : PrimitiveComponent
{
    public PolygonComponent(Polygon polygon)
    {
        Polygon = polygon;
        transformedPrimitive = Polygon.Copy();
    }

    public Polygon Polygon { get; init; }
    private readonly Polygon transformedPrimitive;
    public override Polygon TransformedPrimitive
    {
        get
        {
            var t = TransformedMatrix;
            if (transformedMatrixChanged)
            {
                transformedMatrixChanged = false;
                Polygon.CopyAndTransformTo(transformedPrimitive, t);
            }
            return transformedPrimitive;
        }
    }

    public override void Render(BlazorCanvas.BlazorCanvas ctx)
    {
        var primitive = TransformedPrimitive;

        ctx.FillStyle("green");
        var vertices = new double[primitive.Vertices.Length * 2];
        var vIndex = 0;
        for (var i = 0; i < primitive.Vertices.Length; i++)
        {
            var v = primitive.Vertices[i];
            vertices[vIndex++] = v.X;
            vertices[vIndex++] = v.Y;
        }

        ctx.Polygon(vertices);

        ctx.Fill();
        ctx.Stroke();
    }
}