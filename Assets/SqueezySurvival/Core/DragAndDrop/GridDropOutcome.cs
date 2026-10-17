namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>What releasing a drag here would do, and how to preview it. Immutable.</summary>
    public sealed class GridDropOutcome
    {
        /// <summary>Releasing does nothing; there is nothing to preview.</summary>
        public static readonly GridDropOutcome ReturnToPlace =
            new GridDropOutcome(GridDropOutcomeKind.ReturnToPlace, -1, -1, InsertionMarkerSide.None, string.Empty);

        /// <summary>Creates an outcome.</summary>
        public GridDropOutcome(GridDropOutcomeKind kind, int insertionIndex, int targetCellIndex, InsertionMarkerSide markerSide, string description)
        {
            Kind = kind;
            InsertionIndex = insertionIndex;
            TargetCellIndex = targetCellIndex;
            MarkerSide = markerSide;
            Description = description ?? string.Empty;
        }

        /// <summary>What happens on release.</summary>
        public GridDropOutcomeKind Kind { get; }

        /// <summary>For a move, the insertion index to pass to <c>CountedCollection.Move</c>.</summary>
        public int InsertionIndex { get; }

        /// <summary>The cell the preview is shown against, or -1.</summary>
        public int TargetCellIndex { get; }

        /// <summary>For a move onto a cell, which side of it the thing lands on.</summary>
        public InsertionMarkerSide MarkerSide { get; }

        /// <summary>The preview text: what will happen on release.</summary>
        public string Description { get; }

        /// <summary>The preview's heading: "Move", "Combine", "No change" or "Can't combine".</summary>
        public string Title
        {
            get
            {
                switch (Kind)
                {
                    case GridDropOutcomeKind.Move:
                        return "Move";
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

        /// <summary>Whether releasing changes anything.</summary>
        public bool ChangesSomething => Kind == GridDropOutcomeKind.Move || Kind == GridDropOutcomeKind.Combine;
    }
}
