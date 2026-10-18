using System;

namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>
    /// Works out a fixed grid's geometry from its shape and the space it has: square cells as large as the space
    /// allows, with the grid centered down and placed across it as asked (Docs/Design/UI/InventoryGrids.md, The grids).
    /// </summary>
    public static class FixedGridLayoutCalculator
    {
        /// <summary>Space between cells, in pixels at the reference resolution.</summary>
        public const float CellGap = 8f;

        /// <summary>Calculates the layout of <paramref name="shape"/> in an area of the given size.</summary>
        public static FixedGridLayout Calculate(
            FixedGridShape shape,
            float areaWidth,
            float areaHeight,
            FixedGridHorizontalAlignment horizontalAlignment = FixedGridHorizontalAlignment.Center)
        {
            if (shape == null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            float width = NonNegativeOrZero(areaWidth);
            float height = NonNegativeOrZero(areaHeight);
            float cellSize = Math.Max(0f, Math.Min(CellSize(width, shape.Columns), CellSize(height, shape.Rows)));
            float gridWidth = Extent(shape.Columns, cellSize);
            float gridHeight = Extent(shape.Rows, cellSize);
            float gridLeft = horizontalAlignment == FixedGridHorizontalAlignment.Left ? 0f : (width - gridWidth) / 2f;
            return new FixedGridLayout(shape, cellSize, gridLeft, (height - gridHeight) / 2f, gridWidth, gridHeight);
        }

        /// <summary>
        /// The width a grid needs so that, in an area of the given height, its cells are as tall as the height allows:
        /// used to size the hands beside the inventory so both have the same cells.
        /// </summary>
        public static float WidthForHeight(FixedGridShape shape, float areaHeight)
        {
            if (shape == null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            float cellSize = Math.Max(0f, CellSize(NonNegativeOrZero(areaHeight), shape.Rows));
            return Extent(shape.Columns, cellSize);
        }

        private static float CellSize(float extent, int lines) => (extent - (CellGap * (lines - 1))) / lines;

        private static float Extent(int lines, float cellSize) => (lines * cellSize) + ((lines - 1) * CellGap);

        // Before its first layout pass, a UI element reports its size as NaN; treat that as no space yet.
        private static float NonNegativeOrZero(float extent) => float.IsNaN(extent) || extent < 0f ? 0f : extent;
    }
}
