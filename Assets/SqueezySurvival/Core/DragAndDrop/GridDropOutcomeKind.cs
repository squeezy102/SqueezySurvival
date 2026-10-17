namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What releasing a drag would do (Docs/Design/UI/DragAndDrop.md, Preview).</summary>
    public enum GridDropOutcomeKind
    {
        /// <summary>Nothing: the dragged thing returns. No preview.</summary>
        ReturnToPlace,

        /// <summary>Releasing changes nothing; the preview says why.</summary>
        NoChange,

        /// <summary>The dragged thing moves to a slot; anything there shifts one slot later, up to the next gap.</summary>
        Move,

        /// <summary>The two things combine.</summary>
        Combine,

        /// <summary>The two things cannot be combined; releasing changes nothing.</summary>
        CannotCombine,
    }
}
