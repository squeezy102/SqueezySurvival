using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Moves things between the survivor's inventory, the current zone's ground and the open container, timed by weight.
    /// </summary>
    public sealed class InventoryTransferService : IInventoryTransferService
    {
        private readonly ISurvivorInventoryService _survivorInventory;
        private readonly IGroundInventoryService _groundInventory;
        private readonly IOpenItemContainerSource _openContainers;
        private readonly ItemMoveDestinationPolicy _destinationPolicy;
        private readonly ISurvivorActionCompletionService _actionCompletion;
        private readonly ItemMoveTimeRules _itemMoveTimeRules;
        private readonly IItemArrivalStackingSetting _arrivalStacking;

        /// <summary>
        /// Moves things between <paramref name="survivorInventory"/>, <paramref name="groundInventory"/> and the container
        /// <paramref name="openContainers"/> says is open, sending a modified click where
        /// <paramref name="destinationPolicy"/> says, each move taking the time <paramref name="itemMoveTimeRules"/>
        /// gives it and landing as <paramref name="arrivalStacking"/> says.
        /// </summary>
        public InventoryTransferService(
            ISurvivorInventoryService survivorInventory,
            IGroundInventoryService groundInventory,
            IOpenItemContainerSource openContainers,
            ItemMoveDestinationPolicy destinationPolicy,
            ISurvivorActionCompletionService actionCompletion,
            ItemMoveTimeRules itemMoveTimeRules,
            IItemArrivalStackingSetting arrivalStacking)
        {
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            _openContainers = openContainers ?? throw new ArgumentNullException(nameof(openContainers));
            _destinationPolicy = destinationPolicy ?? throw new ArgumentNullException(nameof(destinationPolicy));
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
            _itemMoveTimeRules = itemMoveTimeRules ?? throw new ArgumentNullException(nameof(itemMoveTimeRules));
            _arrivalStacking = arrivalStacking ?? throw new ArgumentNullException(nameof(arrivalStacking));
        }

        /// <inheritdoc />
        public bool CanTransfer => _actionCompletion.CanStartAction;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel)
        {
            switch (panel.Kind)
            {
                case ItemPanelKind.Inventory:
                    return _survivorInventory.Inventory;
                case ItemPanelKind.Ground:
                    return _groundInventory.Ground;
                case ItemPanelKind.Container:
                    return OpenContainerFor(panel).Contents;
                default:
                    throw new ArgumentOutOfRangeException(nameof(panel), panel.Kind, "Unknown kind of panel.");
            }
        }

        /// <inheritdoc />
        public ItemMovePreview PreviewTransfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            if (from == to)
            {
                throw new ArgumentException("A move goes between two different panels.", nameof(to));
            }

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
                _itemMoveTimeRules.SecondsToMove(stack.Definition.WeightPounds * count),
                to.Kind == ItemPanelKind.Container ? OpenContainerFor(to).Name : null);
        }

        /// <inheritdoc />
        public ItemMovePreview PreviewSend(ItemPanel from, int fromSlot, ItemTransferQuantity quantity)
        {
            IReadOnlyList<ItemPanel> destinations = _destinationPolicy.DestinationsFor(from, _openContainers.Current?.Panel);
            ItemMovePreview firstChoice = null;
            foreach (ItemPanel destination in destinations)
            {
                ItemMovePreview preview = PreviewTransfer(from, destination, fromSlot, -1, quantity);
                if (preview.CanMove)
                {
                    return preview;
                }

                firstChoice = firstChoice ?? preview;
            }

            return firstChoice;
        }

        /// <inheritdoc />
        public void Transfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            _actionCompletion.RequireCanStartAction();
            Carry(PreviewTransfer(from, to, fromSlot, targetSlot, quantity), fromSlot);
        }

        /// <inheritdoc />
        public void Send(ItemPanel from, int fromSlot, ItemTransferQuantity quantity)
        {
            _actionCompletion.RequireCanStartAction();
            Carry(PreviewSend(from, fromSlot, quantity), fromSlot);
        }

        private void Carry(ItemMovePreview preview, int fromSlot)
        {
            if (!preview.CanMove)
            {
                throw new InvalidOperationException("There is no room there.");
            }

            ContentsOf(preview.From).Take(fromSlot, preview.Count);
            ContentsOf(preview.To).Receive(preview.Definition, preview.Count, preview.DestinationSlot, _arrivalStacking.Stacking);
            _actionCompletion.Complete(
                ActionDuration.FromSeconds(preview.Seconds),
                new[] { new EventLogMessage(ItemMoveTextFormatter.FormatLog(preview)) });
        }

        private IOpenItemContainer OpenContainerFor(ItemPanel panel)
        {
            IOpenItemContainer open = _openContainers.Current;
            if (open == null || open.Panel != panel)
            {
                throw new ArgumentException("That container is not open.", nameof(panel));
            }

            return open;
        }
    }
}
