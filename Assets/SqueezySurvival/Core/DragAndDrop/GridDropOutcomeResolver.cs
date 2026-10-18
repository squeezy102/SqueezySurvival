using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Decides what releasing a dragged grid cell would do, and how to describe it, before anything happens
    /// (Docs/Design/UI/DragAndDrop.md, Drop zones).
    /// </summary>
    public sealed class GridDropOutcomeResolver
    {
        private readonly IThingCombinationRules _combinationRules;

        /// <summary>Creates a resolver that asks the given rules about combining.</summary>
        public GridDropOutcomeResolver(IThingCombinationRules combinationRules)
        {
            _combinationRules = combinationRules ?? throw new ArgumentNullException(nameof(combinationRules));
        }

        /// <summary>
        /// The outcome of releasing the thing in <paramref name="sourceSlot"/> over <paramref name="target"/>: into an
        /// empty slot; swapped with another thing; or combined with it, once held over it long enough.
        /// </summary>
        public GridDropOutcome Resolve<TDefinition>(CountedCollection<TDefinition> collection, int sourceSlot, GridDropTarget target)
            where TDefinition : class, ICountableThingDefinition
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            CountedEntry<TDefinition> dragged = collection.EntryAtSlot(sourceSlot)
                ?? throw new ArgumentOutOfRangeException(nameof(sourceSlot), sourceSlot, "Nothing is in that slot.");

            switch (target.Kind)
            {
                case GridDropTargetKind.EmptySlot:
                    return new GridDropOutcome(
                        GridDropOutcomeKind.Move, target.Slot, target.Slot, $"Put the {Lower(dragged.Definition)} here.");
                case GridDropTargetKind.OccupiedSlot:
                    return ResolveOverOccupiedSlot(collection.EntryAtSlot(target.Slot), dragged, target.AttemptsCombine);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverOccupiedSlot<TDefinition>(
            CountedEntry<TDefinition> hovered,
            CountedEntry<TDefinition> dragged,
            bool attemptsCombine)
            where TDefinition : class, ICountableThingDefinition
        {
            if (hovered.Slot == dragged.Slot)
            {
                return NoChange(hovered.Slot);
            }

            if (attemptsCombine)
            {
                return _combinationRules.TryCombine(dragged.Definition, hovered.Definition, out string combination)
                    ? new GridDropOutcome(GridDropOutcomeKind.Combine, -1, hovered.Slot, combination)
                    : new GridDropOutcome(
                        GridDropOutcomeKind.CannotCombine,
                        -1,
                        hovered.Slot,
                        $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} can't be combined.");
            }

            return new GridDropOutcome(
                GridDropOutcomeKind.Swap,
                hovered.Slot,
                hovered.Slot,
                $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} trade places.");
        }

        private static GridDropOutcome NoChange(int anchorSlot) =>
            new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, anchorSlot, "It stays where it is.");

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
