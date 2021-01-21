using System.Drawing;
using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;

namespace Aptacode.AppFramework.Components.Controls
{
    public class Button : PolygonViewModel
    {
        #region Ctor

        public Button(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) : base(Geometry.Primitives.Polygon.Rectangle.Create(topLeft, topRight, bottomRight, bottomLeft))
        {
            OnMouseDown += Handle_OnMouseDown;
            OnMouseUp += Handle_OnMouseUp;
        }

        public Button(Vector2 topLeft, Vector2 bottomRight) : base(Geometry.Primitives.Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight))
        {
            OnMouseDown += Handle_OnMouseDown;
            OnMouseUp += Handle_OnMouseUp;
        }

        #endregion

        #region Events

        private void Handle_OnMouseDown(object? sender, MouseDownEvent e)
        {
            BorderColor = Color.Green;
        }

        private void Handle_OnMouseUp(object? sender, MouseUpEvent e)
        {
            BorderColor = Color.Black;
        }

        #endregion

        #region Props

        #endregion
    }
}