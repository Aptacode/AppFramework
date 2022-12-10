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

    public override void CustomDraw(BlazorCanvas.BlazorCanvas ctx)
    {
        var vertices = new double[Polygon.Vertices.Length * 2];
        var vIndex = 0;
        for (var i = 0; i < Polygon.Vertices.Length; i++)
        {
            var v = Polygon.Vertices[i];
            vertices[vIndex++] = v.X;
            vertices[vIndex++] = v.Y;
        }

        ctx.Polygon(vertices);

        ctx.Fill();
        ctx.Stroke();
    }

    #endregion

    #region Methods

    public void Update(Polygon primitive)
    {
        Primitive = primitive;
    }

    #endregion
}