using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Where a dragged cell is being held: over a zone of an occupied slot, over an empty slot, or somewhere a drop
    /// does nothing. Immutable.
    /// </summary>
    public sealed class GridDropTarget
    {
        /// <summary>Somewhere a drop does nothing: outside the grid, or over another panel.</summary>
        public static readonly GridDropTarget Nowhere = new GridDropTarget(GridDropTargetKind.Nowhere, -1, CellDropZone.Center);

        private GridDropTarget(GridDropTargetKind kind, int slot, CellDropZone zone)
        {
            Kind = kind;
            Slot = slot;
            Zone = zone;
        }

        /// <summary>What is under the pointer.</summary>
        public GridDropTargetKind Kind { get; }

        /// <summary>The slot under the pointer, or -1.</summary>
        public int Slot { get; }

        /// <summary>The zone of an occupied slot under the pointer.</summary>
        public CellDropZone Zone { get; }

        /// <summary>Over a zone of an occupied slot in the dragged cell's own grid.</summary>
        public static GridDropTarget OccupiedSlot(int slot, CellDropZone zone) =>
            new GridDropTarget(GridDropTargetKind.OccupiedSlot, RequireSlot(slot), zone);

        /// <summary>Over an empty slot in the dragged cell's own grid.</summary>
        public static GridDropTarget EmptySlot(int slot) =>
            new GridDropTarget(GridDropTargetKind.EmptySlot, RequireSlot(slot), CellDropZone.Center);

        private static int RequireSlot(int slot) =>
            slot >= 0 ? slot : throw new ArgumentOutOfRangeException(nameof(slot), slot, "Cannot be negative.");
    }
}
