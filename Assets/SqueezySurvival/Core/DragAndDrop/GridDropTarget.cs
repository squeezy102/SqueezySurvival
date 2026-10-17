using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>Where a dragged cell is being held: over a cell's zone, over empty space in its grid, or elsewhere. Immutable.</summary>
    public sealed class GridDropTarget
    {
        /// <summary>Somewhere a drop does nothing: outside the grid, or over another panel.</summary>
        public static readonly GridDropTarget Nowhere = new GridDropTarget(GridDropTargetKind.Nowhere, -1, CellDropZone.Center);

        /// <summary>Empty space in the dragged cell's own grid, after its last cell.</summary>
        public static readonly GridDropTarget EmptyGridSpace = new GridDropTarget(GridDropTargetKind.EmptyGridSpace, -1, CellDropZone.Center);

        private GridDropTarget(GridDropTargetKind kind, int cellIndex, CellDropZone zone)
        {
            Kind = kind;
            CellIndex = cellIndex;
            Zone = zone;
        }

        /// <summary>What is under the pointer.</summary>
        public GridDropTargetKind Kind { get; }

        /// <summary>The cell under the pointer, when <see cref="Kind"/> is <see cref="GridDropTargetKind.Cell"/>.</summary>
        public int CellIndex { get; }

        /// <summary>The zone of that cell under the pointer.</summary>
        public CellDropZone Zone { get; }

        /// <summary>Over a zone of a cell in the dragged cell's own grid.</summary>
        public static GridDropTarget Cell(int cellIndex, CellDropZone zone)
        {
            if (cellIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cellIndex), cellIndex, "Cannot be negative.");
            }

            return new GridDropTarget(GridDropTargetKind.Cell, cellIndex, zone);
        }
    }
}
