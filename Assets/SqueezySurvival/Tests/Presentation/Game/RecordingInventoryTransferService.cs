using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A transfer service that allows every move between the ground and a free slot of the inventory and only records the
    /// moves it is asked to carry out, so a test can tell whether the game tried to move things.
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

        /// <summary>The (from, slot, quantity) of every transfer or send carried out, in order.</summary>
        public List<(ItemPanel From, int FromSlot, ItemTransferQuantity Quantity)> Transfers { get; } =
            new List<(ItemPanel, int, ItemTransferQuantity)>();

        /// <inheritdoc />
        public bool CanTransfer => true;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel) => panel == ItemPanel.Ground ? _ground : _inventory;

        /// <inheritdoc />
        public ItemMovePreview PreviewTransfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            CountedEntry<ItemDefinition> entry = ContentsOf(from).EntryAtSlot(fromSlot);
            return new ItemMovePreview(entry.Definition, entry.Count, from, to, 0, 0, 5);
        }

        /// <inheritdoc />
        public ItemMovePreview PreviewSend(ItemPanel from, int fromSlot, ItemTransferQuantity quantity) =>
            PreviewTransfer(from, from == ItemPanel.Ground ? ItemPanel.Inventory : ItemPanel.Ground, fromSlot, -1, quantity);

        /// <inheritdoc />
        public void Transfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            Transfers.Add((from, fromSlot, quantity));
        }

        /// <inheritdoc />
        public void Send(ItemPanel from, int fromSlot, ItemTransferQuantity quantity)
        {
            Transfers.Add((from, fromSlot, quantity));
        }
    }
}
