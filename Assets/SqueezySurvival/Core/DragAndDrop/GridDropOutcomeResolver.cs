using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Decides what releasing a dragged grid cell would do, and how to describe it, before anything happens
    /// (Docs/Design/UI/DragAndDrop.md).
    /// </summary>
    public sealed class GridDropOutcomeResolver
    {
        private const string NoChangeDescription = "It stays where it is.";

        private readonly IThingCombinationRules _combinationRules;

        /// <summary>Creates a resolver that asks the given rules about combining.</summary>
        public GridDropOutcomeResolver(IThingCombinationRules combinationRules)
        {
            _combinationRules = combinationRules ?? throw new ArgumentNullException(nameof(combinationRules));
        }

        /// <summary>The outcome of releasing the cell at <paramref name="sourceIndex"/> over <paramref name="target"/>.</summary>
        public GridDropOutcome Resolve<TDefinition>(IReadOnlyList<CountedEntry<TDefinition>> entries, int sourceIndex, GridDropTarget target)
            where TDefinition : class, ICountableThingDefinition
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (sourceIndex < 0 || sourceIndex >= entries.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceIndex), sourceIndex, "No cell at that index.");
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            TDefinition dragged = entries[sourceIndex].Definition;
            switch (target.Kind)
            {
                case GridDropTargetKind.EmptyGridSpace:
                    return MoveOutcome(sourceIndex, entries.Count, -1, InsertionMarkerSide.None, $"Place the {Lower(dragged)} at the end.");
                case GridDropTargetKind.Cell when target.CellIndex < entries.Count:
                    return ResolveOverCell(entries, sourceIndex, target);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverCell<TDefinition>(IReadOnlyList<CountedEntry<TDefinition>> entries, int sourceIndex, GridDropTarget target)
            where TDefinition : class, ICountableThingDefinition
        {
            int targetIndex = target.CellIndex;
            TDefinition dragged = entries[sourceIndex].Definition;
            TDefinition onto = entries[targetIndex].Definition;

            if (target.Zone == CellDropZone.Center)
            {
                if (targetIndex == sourceIndex)
                {
                    return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, targetIndex, InsertionMarkerSide.None, NoChangeDescription);
                }

                return _combinationRules.TryCombine(dragged, onto, out string combination)
                    ? new GridDropOutcome(GridDropOutcomeKind.Combine, -1, targetIndex, InsertionMarkerSide.None, combination)
                    : new GridDropOutcome(
                        GridDropOutcomeKind.CannotCombine, -1, targetIndex, InsertionMarkerSide.None, $"The {Lower(dragged)} and the {Lower(onto)} can't be combined.");
            }

            bool before = target.Zone == CellDropZone.Left || target.Zone == CellDropZone.Top;
            int insertionIndex = before ? targetIndex : targetIndex + 1;
            string place = before ? "before" : "after";
            return MoveOutcome(
                sourceIndex,
                insertionIndex,
                targetIndex,
                before ? InsertionMarkerSide.Before : InsertionMarkerSide.After,
                $"Place the {Lower(dragged)} {place} the {Lower(onto)}.");
        }

        private static GridDropOutcome MoveOutcome(int sourceIndex, int insertionIndex, int targetIndex, InsertionMarkerSide side, string description)
        {
            if (OrderedListMove.ResultingIndex(sourceIndex, insertionIndex) == sourceIndex)
            {
                return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, targetIndex, InsertionMarkerSide.None, NoChangeDescription);
            }

            return new GridDropOutcome(GridDropOutcomeKind.Move, insertionIndex, targetIndex, side, description);
        }

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
