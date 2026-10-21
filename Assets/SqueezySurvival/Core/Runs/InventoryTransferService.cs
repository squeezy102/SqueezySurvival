using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Moving things between the survivor's inventory and the ground: previewing a move and carrying it out
    /// (Docs/Design/UI/DragAndDrop.md, Between panels). How the player asked for the move is not its concern.
    /// </summary>
    public sealed class InventoryTransferService
    {
        private readonly SurvivorInventoryService _survivorInventory;
        private readonly GroundInventoryService _groundInventory;
        private readonly SurvivorActionCompletionService _actionCompletion;

        /// <summary>Moves things between <paramref name="survivorInventory"/> and <paramref name="groundInventory"/>.</summary>
        public InventoryTransferService(
            SurvivorInventoryService survivorInventory,
            GroundInventoryService groundInventory,
            SurvivorActionCompletionService actionCompletion)
        {
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
        }

        /// <summary>Whether anything can be moved now: not while a find waits for room on the ground.</summary>
        public bool CanTransfer => _actionCompletion.CanStartAction;

        /// <summary>The slots of a panel things move between.</summary>
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel) =>
            panel == ItemPanel.Inventory ? _survivorInventory.Inventory : _groundInventory.Ground;

        /// <summary>
        /// What moving <paramref name="quantity"/> of the stack in <paramref name="fromSlot"/> to the other panel would
        /// do, letting go over <paramref name="targetSlot"/> (-1 for no particular slot). Changes nothing.
        /// </summary>
        public ItemMovePreview PreviewTransfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            ItemPanel to = Other(from);
            CountedEntry<ItemDefinition> stack = ContentsOf(from).EntryAtSlot(fromSlot)
                ?? throw new ArgumentOutOfRangeException(nameof(fromSlot), fromSlot, "Nothing is in that slot.");
            CountedCollection<ItemDefinition> destination = ContentsOf(to);
            int count = ItemTransferQuantityCalculator.CountToMove(stack.Count, quantity);
            return new ItemMovePreview(
                stack.Definition,
                count,
                from,
                to,
                destination.SlotToReceive(stack.Definition, targetSlot),
                destination.CountOf(stack.Definition),
                ItemMoveTimeRules.SecondsToMove(stack.Definition.WeightPounds * count));
        }

        /// <summary>
        /// Moves things to the other panel as <see cref="PreviewTransfer"/> describes: takes its seconds and logs it.
        /// There is no weight cap. Not available while a find waits for room on the ground, or when there is no room.
        /// </summary>
        public void Transfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            _actionCompletion.RequireCanStartAction();
            ItemMovePreview preview = PreviewTransfer(from, fromSlot, targetSlot, quantity);
            if (!preview.CanMove)
            {
                throw new InvalidOperationException("There is no room there.");
            }

            ContentsOf(from).Take(fromSlot, preview.Count);
            ContentsOf(preview.To).Receive(preview.Definition, preview.Count, preview.DestinationSlot);
            _actionCompletion.Complete(
                ActionDuration.FromSeconds(preview.Seconds),
                new[] { new EventLogMessage(ItemMoveTextFormatter.FormatLog(preview)) });
        }

        private static ItemPanel Other(ItemPanel panel) => panel == ItemPanel.Inventory ? ItemPanel.Ground : ItemPanel.Inventory;
    }
}
