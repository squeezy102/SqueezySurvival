namespace SqueezySurvival.Core.Options
{
    /// <summary>A grid panel's calculated geometry, in pixels. Immutable; made by <see cref="GridPanelLayoutCalculator"/>.</summary>
    public sealed class GridPanelLayout
    {
        private readonly GridPanelScrollDirection _scrollDirection;
        private readonly int _fixedLineCount;

        /// <summary>Creates a layout.</summary>
        public GridPanelLayout(
            GridPanelScrollDirection scrollDirection,
            int fixedLineCount,
            float cellSize,
            float contentWidth,
            float contentHeight,
            bool needsScrolling)
        {
            _scrollDirection = scrollDirection;
            _fixedLineCount = fixedLineCount;
            CellSize = cellSize;
            ContentWidth = contentWidth;
            ContentHeight = contentHeight;
            NeedsScrolling = needsScrolling;
        }

        /// <summary>Width and height of every cell.</summary>
        public float CellSize { get; }

        /// <summary>Width of all the cells together.</summary>
        public float ContentWidth { get; }

        /// <summary>Height of all the cells together.</summary>
        public float ContentHeight { get; }

        /// <summary>Whether the cells overflow the grid area, so the scrollbar is enabled and the wheel scrolls it.</summary>
        public bool NeedsScrolling { get; }

        /// <summary>
        /// The top-left corner of the cell at <paramref name="cellIndex"/>. Vertical grids fill left to right, then
        /// down; horizontal grids fill top to bottom, then right.
        /// </summary>
        public GridCellPosition PositionOf(int cellIndex)
        {
            int lineAlongFixed = cellIndex % _fixedLineCount;
            int lineAlongScroll = cellIndex / _fixedLineCount;
            float pitch = CellSize + GridPanelLayoutCalculator.CellGap;

            return _scrollDirection == GridPanelScrollDirection.Vertical
                ? new GridCellPosition(lineAlongFixed * pitch, lineAlongScroll * pitch)
                : new GridCellPosition(lineAlongScroll * pitch, lineAlongFixed * pitch);
        }
    }
}
