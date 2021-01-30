using System.Drawing;
using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class Button : PolygonViewModel
    {
        #region Ctor

        public Button(Polygon polygon) : base( polygon)
        {
            OnMouseDown += Handle_OnMouseDown;
            OnMouseUp += Handle_OnMouseUp;
            OnMouseEnterEvent += Handle_OnMouseEnterEvent;
            OnMouseLeaveEvent += Handle_OnMouseLeaveEvent;
        }

        public static Button FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new( Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static Button FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new( Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Events

        private void Handle_OnMouseDown(object? sender, MouseDownEvent e)
        {
            BorderColor = Color.Green;
        }

        private void Handle_OnMouseUp(object? sender, MouseUpEvent e)
        {
            if (MouseOver)
            {
                BorderColor = Color.LightGreen;
            }
            else
            {
                BorderColor = Color.Black;
            }
        }

        private void Handle_OnMouseLeaveEvent(object? sender, MouseLeaveEvent e)
        {
            BorderColor = Color.Black;
        }

        private void Handle_OnMouseEnterEvent(object? sender, MouseEnterEvent e)
        {
            BorderColor = Color.LightGreen;
        }

        #endregion
    }
}