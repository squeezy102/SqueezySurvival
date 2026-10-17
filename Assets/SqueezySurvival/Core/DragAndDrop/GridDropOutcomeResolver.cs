using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Decides what releasing a dragged grid cell would do, and how to describe it, before anything happens. A move
    /// only ever involves the dragged thing and at most one other (Docs/Design/UI/DragAndDrop.md).
    /// </summary>
    public sealed class GridDropOutcomeResolver
    {
        private const string StaysDescription = "It stays where it is.";

        private readonly IThingCombinationRules _combinationRules;

        /// <summary>Creates a resolver that asks the given rules about combining.</summary>
        public GridDropOutcomeResolver(IThingCombinationRules combinationRules)
        {
            _combinationRules = combinationRules ?? throw new ArgumentNullException(nameof(combinationRules));
        }

        /// <summary>The outcome of releasing the thing in <paramref name="sourceSlot"/> over <paramref name="target"/>.</summary>
        public GridDropOutcome Resolve<TDefinition>(
            CountedCollection<TDefinition> collection,
            GridSlotNeighborhood neighborhood,
            int sourceSlot,
            GridDropTarget target)
            where TDefinition : class, ICountableThingDefinition
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (neighborhood == null)
            {
                throw new ArgumentNullException(nameof(neighborhood));
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
                    return ResolveOverOccupiedSlot(collection, neighborhood, dragged, target);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverOccupiedSlot<TDefinition>(
            CountedCollection<TDefinition> collection,
            GridSlotNeighborhood neighborhood,
            CountedEntry<TDefinition> dragged,
            GridDropTarget target)
            where TDefinition : class, ICountableThingDefinition
        {
            CountedEntry<TDefinition> hovered = collection.EntryAtSlot(target.Slot);
            if (target.Zone == CellDropZone.Center)
            {
                if (hovered.Slot == dragged.Slot)
                {
                    return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, hovered.Slot, StaysDescription);
                }

                return _combinationRules.TryCombine(dragged.Definition, hovered.Definition, out string combination)
                    ? new GridDropOutcome(GridDropOutcomeKind.Combine, -1, hovered.Slot, combination)
                    : new GridDropOutcome(
                        GridDropOutcomeKind.CannotCombine,
                        -1,
                        hovered.Slot,
                        $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} can't be combined.");
            }

            // An edge means "the slot on that side of this one".
            int neighborSlot = neighborhood.NeighborOf(hovered.Slot, target.Zone);
            if (neighborSlot < 0)
            {
                return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, hovered.Slot, "There is no space on that side.");
            }

            if (neighborSlot == dragged.Slot)
            {
                return new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, hovered.Slot, StaysDescription);
            }

            CountedEntry<TDefinition> occupant = collection.EntryAtSlot(neighborSlot);
            if (occupant == null)
            {
                return new GridDropOutcome(
                    GridDropOutcomeKind.Move,
                    neighborSlot,
                    neighborSlot,
                    $"Place the {Lower(dragged.Definition)} {SideWords(target.Zone)} the {Lower(hovered.Definition)}.");
            }

            return new GridDropOutcome(
                GridDropOutcomeKind.Swap,
                neighborSlot,
                neighborSlot,
                $"The {Lower(dragged.Definition)} and the {Lower(occupant.Definition)} trade places.");
        }

        private static string SideWords(CellDropZone edge)
        {
            switch (edge)
            {
                case CellDropZone.Left:
                    return "left of";
                case CellDropZone.Right:
                    return "right of";
                case CellDropZone.Top:
                    return "above";
                default:
                    return "below";
            }
        }

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
