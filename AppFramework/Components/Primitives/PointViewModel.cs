using System;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PointViewModel : ComponentViewModel
{
    #region Ctor

    public PointViewModel(Point point) : base(point)
    {
    }

    #endregion

    public Point Point => (Point)Primitive;

    #region Canvas

    public override void CustomDraw(BlazorCanvasInterop ctx)
    {
        ctx.BeginPath();
        ctx.Ellipse((int)Point.Position.X * SceneScale.Value, (int)Point.Position.Y * SceneScale.Value,
            1 * SceneScale.Value, 1 * SceneScale.Value, 0, 0, 2 * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }

    #endregion
}