using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>Presents a Core <see cref="CountedCollection{TDefinition}"/> to a <see cref="GridPanelView"/>.</summary>
    public sealed class CountedCollectionGridContents<TDefinition> : IGridPanelContents
        where TDefinition : class, ICountableThingDefinition
    {
        private readonly CountedCollection<TDefinition> _collection;
        private readonly GridDropOutcomeResolver _dropOutcomeResolver;

        /// <summary>Wraps a collection; drops are judged by the given resolver.</summary>
        public CountedCollectionGridContents(CountedCollection<TDefinition> collection, GridDropOutcomeResolver dropOutcomeResolver)
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
        public int Count => _collection.Entries.Count;

        /// <inheritdoc />
        public string DefinitionIdAt(int index) => _collection.Entries[index].Definition.Id;

        /// <inheritdoc />
        public string CountBadgeAt(int index) => CountedThingTextFormatter.FormatCountBadge(_collection.Entries[index].Count);

        /// <inheritdoc />
        public ThingTooltipContent TooltipAt(int index) => ThingTooltipContentFactory.Create(_collection.Entries[index]);

        /// <inheritdoc />
        public GridDropOutcome ResolveDrop(int sourceIndex, GridDropTarget target) =>
            _dropOutcomeResolver.Resolve(_collection.Entries, sourceIndex, target);

        /// <inheritdoc />
        public void ApplyDrop(int sourceIndex, GridDropOutcome outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            switch (outcome.Kind)
            {
                case GridDropOutcomeKind.Move:
                    _collection.Move(sourceIndex, outcome.InsertionIndex);
                    break;
                case GridDropOutcomeKind.Combine:
                    // No combination rules return a match until crafting exists (Docs/Design/UI/DragAndDrop.md, Combining).
                    throw new NotSupportedException("Combining is not built yet.");
                default:
                    break;
            }
        }
    }
}
