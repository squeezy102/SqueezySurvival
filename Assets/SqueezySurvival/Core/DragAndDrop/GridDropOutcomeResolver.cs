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
        /// empty slot; before or after another thing; or combined with it, over its center.
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
                        GridDropOutcomeKind.Move, target.Slot, target.Slot, DropMarkerSide.WholeSlot, $"Put the {Lower(dragged.Definition)} here.");
                case GridDropTargetKind.OccupiedSlot:
                    return ResolveOverOccupiedSlot(collection, collection.EntryAtSlot(target.Slot), dragged, target.Zone);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverOccupiedSlot<TDefinition>(
            CountedCollection<TDefinition> collection,
            CountedEntry<TDefinition> hovered,
            CountedEntry<TDefinition> dragged,
            CellDropZone zone)
            where TDefinition : class, ICountableThingDefinition
        {
            if (hovered.Slot == dragged.Slot)
            {
                return NoChange(hovered.Slot);
            }

            if (zone == CellDropZone.Center)
            {
                return _combinationRules.TryCombine(dragged.Definition, hovered.Definition, out string combination)
                    ? new GridDropOutcome(GridDropOutcomeKind.Combine, -1, hovered.Slot, DropMarkerSide.WholeSlot, combination)
                    : new GridDropOutcome(
                        GridDropOutcomeKind.CannotCombine,
                        -1,
                        hovered.Slot,
                        DropMarkerSide.WholeSlot,
                        $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} can't be combined.");
            }

            // After: the slot after the hovered thing. Before: the empty slot just before it, so nothing shifts, or
            // else the hovered thing's own slot, shifting it and those after it one slot later.
            bool before = zone == CellDropZone.Left;
            int slotBefore = hovered.Slot - 1;
            int targetSlot = !before
                ? hovered.Slot + 1
                : slotBefore >= 0 && (slotBefore == dragged.Slot || collection.EntryAtSlot(slotBefore) == null) ? slotBefore : hovered.Slot;

            if (targetSlot == dragged.Slot)
            {
                return NoChange(hovered.Slot);
            }

            return new GridDropOutcome(
                GridDropOutcomeKind.Move,
                targetSlot,
                hovered.Slot,
                before ? DropMarkerSide.Before : DropMarkerSide.After,
                $"Put the {Lower(dragged.Definition)} {(before ? "before" : "after")} the {Lower(hovered.Definition)}.");
        }

        private static GridDropOutcome NoChange(int anchorSlot) =>
            new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, anchorSlot, DropMarkerSide.WholeSlot, "It stays where it is.");

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
