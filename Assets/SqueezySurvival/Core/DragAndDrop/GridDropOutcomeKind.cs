namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What releasing a drag would do (Docs/Design/UI/DragAndDrop.md, Preview).</summary>
    public enum GridDropOutcomeKind
    {
        /// <summary>Nothing: the dragged thing returns. No preview.</summary>
        ReturnToPlace,

        /// <summary>The dragged thing would end up where it already is.</summary>
        NoChange,

        /// <summary>The dragged thing moves to a new place in the order.</summary>
        Move,

        /// <summary>The two things combine.</summary>
        Combine,

        /// <summary>The two things cannot be combined; releasing changes nothing.</summary>
        CannotCombine,
    }
}
