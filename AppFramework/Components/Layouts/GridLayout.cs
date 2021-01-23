using System;
using System.Linq;
using System.Numerics;
using Aptacode.AppFramework.Enums;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Layouts
{
    public class GridLayout : Layout
    {
        public override void Resize()
        {
            var childCount = Children.Count();
            var childIndex = 0;
            Console.WriteLine("Resize");

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
                        UpdateChild(child, cellSize, cellPosition);
                    }

                    cellPosition += new Vector2(cellSize.X, 0);
                }

                cellPosition += new Vector2(0, cellSize.Y);
            }
        }

        private void UpdateChild(ComponentViewModel child, Vector2 cellSize, Vector2 cellPosition)
        {
            var scale = cellSize / child.BoundingRectangle.Size;
            var xScale = 1.0f;
            var yScale = 1.0f;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    yScale = scale.Y;
                    break;
                case VerticalAlignment.Top:
                    yScale = Math.Min(scale.Y, 1.0f);
                    break;
                case VerticalAlignment.Center:
                    yScale = Math.Min(scale.Y, 1.0f);
                    break;
                case VerticalAlignment.Bottom:
                    yScale = Math.Min(scale.Y, 1.0f);
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    xScale = scale.X;
                    break;
                case HorizontalAlignment.Left:
                    xScale = Math.Min(scale.X, 1.0f);
                    break;
                case HorizontalAlignment.Center:
                    xScale = Math.Min(scale.X, 1.0f);
                    break;
                case HorizontalAlignment.Right:
                    xScale = Math.Min(scale.X, 1.0f);
                    break;
            }

            child.Scale(new Vector2(xScale, yScale));

            var delta = cellPosition - child.BoundingRectangle.TopLeft;
            var xDelta = delta.X;
            var yDelta = delta.Y;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    break;
                case VerticalAlignment.Top:
                    break;
                case VerticalAlignment.Center:
                    yDelta += Math.Clamp((cellSize.Y - child.BoundingRectangle.Size.Y) / 2, 0, cellSize.Y);
                    break;
                case VerticalAlignment.Bottom:
                    yDelta += Math.Clamp(cellSize.Y - child.BoundingRectangle.Size.Y, 0, cellSize.Y);
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    break;
                case HorizontalAlignment.Left:
                    break;
                case HorizontalAlignment.Center:
                    xDelta += Math.Clamp((cellSize.X - child.BoundingRectangle.Size.X) / 2, 0, cellSize.X);
                    break;
                case HorizontalAlignment.Right:
                    xDelta += Math.Clamp(cellSize.X - child.BoundingRectangle.Size.X, 0, cellSize.X);
                    break;
            }


            child.Translate(new Vector2(xDelta, yDelta));
        }

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

        private VerticalAlignment _verticalAlignment = VerticalAlignment.Stretch;

        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                Resize();
            }
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Stretch;

        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                Resize();
            }
        }

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