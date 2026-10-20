using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using UnityEngine;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Connects the ground's and the inventory's grids so things can be dragged or modifier-clicked from one to the
    /// other (Docs/Design/UI/DragAndDrop.md, Between panels). What a move does, and its wording, come from Core's
    /// <see cref="RunSession"/>; this only finds which grid the pointer is over and passes the request on.
    /// </summary>
    public sealed class ItemPanelMoveCoordinator
    {
        private readonly RunSession _session;
        private readonly KeyboardMoveModifierReader _modifiers;
        private readonly Dictionary<ItemPanel, InventoryGridView> _views = new Dictionary<ItemPanel, InventoryGridView>();

        /// <summary>Moves things within <paramref name="session"/>, reading modifiers with <paramref name="modifiers"/>.</summary>
        public ItemPanelMoveCoordinator(RunSession session, KeyboardMoveModifierReader modifiers)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
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
        /// The other panel's grid when <paramref name="pointerWorldPosition"/> is over it, with the slot under the
        /// pointer (-1 between slots); null when the pointer is over neither.
        /// </summary>
        public InventoryGridView OtherPanelUnder(ItemPanel from, Vector2 pointerWorldPosition, out int hoveredSlot)
        {
            hoveredSlot = -1;
            if (!_views.TryGetValue(Other(from), out InventoryGridView other) || !other.GridArea.worldBound.Contains(pointerWorldPosition))
            {
                return null;
            }

            hoveredSlot = other.SlotAtWorldPosition(pointerWorldPosition, out _, out _);
            return other;
        }

        /// <summary>How much a drag released now would move: the held modifier's role, or the whole stack.</summary>
        public ItemTransferQuantity QuantityForDrag() => _modifiers.HeldQuantity() ?? ItemTransferQuantity.All;

        /// <summary>
        /// What releasing the stack in <paramref name="fromSlot"/> over the other panel would do. Nothing moves while a
        /// find waits for room on the ground.
        /// </summary>
        public GridDropOutcome Resolve(ItemPanel from, int fromSlot, int hoveredSlot, ItemTransferQuantity quantity)
        {
            if (_session.HasPendingGroundOverflow)
            {
                return GridDropOutcome.ReturnToPlace;
            }

            return ItemMoveDropOutcomeFactory.Create(_session.PreviewItemMove(from, fromSlot, hoveredSlot, quantity), hoveredSlot);
        }

        /// <summary>Carries out a move <see cref="Resolve"/> previewed as possible.</summary>
        public void Apply(ItemPanel from, int fromSlot, int hoveredSlot, ItemTransferQuantity quantity)
        {
            _session.MoveItems(from, fromSlot, hoveredSlot, quantity);
        }

        /// <summary>
        /// A modified click: sends the held modifier's amount of the stack straight to the other panel. Returns false,
        /// changing nothing, when no modifier is held, so the click does its usual thing.
        /// </summary>
        public bool TrySendToOtherPanel(ItemPanel from, int fromSlot)
        {
            ItemTransferQuantity? quantity = _modifiers.HeldQuantity();
            if (quantity == null)
            {
                return false;
            }

            GridDropOutcome outcome = Resolve(from, fromSlot, -1, quantity.Value);
            if (outcome.Kind == GridDropOutcomeKind.Transfer)
            {
                Apply(from, fromSlot, -1, quantity.Value);
            }
            else if (outcome.Kind == GridDropOutcomeKind.NoRoom)
            {
                _views[from].ShowBriefMessage(outcome, fromSlot);
            }

            return true;
        }

        private static ItemPanel Other(ItemPanel panel) => panel == ItemPanel.Inventory ? ItemPanel.Ground : ItemPanel.Inventory;
    }
}
