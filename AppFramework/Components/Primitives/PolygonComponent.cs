using System.Numerics;
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

    public override void CustomDraw(BlazorCanvas.BlazorCanvas ctx)
    {
        var vertices = new Vector2[Polygon.Vertices.Length];
        for (var i = 0; i < Polygon.Vertices.Length; i++)
        {
            vertices[i] = Polygon.Vertices[i];
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