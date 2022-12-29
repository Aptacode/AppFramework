using Aptacode.Geometry.Primitives;
using System.Numerics;

namespace Aptacode.AppFramework.Components.Primitives;

public abstract class PrimitiveComponent : Component
{
    public bool CollidesWith(Vector2 point)
    {
        return TransformedPrimitive.CollidesWith(point);
    }
    public bool CollidesWith(Primitive primitive)
    {
        return TransformedPrimitive.CollidesWithPrimitive(primitive);
    }
    public bool CollidesWith(PrimitiveComponent primitive)
    {
        return TransformedPrimitive.CollidesWithPrimitive(primitive.TransformedPrimitive);
    }
    public abstract Primitive TransformedPrimitive { get; }
}