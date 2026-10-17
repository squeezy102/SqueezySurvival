using System;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// Works out a grid panel's geometry from its settings and the space it has: cell size, where each cell goes, and
    /// how large the scrolling content is (Docs/Design/UI/GridPanels.md, The grid). The panel never resizes; cells
    /// are sized to fit it.
    /// </summary>
    public static class GridPanelLayoutCalculator
    {
        /// <summary>Space between cells, in pixels at the reference resolution.</summary>
        public const float CellGap = 8f;

        /// <summary>Calculates the grid for <paramref name="cellCount"/> cells in a viewport of the given size.</summary>
        public static GridPanelLayout Calculate(GridPanelLayoutOptions options, float viewportWidth, float viewportHeight, int cellCount)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (cellCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cellCount), cellCount, "Cannot be negative.");
            }

            bool vertical = options.ScrollDirection == GridPanelScrollDirection.Vertical;
            int fixedLines = options.FixedLineCount;
            float fixedExtent = NonNegativeOrZero(vertical ? viewportWidth : viewportHeight);
            float cellSize = Math.Max(0f, (fixedExtent - (CellGap * (fixedLines - 1))) / fixedLines);

            int scrollingLines = (cellCount + fixedLines - 1) / fixedLines;
            float scrollingExtent = scrollingLines == 0 ? 0f : (scrollingLines * cellSize) + ((scrollingLines - 1) * CellGap);

            return vertical
                ? new GridPanelLayout(options.ScrollDirection, fixedLines, cellSize, fixedExtent, scrollingExtent)
                : new GridPanelLayout(options.ScrollDirection, fixedLines, cellSize, scrollingExtent, fixedExtent);
        }

        // Before its first layout pass, a UI element reports its size as NaN; treat that as no space yet.
        private static float NonNegativeOrZero(float extent) => float.IsNaN(extent) || extent < 0f ? 0f : extent;
    }
}
