using System;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PointComponent : Component
{
    #region Ctor

    public PointComponent(Point point) : base(point)
    {
    }

    #endregion

    public Point Point => (Point)Primitive;

    #region Canvas

    public override void CustomDraw(BlazorCanvasInterop ctx)
    {
        ctx.BeginPath();
        ctx.Ellipse((int)Point.Position.X, (int)Point.Position.Y,
            1, 1, 0, 0, 2 * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }

    #endregion

    #region Methods

    public void Update(Point primitive)
    {
        Primitive = primitive;
    }

    #endregion
}