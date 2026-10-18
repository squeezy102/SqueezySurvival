using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Where a dragged cell is being held: over an occupied slot, long enough or not to attempt a combine, over an empty slot, or somewhere a drop
    /// does nothing. Immutable.
    /// </summary>
    public sealed class GridDropTarget
    {
        /// <summary>Somewhere a drop does nothing: outside the grid, or over another panel.</summary>
        public static readonly GridDropTarget Nowhere = new GridDropTarget(GridDropTargetKind.Nowhere, -1, false);

        private GridDropTarget(GridDropTargetKind kind, int slot, bool attemptsCombine)
        {
            Kind = kind;
            Slot = slot;
            AttemptsCombine = attemptsCombine;
        }

        /// <summary>What is under the pointer.</summary>
        public GridDropTargetKind Kind { get; }

        /// <summary>The slot under the pointer, or -1.</summary>
        public int Slot { get; }

        /// <summary>
        /// Whether the dragged thing has been held over an occupied slot long enough to attempt a combine
        /// (Docs/Design/UI/DragAndDrop.md, Moving, swapping and combining).
        /// </summary>
        public bool AttemptsCombine { get; }

        /// <summary>Over an occupied slot in the dragged cell's own grid.</summary>
        public static GridDropTarget OccupiedSlot(int slot, bool attemptsCombine) =>
            new GridDropTarget(GridDropTargetKind.OccupiedSlot, RequireSlot(slot), attemptsCombine);

        /// <summary>Over an empty slot in the dragged cell's own grid.</summary>
        public static GridDropTarget EmptySlot(int slot) =>
            new GridDropTarget(GridDropTargetKind.EmptySlot, RequireSlot(slot), false);

        private static int RequireSlot(int slot) =>
            slot >= 0 ? slot : throw new ArgumentOutOfRangeException(nameof(slot), slot, "Cannot be negative.");
    }
}
