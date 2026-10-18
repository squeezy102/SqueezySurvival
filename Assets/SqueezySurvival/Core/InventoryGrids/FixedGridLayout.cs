using System;

namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>A fixed grid's calculated geometry, in pixels. Immutable; made by <see cref="FixedGridLayoutCalculator"/>.</summary>
    public sealed class FixedGridLayout
    {
        private readonly FixedGridShape _shape;

        /// <summary>Creates a layout.</summary>
        public FixedGridLayout(FixedGridShape shape, float cellSize, float gridLeft, float gridTop, float gridWidth, float gridHeight)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            CellSize = cellSize;
            GridLeft = gridLeft;
            GridTop = gridTop;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
        }

        /// <summary>Width and height of every cell.</summary>
        public float CellSize { get; }

        /// <summary>Where the grid's left edge is in its area.</summary>
        public float GridLeft { get; }

        /// <summary>Where the grid's top edge is in its area.</summary>
        public float GridTop { get; }

        /// <summary>Width of all the cells together.</summary>
        public float GridWidth { get; }

        /// <summary>Height of all the cells together.</summary>
        public float GridHeight { get; }

        /// <summary>The top-left corner of the cell for <paramref name="slot"/>, filling left to right, then down.</summary>
        public GridCellPosition PositionOf(int slot)
        {
            RequireSlot(slot);
            float pitch = CellSize + FixedGridLayoutCalculator.CellGap;
            return new GridCellPosition(GridLeft + ((slot % _shape.Columns) * pitch), GridTop + ((slot / _shape.Columns) * pitch));
        }

        /// <summary>
        /// The slot at a point in the area, or -1 when the point is outside the grid. Each cell owns the gap after it,
        /// so there is no dead space between cells. The fractions say where in that cell the point is, from 0 (left or
        /// top) to 1 (right or bottom), and may exceed 1 in a gap.
        /// </summary>
        public int SlotAt(float left, float top, out float xFraction, out float yFraction)
        {
            xFraction = 0f;
            yFraction = 0f;
            float x = left - GridLeft;
            float y = top - GridTop;
            if (CellSize <= 0f || x < 0f || y < 0f || x > GridWidth || y > GridHeight)
            {
                return -1;
            }

            float pitch = CellSize + FixedGridLayoutCalculator.CellGap;
            int column = Math.Min(_shape.Columns - 1, (int)Math.Floor(x / pitch));
            int row = Math.Min(_shape.Rows - 1, (int)Math.Floor(y / pitch));
            xFraction = (x - (column * pitch)) / CellSize;
            yFraction = (y - (row * pitch)) / CellSize;
            return (row * _shape.Columns) + column;
        }

        private void RequireSlot(int slot)
        {
            if (slot < 0 || slot >= _shape.SlotCount)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, $"Must be from 0 to {_shape.SlotCount - 1}.");
            }
        }
    }
}
