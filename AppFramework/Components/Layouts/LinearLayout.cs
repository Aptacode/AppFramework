using System.Numerics;
using Aptacode.AppFramework.Enums;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Layouts
{
    public class LinearLayout : Layout
    {
        #region Ctor

        public LinearLayout(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) : base(Polygon.Rectangle.Create(topLeft, topRight, bottomRight, bottomLeft))
        {
            
        }

        public LinearLayout(Vector2 topLeft, Vector2 bottomRight) : base(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight))
        {
            
        }

        #endregion
        
        public override void Resize()
        {
            var position = Position;
            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    var scale = Size.X / child.BoundingRectangle.Size.X;
                    child.Scale(new Vector2(scale, 1));
                    var delta = position - child.BoundingRectangle.TopLeft;
                    child.Translate(delta);
                    position += new Vector2(0, child.BoundingRectangle.Size.Y);
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    var scale = Size.Y / child.BoundingRectangle.Size.Y;
                    child.Scale(new Vector2(1, scale));
                    var delta = position - child.BoundingRectangle.TopLeft;
                    child.Translate(delta);
                    position += new Vector2(child.BoundingRectangle.Size.X, 0);
                }
            }
        }

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