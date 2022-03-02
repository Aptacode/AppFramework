using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives;

public abstract class PrimitiveViewModel<TPrimitive> : ComponentViewModel where TPrimitive : Primitive
{
    #region Ctor

    protected PrimitiveViewModel(TPrimitive primitive)
    {
        Primitive = primitive;
    }

    #endregion

    #region Props

    private TPrimitive _primitive;

    public TPrimitive Primitive
    {
        get => _primitive;
        set
        {
            _primitive = value;
            UpdateBounds();
            Invalidated = true;
        }
    }

    #endregion

    #region Transformations

    public override void Translate(Scene.Scene scene, Vector2 delta, bool source)
    {
        Primitive.Translate(delta);
        base.Translate(scene, delta, source);
    }

    public override void ScaleAboutCenter(Scene.Scene scene, Vector2 delta)
    {
        Primitive.ScaleAboutCenter(delta);
        base.ScaleAboutCenter(scene, delta);
    }

    public override void ScaleAboutTopLeft(Scene.Scene scene, Vector2 delta)
    {
        Primitive.Scale(delta, BoundingRectangle.TopLeft);
        base.ScaleAboutCenter(scene, delta);
    }

    public override void Scale(Scene.Scene scene, Vector2 scaleCenter, Vector2 delta)
    {
        Primitive.Scale(scaleCenter, delta);
        base.ScaleAboutCenter(scene, delta);
    }

    public override void SetPosition(Scene.Scene scene, Vector2 position, bool source)
    {
        Primitive.SetPosition(position);
        base.SetPosition(scene, position, source);
    }

    public override void SetSize(Scene.Scene scene, Vector2 size)
    {
        Primitive.SetSize(size);
        base.SetSize(scene, size);
    }

    public override void Rotate(Scene.Scene scene, float theta)
    {
        Primitive.Rotate(theta);
        base.Rotate(scene, theta);
    }

    public override void Rotate(Scene.Scene scene, Vector2 rotationCenter, float theta)
    {
        Primitive.Rotate(rotationCenter, theta);
        base.Rotate(scene, rotationCenter, theta);
    }

    public override void Skew(Scene.Scene scene, Vector2 delta)
    {
        Primitive.Skew(delta);
        base.Skew(scene, delta);
    }

    #endregion

    #region Collision

    public override bool CollidesWith(Vector2 point)
    {
        return (Primitive.CollidesWith(point) || base.CollidesWith(point));
    }
    public override bool CollidesWith(Primitive primitive)
    {
        return (primitive.CollidesWithPrimitive(Primitive) || base.CollidesWith(primitive));
    }
    public override bool CollidesWith(ComponentViewModel component)
    {
        return (component.CollidesWith(Primitive) || base.CollidesWith(component));
    }

    #endregion
}