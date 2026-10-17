namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>One of a grid cell's five hidden drop zones (Docs/Design/UI/DragAndDrop.md, Drop zones).</summary>
    public enum CellDropZone
    {
        /// <summary>The middle of the cell: combine.</summary>
        Center,

        /// <summary>The left edge: move before.</summary>
        Left,

        /// <summary>The right edge: move after.</summary>
        Right,

        /// <summary>The top edge: move before.</summary>
        Top,

        /// <summary>The bottom edge: move after.</summary>
        Bottom,
    }
}
