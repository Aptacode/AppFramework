using System.Linq;
using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Layouts
{
    public class GridLayout : Layout
    {
        #region Overrides

        public override void Resize()
        {
            var childCount = Children.Count();
            var childIndex = 0;

            var cellSize = new Vector2(Size.X / _columns, Size.Y / _rows);
            var cellPosition = Position;
            for (var j = 0; j < _rows; j++)
            {
                cellPosition = new Vector2(Position.X, cellPosition.Y);

                for (var k = 0; k < _columns; k++)
                {
                    if (childIndex < childCount)
                    {
                        var child = Children.ElementAt(childIndex++);
                        RepositionChild(child, cellSize, cellPosition);
                    }
                    else
                    {
                        return;
                    }

                    cellPosition += new Vector2(cellSize.X, 0);
                }

                cellPosition += new Vector2(0, cellSize.Y);
            }
        }

        #endregion

        #region Ctor

        public GridLayout(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) : base(Polygon.Rectangle.Create(topLeft, topRight, bottomRight, bottomLeft))
        {
        }

        public GridLayout(Vector2 topLeft, Vector2 bottomRight) : base(Polygon.Rectangle.FromTwoPoints(topLeft, bottomRight))
        {
        }

        #endregion

        #region Events

        #endregion

        #region Props

        public Vector2 Size => Primitive.BoundingRectangle.Size;
        public Vector2 Position => Primitive.BoundingRectangle.TopLeft;
        public int CellCount => Rows + Columns;

        private int _rows = 3;

        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                Resize();
            }
        }

        private int _columns = 3;

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                Resize();
            }
        }

        #endregion
    }
}