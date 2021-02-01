using System.Numerics;
using Aptacode.Geometry.Primitives;

namespace Aptacode.AppFramework.Components.Containers.Layouts
{
    public class GridLayout : Layout
    {
        #region Overrides

        public override void Resize()
        {
            var cellSize = new Vector2(Size.X / _columns, Size.Y / _rows);
            var cellPosition = Position;
            for (var j = 0; j < _rows; j++)
            {
                for (var k = 0; k < _columns; k++)
                {
                    var child = Cells[j][k];
                    if (child != null)
                    {
                        RepositionChild(child, cellSize, new Vector2(cellPosition.X + cellSize.X * k, cellPosition.Y + cellSize.Y * j));
                    }
                }
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

        private void ResizeCells()
        {
            var newCells = new ComponentViewModel[_rows][];

            for (var i = 0; i < newCells.Length; i++)
            {
                var row = newCells[i] = new ComponentViewModel[_columns];
                for (var j = 0; j < row.Length; j++)
                {
                    ComponentViewModel cell = null;
                    if (Cells != null && i < Cells.Length && j < Cells[i].Length)
                    {
                        cell = Cells[i][j];
                    }

                    row[j] = cell;
                }
            }

            Cells = newCells;
            Resize();
        }

        public (int, int) GetCell(ComponentViewModel child)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                var row = Cells[i];
                for (var j = 0; j < row.Length; j++)
                {
                    if (row[j]?.Id == child.Id)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        public void SetCell(ComponentViewModel child, int rowIndex, int columnIndex)
        {
            Add(child);

            var oldCell = GetCell(child);
            if (oldCell != (-1, -1))
            {
                Cells[rowIndex][columnIndex] = null;
            }

            Cells[rowIndex][columnIndex] = child;

            Resize();
        }

        public override void Add(ComponentViewModel child)
        {
            base.Add(child);

            for (var i = 0; i < Cells.Length; i++)
            {
                var row = Cells[i];
                for (var j = 0; j < row.Length; j++)
                {
                    if (row[j] == null)
                    {
                        row[j] = child;
                        Resize();
                        return;
                    }
                }
            }
        }


        private ComponentViewModel[][] Cells;

        public override void Remove(ComponentViewModel child)
        {
            var cell = GetCell(child);
            if (cell != (-1, -1))
            {
                Cells[cell.Item1][cell.Item2] = null;
            }

            base.Remove(child);
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
                ResizeCells();
            }
        }

        private int _columns = 3;

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                ResizeCells();
            }
        }

        #endregion
    }
}