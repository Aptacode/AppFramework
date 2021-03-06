﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.AppFramework.Components.Controls;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Utilities;
using Aptacode.BlazorCanvas;
using Aptacode.Geometry;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Containers
{
    public class ScrollBox : PolygonViewModel
    {
        #region Ctor

        public ScrollBox(Polygon polygon) : base(polygon)
        {
            ScrollBar = new ScrollBar(
                Polygon.Rectangle.FromPositionAndSize(
                    BoundingRectangle.TopRight - new Vector2(ScrollBarWidth, 0),
                    new Vector2(ScrollBarWidth, polygon.BoundingRectangle.Height)));

            ScrollBar.OnScroll += ScrollBarOnOnScroll;

            base.Add(ScrollBar);
        }

        public override async Task Draw(Scene.Scene scene, BlazorCanvasInterop ctx)
        {
            OldBoundingRectangle = BoundingRectangle;
            Invalidated = false;

            if (!IsShown)
            {
                return;
            }

            ctx.FillStyle(FillColorName);

            ctx.StrokeStyle(BorderColorName);

            ctx.LineWidth(BorderThickness * SceneScale.Value);

            await CustomDraw(ctx);

            if (_tempCanvasName == string.Empty)
            {
                _tempCanvasName = scene.Id + "temp";
                await ctx.CreateCanvas(_tempCanvasName, scene.Size.X * SceneScale.Value, scene.Size.Y * SceneScale.Value);
            }

            ctx.SelectCanvas(_tempCanvasName);
            ctx.Save();
            ctx.ClearRect(0, 0, scene.Size.X * SceneScale.Value, scene.Size.Y * SceneScale.Value);

            foreach (var componentViewModel in Children.Where(c => c.CollidesWith(this)))
            {
                await componentViewModel.Draw(scene, ctx);
            }

            ctx.GlobalCompositeOperation(CompositeOperation.DestinationIn);

            ctx.FillRect(BoundingRectangle.X * SceneScale.Value, BoundingRectangle.Y * SceneScale.Value, BoundingRectangle.Width * SceneScale.Value, BoundingRectangle.Height * SceneScale.Value);
            ctx.Restore();

            ctx.SelectCanvas(scene.Id.ToString());
            ctx.DrawCanvas(_tempCanvasName, 0, 0);
        }

        public override void Add(ComponentViewModel child)
        {
            ScrollChildren.Add(child);
            base.Add(child);
        }

        public override void Remove(ComponentViewModel child)
        {
            ScrollChildren.Remove(child);
            base.Remove(child);
        }

        private void ScrollBarOnOnScroll(object? sender, float e)
        {
            if (_lastScrollPosition > Constants.Tolerance)
            {
                var delta = new Vector2(0, (e - _lastScrollPosition) * BoundingRectangle.Height);
                foreach (var child in ScrollChildren)
                {
                    child.Translate(delta);
                }
            }

            _lastScrollPosition = e;
        }

        public static ScrollBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static ScrollBox FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Props

        public ScrollBar ScrollBar { get; set; }
        public List<ComponentViewModel> ScrollChildren { get; set; } = new();
        public float ScrollBarWidth { get; set; } = 5;
        private string _tempCanvasName = string.Empty;
        private float _lastScrollPosition;

        #endregion
    }
}