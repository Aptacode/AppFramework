using System.Numerics;
using Aptacode.AppFramework.Components.Primitives;
using Aptacode.AppFramework.Scene.Events;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Controls
{
    public class CheckBox : PolygonViewModel
    {
        #region Events

        private void CheckButtonOnOnMouseClickTunneled(object? sender, MouseClickEvent e)
        {
            IsChecked = !IsChecked;
        }

        #endregion

        #region Ctor

        public CheckBox(Polygon polygon) : base(polygon)
        {
            CheckButton = new Button(Polygon.Rectangle.FromPositionAndSize(Vector2.Zero, new Vector2(2, 2)));
            Check = new PolylineViewModel(PolyLine.Create(0, 0, 1, 1, 2, 0, 1, 1, 0, 2, 1, 1, 2, 2))
            {
                Margin = 0.1f
            };

            CheckButton.Add(Check);

            CheckButton.SetPosition(BoundingRectangle.TopLeft);
            CheckButton.SetSize(new Vector2(BoundingRectangle.Height, BoundingRectangle.Height));
            Add(CheckButton);

            IsChecked = false;

            OnMouseClickTunneled += CheckButtonOnOnMouseClickTunneled;
        }

        public static CheckBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new(Polygon.Rectangle.FromPositionAndSize(position, size));
        }

        public static CheckBox FromTwoPoints(Vector2 topLeft, Vector2 bottomRight)
        {
            return new(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight));
        }

        #endregion

        #region Properties

        public Button CheckButton { get; set; }
        public PolylineViewModel Check { get; set; }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                Check.IsShown = _isChecked;
            }
        }

        #endregion
    }
}