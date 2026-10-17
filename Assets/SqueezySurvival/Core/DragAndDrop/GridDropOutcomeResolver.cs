using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Decides what releasing a dragged grid cell would do, and how to describe it, before anything happens. A drop
    /// only ever involves the dragged thing and the thing under the pointer (Docs/Design/UI/DragAndDrop.md).
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
        /// The outcome of releasing the thing in <paramref name="sourceSlot"/> over <paramref name="target"/>: it goes to
        /// the slot under the pointer, moving into an empty one or swapping with the thing there, or combining with
        /// that thing when released over its center.
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
                    return new GridDropOutcome(GridDropOutcomeKind.Move, target.Slot, target.Slot, $"Place the {Lower(dragged.Definition)} here.");
                case GridDropTargetKind.OccupiedSlot:
                    return ResolveOverOccupiedSlot(collection.EntryAtSlot(target.Slot), dragged, target.Zone);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverOccupiedSlot<TDefinition>(
            CountedEntry<TDefinition> hovered,
            CountedEntry<TDefinition> dragged,
            CellDropZone zone)
            where TDefinition : class, ICountableThingDefinition
        {
            if (hovered.Slot == dragged.Slot)
            {
                return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, hovered.Slot, "It stays where it is.");
            }

            if (zone != CellDropZone.Center)
            {
                return new GridDropOutcome(
                    GridDropOutcomeKind.Swap,
                    hovered.Slot,
                    hovered.Slot,
                    $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} trade places.");
            }

            return _combinationRules.TryCombine(dragged.Definition, hovered.Definition, out string combination)
                ? new GridDropOutcome(GridDropOutcomeKind.Combine, -1, hovered.Slot, combination)
                : new GridDropOutcome(
                    GridDropOutcomeKind.CannotCombine,
                    -1,
                    hovered.Slot,
                    $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} can't be combined.");
        }

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
