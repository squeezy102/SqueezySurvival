using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Moves things between the survivor's inventory and the current zone's ground, timed by weight.</summary>
    public sealed class InventoryTransferService : IInventoryTransferService
    {
        private readonly ISurvivorInventoryService _survivorInventory;
        private readonly IGroundInventoryService _groundInventory;
        private readonly ISurvivorActionCompletionService _actionCompletion;
        private readonly ItemMoveTimeRules _itemMoveTimeRules;
        private readonly IItemArrivalStackingSetting _arrivalStacking;

        /// <summary>
        /// Moves things between <paramref name="survivorInventory"/> and <paramref name="groundInventory"/>, each move
        /// taking the time <paramref name="itemMoveTimeRules"/> gives it and landing as <paramref name="arrivalStacking"/>
        /// says.
        /// </summary>
        public InventoryTransferService(
            ISurvivorInventoryService survivorInventory,
            IGroundInventoryService groundInventory,
            ISurvivorActionCompletionService actionCompletion,
            ItemMoveTimeRules itemMoveTimeRules,
            IItemArrivalStackingSetting arrivalStacking)
        {
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
            _itemMoveTimeRules = itemMoveTimeRules ?? throw new ArgumentNullException(nameof(itemMoveTimeRules));
            _arrivalStacking = arrivalStacking ?? throw new ArgumentNullException(nameof(arrivalStacking));
        }

        /// <inheritdoc />
        public bool CanTransfer => _actionCompletion.CanStartAction;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel) =>
            panel == ItemPanel.Inventory ? _survivorInventory.Inventory : _groundInventory.Ground;

        /// <inheritdoc />
        public ItemMovePreview PreviewTransfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            ItemPanel to = Other(from);
            CountedEntry<ItemDefinition> stack = ContentsOf(from).EntryAtSlot(fromSlot)
                ?? throw new ArgumentOutOfRangeException(nameof(fromSlot), fromSlot, "Nothing is in that slot.");
            CountedCollection<ItemDefinition> destination = ContentsOf(to);
            int count = ItemTransferQuantityCalculator.CountToMove(stack.Count, quantity);
            int destinationSlot = destination.SlotToReceive(stack.Definition, targetSlot, _arrivalStacking.Stacking);
            return new ItemMovePreview(
                stack.Definition,
                count,
                from,
                to,
                destinationSlot,
                destination.CountInSlot(stack.Definition, destinationSlot),
                _itemMoveTimeRules.SecondsToMove(stack.Definition.WeightPounds * count));
        }

        /// <inheritdoc />
        public void Transfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            _actionCompletion.RequireCanStartAction();
            ItemMovePreview preview = PreviewTransfer(from, fromSlot, targetSlot, quantity);
            if (!preview.CanMove)
            {
                throw new InvalidOperationException("There is no room there.");
            }

            ContentsOf(from).Take(fromSlot, preview.Count);
            ContentsOf(preview.To).Receive(preview.Definition, preview.Count, preview.DestinationSlot, _arrivalStacking.Stacking);
            _actionCompletion.Complete(
                ActionDuration.FromSeconds(preview.Seconds),
                new[] { new EventLogMessage(ItemMoveTextFormatter.FormatLog(preview)) });
        }

        private static ItemPanel Other(ItemPanel panel) => panel == ItemPanel.Inventory ? ItemPanel.Ground : ItemPanel.Inventory;
    }
}
