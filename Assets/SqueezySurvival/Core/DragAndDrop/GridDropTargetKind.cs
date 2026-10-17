namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What a dragged cell is being held over.</summary>
    public enum GridDropTargetKind
    {
        /// <summary>Nothing a drop can act on.</summary>
        Nowhere,

        /// <summary>A slot holding something, in the same grid.</summary>
        OccupiedSlot,

        /// <summary>An empty slot in the same grid.</summary>
        EmptySlot,
    }
}
