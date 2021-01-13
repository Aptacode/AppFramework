using System.Drawing;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Rectangle = Aptacode.Geometry.Primitives.Polygons.Rectangle;

namespace Aptacode.AppFramework.Components.Controls
{
    public class ButtonViewModel : RectangleViewModel
    {
        #region Ctor

        public ButtonViewModel(Rectangle rectangle) : base(rectangle)
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