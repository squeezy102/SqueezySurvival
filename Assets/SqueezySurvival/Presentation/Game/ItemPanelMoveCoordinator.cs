using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using UnityEngine;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Connects the grids of every panel things move between (the ground, the inventory and the open container) so things
    /// can be dragged or modifier-clicked from one to another (Docs/Design/UI/DragAndDrop.md, Between panels and
    /// Destinations). What a move does, where a modified click sends things, and the wording all come from Core's
    /// <see cref="IInventoryTransferService"/>; this only finds which grid the pointer is over and passes the request on.
    /// </summary>
    public sealed class ItemPanelMoveCoordinator
    {
        private readonly IInventoryTransferService _inventoryTransfers;
        private readonly IMoveModifierReader _modifiers;
        private readonly Dictionary<ItemPanel, InventoryGridView> _views = new Dictionary<ItemPanel, InventoryGridView>();

        /// <summary>Moves things through <paramref name="inventoryTransfers"/>, reading modifiers with <paramref name="modifiers"/>.</summary>
        public ItemPanelMoveCoordinator(IInventoryTransferService inventoryTransfers, IMoveModifierReader modifiers)
        {
            _inventoryTransfers = inventoryTransfers ?? throw new ArgumentNullException(nameof(inventoryTransfers));
            _modifiers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
        }

        /// <summary>Connects a panel's grid. Each panel has exactly one.</summary>
        public void Register(ItemPanel panel, InventoryGridView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            _views.Add(panel, view);
            view.EnableMovesBetweenPanels(panel, this);
        }

        /// <summary>
        /// Finds the grid of a panel other than <paramref name="from"/> that <paramref name="pointerWorldPosition"/> is
        /// over, with its panel and the slot under the pointer (-1 between slots). Returns false, with no panel or grid,
        /// when the pointer is over none.
        /// </summary>
        public bool TryFindOtherPanelUnder(
            ItemPanel from, Vector2 pointerWorldPosition, out ItemPanel panel, out InventoryGridView view, out int hoveredSlot)
        {
            foreach (KeyValuePair<ItemPanel, InventoryGridView> candidate in _views)
            {
                if (candidate.Key == from || !candidate.Value.GridArea.worldBound.Contains(pointerWorldPosition))
                {
                    continue;
                }

                panel = candidate.Key;
                view = candidate.Value;
                hoveredSlot = view.SlotAtWorldPosition(pointerWorldPosition, out _, out _);
                return true;
            }

            panel = default;
            view = null;
            hoveredSlot = -1;
            return false;
        }

        /// <summary>Whether things can be moved between panels now.</summary>
        public bool CanTransfer => _inventoryTransfers.CanTransfer;

        /// <summary>Changes whenever any connected panel's contents change, so a held drag can tell its preview is still right.</summary>
        public long PanelContentsRevision
        {
            get
            {
                long total = 0;
                foreach (ItemPanel panel in _views.Keys)
                {
                    total += _inventoryTransfers.ContentsOf(panel).Revision;
                }

                return total;
            }
        }

        /// <summary>How much a drag released now would move: the held modifier's role, or the whole stack.</summary>
        public ItemTransferQuantity QuantityForDrag() => _modifiers.HeldQuantity() ?? ItemTransferQuantity.All;

        /// <summary>
        /// What releasing the stack in <paramref name="fromSlot"/> of <paramref name="from"/> over <paramref name="to"/>
        /// would do. Nothing moves while a find waits for room on the ground.
        /// </summary>
        public GridDropOutcome Resolve(ItemPanel from, ItemPanel to, int fromSlot, int hoveredSlot, ItemTransferQuantity quantity)
        {
            if (!_inventoryTransfers.CanTransfer)
            {
                return GridDropOutcome.ReturnToPlace;
            }

            return ItemMoveDropOutcomeFactory.Create(_inventoryTransfers.PreviewTransfer(from, to, fromSlot, hoveredSlot, quantity), hoveredSlot);
        }

        /// <summary>Carries out a move <see cref="Resolve"/> previewed as possible.</summary>
        public void Apply(ItemPanel from, ItemPanel to, int fromSlot, int hoveredSlot, ItemTransferQuantity quantity)
        {
            _inventoryTransfers.Transfer(from, to, fromSlot, hoveredSlot, quantity);
        }

        /// <summary>
        /// A modified click: sends the held modifier's amount of the stack where Core's destination policy says. Returns
        /// false, changing nothing, when no modifier is held, so the click does its usual thing.
        /// </summary>
        public bool TrySendModifiedClick(ItemPanel from, int fromSlot)
        {
            ItemTransferQuantity? quantity = _modifiers.HeldQuantity();
            if (quantity == null)
            {
                return false;
            }

            if (!_inventoryTransfers.CanTransfer)
            {
                return true;
            }

            GridDropOutcome outcome = ItemMoveDropOutcomeFactory.Create(_inventoryTransfers.PreviewSend(from, fromSlot, quantity.Value), -1);
            if (outcome.Kind == GridDropOutcomeKind.Transfer)
            {
                _inventoryTransfers.Send(from, fromSlot, quantity.Value);
            }
            else if (outcome.Kind == GridDropOutcomeKind.NoRoom)
            {
                _views[from].ShowBriefMessage(outcome, fromSlot);
            }

            return true;
        }
    }
}
