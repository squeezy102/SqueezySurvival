namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What releasing a drag here would do, and how to preview it. Immutable.</summary>
    public sealed class GridDropOutcome
    {
        /// <summary>Releasing does nothing; there is nothing to preview.</summary>
        public static readonly GridDropOutcome ReturnToPlace =
            new GridDropOutcome(GridDropOutcomeKind.ReturnToPlace, -1, -1, string.Empty);

        /// <summary>Creates an outcome.</summary>
        /// <param name="kind">What happens on release.</param>
        /// <param name="targetSlot">For a move or swap, the slot the dragged thing lands in; otherwise -1.</param>
        /// <param name="anchorSlot">The slot the marker and tooltip are shown against, or -1 for none.</param>
        /// <param name="description">The preview text.</param>
        public GridDropOutcome(GridDropOutcomeKind kind, int targetSlot, int anchorSlot, string description)
        {
            Kind = kind;
            TargetSlot = targetSlot;
            AnchorSlot = anchorSlot;
            Description = description ?? string.Empty;
        }

        /// <summary>What happens on release.</summary>
        public GridDropOutcomeKind Kind { get; }

        /// <summary>For a move or swap, the slot the dragged thing lands in; for a swap, what was there takes the dragged thing's old slot.</summary>
        public int TargetSlot { get; }

        /// <summary>The slot the marker and tooltip are shown against, or -1.</summary>
        public int AnchorSlot { get; }

        /// <summary>The preview text: what will happen on release.</summary>
        public string Description { get; }

        /// <summary>Whether releasing changes anything.</summary>
        public bool ChangesSomething =>
            Kind == GridDropOutcomeKind.Move || Kind == GridDropOutcomeKind.Swap || Kind == GridDropOutcomeKind.Combine;

        /// <summary>The preview's heading.</summary>
        public string Title
        {
            get
            {
                switch (Kind)
                {
                    case GridDropOutcomeKind.Move:
                        return "Move";
                    case GridDropOutcomeKind.Swap:
                        return "Swap";
                    case GridDropOutcomeKind.Combine:
                        return "Combine";
                    case GridDropOutcomeKind.CannotCombine:
                        return "Can't combine";
                    case GridDropOutcomeKind.NoChange:
                        return "No change";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
