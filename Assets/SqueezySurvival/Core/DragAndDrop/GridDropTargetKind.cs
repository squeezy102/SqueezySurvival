namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What a dragged cell is being held over.</summary>
    public enum GridDropTargetKind
    {
        /// <summary>Nothing a drop can act on.</summary>
        Nowhere,

        /// <summary>A cell in the same grid.</summary>
        Cell,

        /// <summary>Empty space in the same grid.</summary>
        EmptyGridSpace,
    }
}
