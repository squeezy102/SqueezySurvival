using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>Presents a Core <see cref="CountedCollection{TDefinition}"/> to a <see cref="InventoryGridView"/>.</summary>
    public sealed class CountedCollectionInventoryGridContents<TDefinition> : IInventoryGridContents
        where TDefinition : class, ICountableThingDefinition
    {
        private readonly CountedCollection<TDefinition> _collection;
        private readonly GridDropOutcomeResolver _dropOutcomeResolver;

        /// <summary>Wraps a collection; drops are judged by the given resolver.</summary>
        public CountedCollectionInventoryGridContents(CountedCollection<TDefinition> collection, GridDropOutcomeResolver dropOutcomeResolver)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _dropOutcomeResolver = dropOutcomeResolver ?? throw new ArgumentNullException(nameof(dropOutcomeResolver));
        }

        /// <inheritdoc />
        public event Action Changed
        {
            add => _collection.Changed += value;
            remove => _collection.Changed -= value;
        }

        /// <inheritdoc />
        public int SlotCapacity => _collection.SlotCapacity;

        /// <inheritdoc />
        public bool IsOccupied(int slot) => _collection.EntryAtSlot(slot) != null;

        /// <inheritdoc />
        public string DefinitionIdAt(int slot) => Entry(slot).Definition.Id;

        /// <inheritdoc />
        public string CountBadgeAt(int slot) => CountedThingTextFormatter.FormatCountBadge(Entry(slot).Count);

        /// <inheritdoc />
        public ThingTooltipContent TooltipAt(int slot) => ThingTooltipContentFactory.Create(Entry(slot));

        /// <inheritdoc />
        public GridDropOutcome ResolveDrop(int sourceSlot, GridDropTarget target, ItemTransferQuantity quantity) =>
            _dropOutcomeResolver.Resolve(_collection, sourceSlot, target, quantity);

        /// <inheritdoc />
        public void ApplyDrop(int sourceSlot, GridDropOutcome outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            switch (outcome.Kind)
            {
                case GridDropOutcomeKind.Move:
                case GridDropOutcomeKind.Swap:
                    _collection.MoveToSlot(sourceSlot, outcome.TargetSlot);
                    break;
                case GridDropOutcomeKind.Split:
                    _collection.SplitToSlot(sourceSlot, outcome.TargetSlot, outcome.Count);
                    break;
                case GridDropOutcomeKind.Merge:
                    _collection.MergeIntoSlot(sourceSlot, outcome.TargetSlot, outcome.Count);
                    break;
                case GridDropOutcomeKind.Combine:
                    // No combination rules return a match until crafting exists (Docs/Design/UI/DragAndDrop.md, Combining).
                    throw new NotSupportedException("Combining is not built yet.");
                default:
                    break;
            }
        }

        /// <inheritdoc />
        public void Sort(ThingSortOrder order) => _collection.Sort(new ThingSortComparer<TDefinition>(order));

        private CountedEntry<TDefinition> Entry(int slot) =>
            _collection.EntryAtSlot(slot) ?? throw new ArgumentOutOfRangeException(nameof(slot), slot, "Nothing is in that slot.");
    }
}
