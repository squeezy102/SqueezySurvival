using System;

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
        /// The cell at a point in the content (pixels from its top-left corner), or -1 when the point is past the
        /// last cell. Each cell owns the gap after it, so there is no dead space between cells. The fractions say
        /// where in that cell the point is, from 0 (left or top) to 1 (right or bottom), and may exceed 1 in a gap.
        /// </summary>
        public int CellIndexAt(float left, float top, int cellCount, out float xFraction, out float yFraction)
        {
            xFraction = 0f;
            yFraction = 0f;
            float pitch = CellSize + GridPanelLayoutCalculator.CellGap;
            if (cellCount <= 0 || CellSize <= 0f)
            {
                return -1;
            }

            int column = Math.Max(0, (int)Math.Floor(Math.Max(0f, left) / pitch));
            int row = Math.Max(0, (int)Math.Floor(Math.Max(0f, top) / pitch));
            bool vertical = _scrollDirection == GridPanelScrollDirection.Vertical;
            int lineAlongFixed = vertical ? column : row;
            int lineAlongScroll = vertical ? row : column;
            if (lineAlongFixed >= _fixedLineCount)
            {
                lineAlongFixed = _fixedLineCount - 1;
            }

            int index = (lineAlongScroll * _fixedLineCount) + lineAlongFixed;
            if (index >= cellCount)
            {
                return -1;
            }

            GridCellPosition position = PositionOf(index);
            xFraction = (left - position.Left) / CellSize;
            yFraction = (top - position.Top) / CellSize;
            return index;
        }

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
