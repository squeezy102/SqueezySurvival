namespace SqueezySurvival.Core.Items
{
    /// <summary>How much of a stack a move takes (Docs/Design/UI/Controls.md, Principles).</summary>
    public enum ItemTransferQuantity
    {
        /// <summary>The whole stack.</summary>
        All,

        /// <summary>Half the stack, rounded up.</summary>
        Half,

        /// <summary>A single thing.</summary>
        One,
    }
}
