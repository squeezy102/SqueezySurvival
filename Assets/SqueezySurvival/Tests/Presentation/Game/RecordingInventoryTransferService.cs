using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A transfer service that allows every move from the ground to a free slot of the inventory and only records the
    /// moves it is asked to carry out, so a test can tell whether the game tried to send things to the other panel.
    /// </summary>
    public sealed class RecordingInventoryTransferService : IInventoryTransferService
    {
        private readonly CountedCollection<ItemDefinition> _ground;
        private readonly CountedCollection<ItemDefinition> _inventory;

        /// <summary>Creates the service over the two panels' contents.</summary>
        public RecordingInventoryTransferService(CountedCollection<ItemDefinition> ground, CountedCollection<ItemDefinition> inventory)
        {
            _ground = ground;
            _inventory = inventory;
        }

        /// <summary>The (from, slot, quantity) of every transfer carried out, in order.</summary>
        public List<(ItemPanel From, int FromSlot, ItemTransferQuantity Quantity)> Transfers { get; } =
            new List<(ItemPanel, int, ItemTransferQuantity)>();

        /// <inheritdoc />
        public bool CanTransfer => true;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel) => panel == ItemPanel.Ground ? _ground : _inventory;

        /// <inheritdoc />
        public ItemMovePreview PreviewTransfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            CountedEntry<ItemDefinition> entry = ContentsOf(from).EntryAtSlot(fromSlot);
            ItemPanel to = from == ItemPanel.Ground ? ItemPanel.Inventory : ItemPanel.Ground;
            return new ItemMovePreview(entry.Definition, entry.Count, from, to, 0, 0, 5);
        }

        /// <inheritdoc />
        public void Transfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            Transfers.Add((from, fromSlot, quantity));
        }
    }
}
