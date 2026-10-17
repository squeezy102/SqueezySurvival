using System;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// Works out a grid panel's geometry from its settings and the space it has: cell size, where each cell goes, how
    /// large the scrolling content is, and whether it needs to scroll at all (Docs/Design/UI/GridPanels.md, The grid).
    /// The panel never resizes, and room for the scrollbar is always kept, so cells never change size as things are
    /// added.
    /// </summary>
    public static class GridPanelLayoutCalculator
    {
        /// <summary>Space between cells, and between the cells and a scrollbar, in pixels at the reference resolution.</summary>
        public const float CellGap = 8f;

        /// <summary>
        /// Calculates the grid for <paramref name="cellCount"/> cells in a grid area of the given size. A scrollbar of
        /// <paramref name="scrollbarThickness"/>, with a cell gap before it, always takes its place along the scroll
        /// direction's edge; it is enabled only when the cells overflow.
        /// </summary>
        public static GridPanelLayout Calculate(
            GridPanelLayoutOptions options,
            float areaWidth,
            float areaHeight,
            int cellCount,
            float scrollbarThickness)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (cellCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cellCount), cellCount, "Cannot be negative.");
            }

            if (scrollbarThickness < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(scrollbarThickness), scrollbarThickness, "Cannot be negative.");
            }

            bool vertical = options.ScrollDirection == GridPanelScrollDirection.Vertical;
            int fixedLines = options.FixedLineCount;
            float fixedExtent = NonNegativeOrZero(vertical ? areaWidth : areaHeight);
            float scrollingRoom = NonNegativeOrZero(vertical ? areaHeight : areaWidth);
            int scrollingLines = (cellCount + fixedLines - 1) / fixedLines;

            float cellAreaExtent = Math.Max(0f, fixedExtent - scrollbarThickness - CellGap);
            float cellSize = CellSize(cellAreaExtent, fixedLines);
            float scrollingExtent = ScrollingExtent(scrollingLines, cellSize);
            bool needsScrolling = scrollingExtent > scrollingRoom;

            return vertical
                ? new GridPanelLayout(options.ScrollDirection, fixedLines, cellSize, cellAreaExtent, scrollingExtent, needsScrolling)
                : new GridPanelLayout(options.ScrollDirection, fixedLines, cellSize, scrollingExtent, cellAreaExtent, needsScrolling);
        }

        private static float CellSize(float extent, int lines) => Math.Max(0f, (extent - (CellGap * (lines - 1))) / lines);

        private static float ScrollingExtent(int lines, float cellSize) => lines == 0 ? 0f : (lines * cellSize) + ((lines - 1) * CellGap);

        // Before its first layout pass, a UI element reports its size as NaN; treat that as no space yet.
        private static float NonNegativeOrZero(float extent) => float.IsNaN(extent) || extent < 0f ? 0f : extent;
    }
}
