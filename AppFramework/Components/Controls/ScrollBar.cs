using System;
using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class ScrollBar : PolygonViewModel
    {
        #region Events

        public event EventHandler<float> OnScroll;

        #endregion

        #region Ctor

        public ScrollBar(Polygon polygon) : base(polygon)
        {
            ScrollButton = new Button(
                Polygon.Rectangle.FromPositionAndSize(polygon.BoundingRectangle.TopLeft, new Vector2(polygon.BoundingRectangle.Width, 5)));
            Add(ScrollButton);

            OnMouseMoveTunneled += Handle_OnMouseMove;
            ScrollButton.OnMouseDown += ScrollButtonOnOnMouseDown;
            OnMouseUpTunneled += ScrollButtonOnOnMouseUp;
            OnMouseLeaveEvent += ScrollButtonOnOnMouseLeaveEvent;
        }

        private void Handle_OnMouseMove(object? sender, MouseMoveEvent e)
        {
            if (IsScrolling)
            {
                var delta = new Vector2(0, e.Position.Y - LastMousePosition.Y);
                ScrollButton.Translate(delta);
                if (ScrollButton.BoundingRectangle.Y < BoundingRectangle.Y ||
                    ScrollButton.BoundingRectangle.BottomRight.Y > BoundingRectangle.BottomRight.Y)
                {
                    ScrollButton.Translate(-delta);
                }
                else
                {
                    OnScroll?.Invoke(this, ScrollButton.BoundingRectangle.Center.Y / (BoundingRectangle.Height - ScrollButton.BoundingRectangle.Height));
                }
            }

            LastMousePosition = e.Position;
        }

        private void ScrollButtonOnOnMouseLeaveEvent(object? sender, MouseLeaveEvent e)
        {
            IsScrolling = false;
        }

        private void ScrollButtonOnOnMouseUp(object? sender, MouseUpEvent e)
        {
            IsScrolling = false;
        }

        private void ScrollButtonOnOnMouseDown(object? sender, MouseDownEvent e)
        {
            IsScrolling = true;
            LastMousePosition = e.Position;
        }

        public static ScrollBar FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static ScrollBar FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Props

        public Button ScrollButton { get; set; }
        public bool IsScrolling { get; protected set; }
        public Vector2 LastMousePosition { get; set; }

        #endregion
    }
}