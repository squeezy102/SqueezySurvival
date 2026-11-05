using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Game;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>Grid contents that hold nothing and only record the sort orders it is asked for.</summary>
    public sealed class RecordingInventoryGridContents : IInventoryGridContents
    {
        /// <summary>The orders sorted in, in order.</summary>
        public List<ThingSortOrder> Sorts { get; } = new List<ThingSortOrder>();

        /// <inheritdoc />
        public event EventHandler<CountedCollectionChange> Changed
        {
            add { }
            remove { }
        }

        /// <inheritdoc />
        public long Revision => 0;

        /// <inheritdoc />
        public int SlotCapacity => 0;

        /// <inheritdoc />
        public bool IsOccupied(int slot) => false;

        /// <inheritdoc />
        public string DefinitionIdAt(int slot) => throw new InvalidOperationException("The grid is empty.");

        /// <inheritdoc />
        public string CountBadgeAt(int slot) => throw new InvalidOperationException("The grid is empty.");

        /// <inheritdoc />
        public ThingTooltipContent TooltipAt(int slot) => throw new InvalidOperationException("The grid is empty.");

        /// <inheritdoc />
        public GridDropOutcome ResolveDrop(int sourceSlot, GridDropTarget target, ItemTransferQuantity quantity) =>
            throw new InvalidOperationException("The grid is empty.");

        /// <inheritdoc />
        public void ApplyDrop(int sourceSlot, GridDropOutcome outcome) => throw new InvalidOperationException("The grid is empty.");

        /// <inheritdoc />
        public void Sort(ThingSortOrder order) => Sorts.Add(order);
    }
}
