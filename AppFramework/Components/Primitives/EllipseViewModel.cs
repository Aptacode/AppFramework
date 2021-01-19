﻿using System;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry.Collision.Rectangles;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Primitives
{
    public class EllipseViewModel : ComponentViewModel
    {
        #region Ctor

        public EllipseViewModel(Ellipse ellipse)
        {
            Ellipse = ellipse;
            OldBoundingRectangle = BoundingRectangle =
                Children.ToBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
        }

        #endregion

        #region Canvase

        public override async Task CustomDraw(BlazorCanvasInterop ctx)
        {
            ctx.BeginPath();

            ctx.Ellipse((int) Ellipse.Position.X * SceneScale.Value, (int) Ellipse.Position.Y * SceneScale.Value, (int) Ellipse.Radii.X * SceneScale.Value,
                (int) Ellipse.Radii.Y * SceneScale.Value, Ellipse.Rotation, 0, 2.0f * (float) Math.PI);
            ctx.Fill();
            ctx.Stroke();
        }

        #endregion

        #region Props

        private Ellipse _ellipse;

        public Ellipse Ellipse
        {
            get => _ellipse;
            set
            {
                _ellipse = value;
                UpdateMargin();
                Invalidated = true;
            }
        }

        public override void UpdateMargin()
        {
            if (_ellipse == null)
            {
                return;
            }

            BoundingPrimitive = new Ellipse(_ellipse.Position, _ellipse.Radii + new Vector2(Margin, Margin),
                _ellipse.Rotation);
        }

        #endregion


        #region Transformations

        public override void Translate(Vector2 delta)
        {
            Ellipse.Translate(delta);
            BoundingPrimitive.Translate(delta);
            base.Translate(delta);
        }

        public override void Scale(Vector2 delta)
        {
            Ellipse.Scale(delta);
            BoundingPrimitive.Scale(delta);
            base.Scale(delta);
        }

        public override void Rotate(float theta)
        {
            Ellipse.Rotate(theta);
            BoundingPrimitive.Rotate(theta);
            base.Rotate(theta);
        }

        public override void Rotate(Vector2 rotationCenter, float theta)
        {
            Ellipse.Rotate(rotationCenter, theta);
            BoundingPrimitive.Rotate(rotationCenter, theta);
            base.Rotate(rotationCenter, theta);
        }

        public override void Skew(Vector2 delta)
        {
            Ellipse.Skew(delta);
            BoundingPrimitive.Skew(delta);
            base.Skew(delta);
        }

        #endregion

        #region Collision

        public override BoundingRectangle UpdateBoundingRectangle()
        {
            BoundingRectangle = base.UpdateBoundingRectangle().Combine(BoundingPrimitive.BoundingRectangle);
            return BoundingRectangle;
        }

        public override bool CollidesWith(ComponentViewModel component)
        {
            return CollisionDetectionEnabled && (component.CollidesWith(Ellipse) || base.CollidesWith(component));
        }

        public override bool CollidesWith(Point point)
        {
            return CollisionDetectionEnabled && (Ellipse.HybridCollidesWith(point) || base.CollidesWith(point));
        }
        public override bool CollidesWith(PolyLine polyLine)
        {
            return CollisionDetectionEnabled && (Ellipse.HybridCollidesWith(polyLine) || base.CollidesWith(polyLine));
        }
        public override bool CollidesWith(Ellipse ellipse)
        {
            return CollisionDetectionEnabled && (Ellipse.HybridCollidesWith(ellipse) || base.CollidesWith(ellipse));
        }
        public override bool CollidesWith(Polygon polygon)
        {
            return CollisionDetectionEnabled && (Ellipse.HybridCollidesWith(polygon) || base.CollidesWith(polygon));
        }

        public override bool CollidesWith(Vector2 point)
        {
            return CollisionDetectionEnabled && (Ellipse.HybridCollidesWith(point) || base.CollidesWith(point));
        }

        #endregion
    }
}