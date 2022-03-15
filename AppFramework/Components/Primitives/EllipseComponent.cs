using System;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class EllipseComponent : Component
{
    #region Ctor

    public EllipseComponent(Ellipse ellipse) : base(ellipse)
    {
    }

    #endregion

    public Ellipse Ellipse => (Ellipse)Primitive;

    #region Canvase

    public override void CustomDraw(BlazorCanvasInterop ctx)
    {
        ctx.BeginPath();

        ctx.Ellipse((int)Ellipse.Position.X, (int)Ellipse.Position.Y,
            (int)Ellipse.Radii.X,
            (int)Ellipse.Radii.Y, Ellipse.Rotation, 0, 2.0f * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }

    #endregion

    public void Update(Ellipse ellipse)
    {
        Primitive = ellipse;

    }
}