using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives
{
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

        public override void Translate(Vector2 delta)
        {
            Primitive.Translate(delta);
            base.Translate(delta);
        }

        public override void Scale(Vector2 delta)
        {
            Primitive.ScaleAboutTopLeft(delta);
            base.Scale(delta);
        }

        public override void Rotate(float theta)
        {
            Primitive.Rotate(theta);
            base.Rotate(theta);
        }

        public override void Rotate(Vector2 rotationCenter, float theta)
        {
            Primitive.Rotate(rotationCenter, theta);
            base.Rotate(rotationCenter, theta);
        }

        public override void Skew(Vector2 delta)
        {
            Primitive.Skew(delta);
            base.Skew(delta);
        }

        #endregion

        #region Collision
        
        public override bool CollidesWith(Point point)
        {
            return CollisionDetectionEnabled && (Primitive.HybridCollidesWith(point) || base.CollidesWith(point));
        }

        public override bool CollidesWith(PolyLine polyLine)
        {
            return CollisionDetectionEnabled && (Primitive.HybridCollidesWith(polyLine) || base.CollidesWith(polyLine));
        }

        public override bool CollidesWith(Ellipse ellipse)
        {
            return CollisionDetectionEnabled && (Primitive.HybridCollidesWith(ellipse) || base.CollidesWith(ellipse));
        }

        public override bool CollidesWith(Polygon polygon)
        {
            return CollisionDetectionEnabled && (Primitive.HybridCollidesWith(polygon) || base.CollidesWith(polygon));
        }

        public override bool CollidesWith(Vector2 point)
        {
            return CollisionDetectionEnabled && (Primitive.HybridCollidesWith(point) || base.CollidesWith(point));
        }

        #endregion
    }
}