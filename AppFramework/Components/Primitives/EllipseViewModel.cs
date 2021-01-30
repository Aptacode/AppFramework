﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.AppFramework.Extensions;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;

namespace Aptacode.AppFramework.Components.Primitives
{
    public class EllipseViewModel : PrimitiveViewModel<Ellipse>
    {
        #region Ctor

        public EllipseViewModel(ComponentViewModel parent, Ellipse ellipse) : base(parent, ellipse)
        {
        }

        #endregion

        #region Canvase

        public override async Task CustomDraw(BlazorCanvasInterop ctx)
        {
            ctx.BeginPath();

            ctx.Ellipse((int) Primitive.Position.X * SceneScale.Value, (int) Primitive.Position.Y * SceneScale.Value, (int) Primitive.Radii.X * SceneScale.Value,
                (int) Primitive.Radii.Y * SceneScale.Value, Primitive.Rotation, 0, 2.0f * (float) Math.PI);
            ctx.Fill();
            ctx.Stroke();
        }

        #endregion

        #region Collision

        public override void UpdateBounds()
        {
            if (Primitive == null)
            {
                Primitive = Ellipse.Zero;
            }

            BoundingRectangle = GetChildrenBoundingRectangle().Combine(Primitive.BoundingRectangle);

            if (Margin > Constants.Tolerance)
            {
                BoundingPrimitive = Polygon.Create(Primitive.Vertices.ToConvexHull(Margin).Vertices);
            }
            else
            {
                BoundingPrimitive = PolyLine.Create(Primitive.Vertices.Vertices.ToArray());
            }
        }

        public override bool CollidesWith(ComponentViewModel component)
        {
            return CollisionDetectionEnabled && (component.CollidesWith(Primitive) || base.CollidesWith(component));
        }

        #endregion
    }
}