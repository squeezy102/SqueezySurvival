namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Where over an occupied grid cell a dragged thing is held (Docs/Design/UI/DragAndDrop.md, Drop zones). The top and
    /// bottom edges belong to click-and-hold bulk moves, not to dragging, so a drag only distinguishes these three.
    /// </summary>
    public enum CellDropZone
    {
        /// <summary>The middle of the cell: combine.</summary>
        Center,

        /// <summary>The left side: put the dragged thing before this one.</summary>
        Left,

        /// <summary>The right side: put the dragged thing after this one.</summary>
        Right,
    }
}
