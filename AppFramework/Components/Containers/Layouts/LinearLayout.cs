using System.Numerics;
using Aptacode.AppFramework.Enums;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Containers.Layouts
{
    public class LinearLayout : Layout
    {
        public override void Resize()
        {
            var position = Position;
            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    RepositionChild(child, new Vector2(Size.X, child.BoundingRectangle.Size.Y), position);
                    position += new Vector2(0, child.BoundingRectangle.Size.Y);
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    RepositionChild(child, new Vector2(child.BoundingRectangle.Size.X, Size.Y), position);
                    position += new Vector2(child.BoundingRectangle.Size.X, 0);
                }
            }
        }

        #region Ctor

        public LinearLayout(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) : base( Polygon.Rectangle.Create(topLeft, topRight, bottomRight, bottomLeft))
        {
        }

        public LinearLayout(Vector2 topLeft, Vector2 bottomRight) : base( Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight))
        {
        }

        #endregion

        #region Events

        #endregion

        #region Props

        private Orientation _orientation = Orientation.Vertical;

        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                Resize();
            }
        }

        #endregion
    }
}