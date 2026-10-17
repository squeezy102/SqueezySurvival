namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>Where the drop marker is drawn against the target cell.</summary>
    public enum DropMarkerSide
    {
        /// <summary>The whole target slot is outlined.</summary>
        WholeSlot,

        /// <summary>A bar on the target cell's left side: the dragged thing goes before it.</summary>
        Before,

        /// <summary>A bar on the target cell's right side: the dragged thing goes after it.</summary>
        After,
    }
}
