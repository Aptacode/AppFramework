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

        ctx.Ellipse((int)Ellipse.Position.X * SceneScale.Value, (int)Ellipse.Position.Y * SceneScale.Value,
            (int)Ellipse.Radii.X * SceneScale.Value,
            (int)Ellipse.Radii.Y * SceneScale.Value, Ellipse.Rotation, 0, 2.0f * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }

    #endregion
}