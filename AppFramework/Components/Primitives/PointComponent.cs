﻿using System;
using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class PointComponent : PrimitiveComponent
{
    public PointComponent(Vector2 position)
    {
        Point = new Point(Vector2.Zero);
        transformedPrimitive = Point.Copy();
        TranslationMatrix = Matrix3x2.CreateTranslation(position);
    }

    public Point Point { get; init; }
    private readonly Point transformedPrimitive;
    public override Point TransformedPrimitive
    {
        get
        {
            var t = TransformedMatrix;
            if (transformedMatrixChanged)
            {
                transformedMatrixChanged = false;
                Point.CopyAndTransformTo(transformedPrimitive, t);
            }
            return transformedPrimitive;
        }
    }
    public override void Render(BlazorCanvas.BlazorCanvas ctx)
    {
        var primitive = TransformedPrimitive;

        ctx.BeginPath();
        ctx.Ellipse((int)primitive.Position.X, (int)primitive.Position.Y,
            1, 1, 0, 0, 2 * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }
}