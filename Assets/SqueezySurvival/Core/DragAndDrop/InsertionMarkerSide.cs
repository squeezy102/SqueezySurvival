namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>Which side of the target cell the dragged thing will land on.</summary>
    public enum InsertionMarkerSide
    {
        /// <summary>No marker.</summary>
        None,

        /// <summary>Just before the target: its left side in a vertical grid, its top in a horizontal one.</summary>
        Before,

        /// <summary>Just after the target: its right side in a vertical grid, its bottom in a horizontal one.</summary>
        After,
    }
}
