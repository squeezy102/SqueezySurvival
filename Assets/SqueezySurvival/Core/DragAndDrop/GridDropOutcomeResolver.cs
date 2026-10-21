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
        /// The outcome of releasing <paramref name="quantity"/> of the stack in <paramref name="sourceSlot"/> over
        /// <paramref name="target"/>: into an empty slot, whole or split; merged with a stack of the same kind; swapped
        /// with another thing; or combined with it, once held over it long enough.
        /// </summary>
        public GridDropOutcome Resolve<TDefinition>(
            CountedCollection<TDefinition> collection,
            int sourceSlot,
            GridDropTarget target,
            ItemTransferQuantity quantity = ItemTransferQuantity.All)
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

            int count = ItemTransferQuantityCalculator.CountToMove(dragged.Count, quantity);
            switch (target.Kind)
            {
                case GridDropTargetKind.EmptySlot:
                    return count < dragged.Count
                        ? new GridDropOutcome(
                            GridDropOutcomeKind.Split, target.Slot, target.Slot, $"Put {PartOf(dragged, count)} here.", count: count)
                        : new GridDropOutcome(
                            GridDropOutcomeKind.Move, target.Slot, target.Slot, $"Put the {Lower(dragged.Definition)} here.");
                case GridDropTargetKind.OccupiedSlot:
                    return ResolveOverOccupiedSlot(collection.EntryAtSlot(target.Slot), dragged, target.AttemptsCombine, count);
                default:
                    return GridDropOutcome.ReturnToPlace;
            }
        }

        private GridDropOutcome ResolveOverOccupiedSlot<TDefinition>(
            CountedEntry<TDefinition> hovered,
            CountedEntry<TDefinition> dragged,
            bool attemptsCombine,
            int count)
            where TDefinition : class, ICountableThingDefinition
        {
            if (hovered.Slot == dragged.Slot && !attemptsCombine)
            {
                return NoChange(hovered.Slot);
            }

            // A stack held over itself combines two of its own, as two stones make a sharpened stone.
            if (hovered.Slot == dragged.Slot && dragged.Count < 2)
            {
                return new GridDropOutcome(
                    GridDropOutcomeKind.CannotCombine,
                    -1,
                    hovered.Slot,
                    $"There is only one {Lower(dragged.Definition)}. It takes two to combine one with itself.");
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

            if (hovered.Definition.Id == dragged.Definition.Id)
            {
                return new GridDropOutcome(
                    GridDropOutcomeKind.Merge,
                    hovered.Slot,
                    hovered.Slot,
                    $"Add {PartOf(dragged, count)} to the stack of {hovered.Count} here.",
                    count: count);
            }

            if (count < dragged.Count)
            {
                return new GridDropOutcome(
                    GridDropOutcomeKind.NoChange,
                    -1,
                    hovered.Slot,
                    $"Split onto an empty slot or another stack of {dragged.Definition.PluralName}.");
            }

            return new GridDropOutcome(
                GridDropOutcomeKind.Swap,
                hovered.Slot,
                hovered.Slot,
                $"The {Lower(dragged.Definition)} and the {Lower(hovered.Definition)} trade places.");
        }

        // "the stone", "the 3 stones", "one of the 7 stones", "4 of the 7 stones".
        private static string PartOf<TDefinition>(CountedEntry<TDefinition> stack, int count)
            where TDefinition : class, ICountableThingDefinition
        {
            string whole = stack.Count == 1 ? $"the {Lower(stack.Definition)}" : $"the {stack.Count} {stack.Definition.PluralName}";
            if (count == stack.Count)
            {
                return whole;
            }

            return count == 1 ? $"one of {whole}" : $"{count} of {whole}";
        }

        private static GridDropOutcome NoChange(int anchorSlot) =>
            new GridDropOutcome(GridDropOutcomeKind.NoChange, -1, anchorSlot, "It stays where it is.");

        private static string Lower(ICountableThingDefinition thing) => thing.Name.ToLowerInvariant();
    }
}
