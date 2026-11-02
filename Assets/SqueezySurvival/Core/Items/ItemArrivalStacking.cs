namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Whether things moved into a panel join the stack of their kind that is already there
    /// (Docs/Design/UI/DragAndDrop.md, Between panels).
    /// </summary>
    public enum ItemArrivalStacking
    {
        /// <summary>Things join the stack let go over, or else the kind's first stack. The default.</summary>
        AutoStack = 0,

        /// <summary>Things take the slot let go over, or the first empty slot; they join a stack only when pointed at.</summary>
        StartOwnStack = 1
    }
}
