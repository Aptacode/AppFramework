using System;
using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public class CircleComponent : PrimitiveComponent
{
    public CircleComponent(Vector2 position, float radius)
    {
        Circle = new Circle(Vector2.Zero, radius);
        transformedPrimitive = Circle.Copy();
        TranslationMatrix = Matrix3x2.CreateTranslation(position);
    }

    public Circle Circle { get; init; }
    private readonly Circle transformedPrimitive;
    public override Circle TransformedPrimitive
    {
        get
        {
            var t = TransformedMatrix;
            if (transformedMatrixChanged)
            {
                transformedMatrixChanged = false;
                Circle.CopyAndTransformTo(transformedPrimitive, t);
            }

            return transformedPrimitive;
        }
    }

    public override void Render(BlazorCanvas.BlazorCanvas ctx)
    {
        var primitive = TransformedPrimitive;

        ctx.BeginPath();

        ctx.Ellipse((int)primitive.Position.X, (int)primitive.Position.Y,
            (int)primitive.Radius,
            (int)primitive.Radius, 0, 0, 2.0f * (float)Math.PI);
        ctx.Fill();
        ctx.Stroke();
    }
}