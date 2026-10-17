using System;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Which slot lies beside another in a grid of fixed slots, given the grid's scroll direction and fixed line
    /// count (Docs/Design/UI/DragAndDrop.md, Drop zones). Immutable.
    /// </summary>
    public sealed class GridSlotNeighborhood
    {
        /// <summary>Describes a grid filled in the given direction with the given number of fixed lines.</summary>
        public GridSlotNeighborhood(GridPanelScrollDirection scrollDirection, int fixedLineCount)
        {
            if (fixedLineCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(fixedLineCount), fixedLineCount, "Must be at least 1.");
            }

            ScrollDirection = scrollDirection;
            FixedLineCount = fixedLineCount;
        }

        /// <summary>The grid's scroll direction.</summary>
        public GridPanelScrollDirection ScrollDirection { get; }

        /// <summary>Columns of a vertical grid, rows of a horizontal one.</summary>
        public int FixedLineCount { get; }

        /// <summary>
        /// The slot beside <paramref name="slot"/> on the side of the given edge, or -1 when there is none (past the
        /// grid's fixed edge, or before its start). The grid grows without limit along its scroll direction.
        /// </summary>
        public int NeighborOf(int slot, CellDropZone edge)
        {
            if (slot < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, "Cannot be negative.");
            }

            int n = FixedLineCount;
            int alongFixed = slot % n;
            bool vertical = ScrollDirection == GridPanelScrollDirection.Vertical;

            // In a vertical grid, columns are the fixed lines: left and right stay within a row, up and down step a
            // whole row. A horizontal grid is the same turned on its side.
            CellDropZone stepWithinLine = vertical ? CellDropZone.Right : CellDropZone.Bottom;
            CellDropZone stepBackWithinLine = vertical ? CellDropZone.Left : CellDropZone.Top;
            CellDropZone stepToNextLine = vertical ? CellDropZone.Bottom : CellDropZone.Right;
            CellDropZone stepToPreviousLine = vertical ? CellDropZone.Top : CellDropZone.Left;

            if (edge == stepWithinLine)
            {
                return alongFixed < n - 1 ? slot + 1 : -1;
            }

            if (edge == stepBackWithinLine)
            {
                return alongFixed > 0 ? slot - 1 : -1;
            }

            if (edge == stepToNextLine)
            {
                return slot + n;
            }

            if (edge == stepToPreviousLine)
            {
                return slot >= n ? slot - n : -1;
            }

            throw new ArgumentOutOfRangeException(nameof(edge), edge, "The center has no neighbor.");
        }
    }
}
