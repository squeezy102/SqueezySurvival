using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Presentation.Game;
using UnityEngine;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// Connects the ground's and the inventory's grids (Docs/Design/UI/DragAndDrop.md, Between panels). Core decides what
    /// a move does; these tests check the coordinator finds the grid under the pointer, reads the modifier and passes
    /// requests on.
    /// </summary>
    [TestFixture]
    public sealed class ItemPanelMoveCoordinatorTests : InventoryGridUITestFixture
    {
        private const int PebbleCount = 3;

        private RunSession _session;
        private InventoryGridView _groundGrid;
        private InventoryGridView _inventoryGrid;
        private ItemPanelMoveCoordinator _moves;

        private void ComposeMoves(RunSession session, ItemTransferQuantity? heldQuantity)
        {
            _session = session;
            var shapes = new InventoryGridShapes(SlotCapacityRules.Wireframe);
            _groundGrid = CreateGrid(shapes.Ground, "ground-pool-grid", _session.GroundInventory.Ground);
            _inventoryGrid = CreateGrid(shapes.Inventory, "inventory-grid", _session.SurvivorInventory.Inventory);
            _moves = new ItemPanelMoveCoordinator(_session.InventoryTransfers, new FixedMoveModifierReader(heldQuantity));
            _moves.Register(ItemPanel.Ground, _groundGrid);
            _moves.Register(ItemPanel.Inventory, _inventoryGrid);
        }

        private int PebbleSlotOnTheGround => TestRunSessions.SlotOf(_session.GroundInventory.Ground, TestZoneDefinitions.Pebble);

        [Test]
        public void QuantityForDrag_WithNoModifierHeld_IsTheWholeStack()
        {
            ComposeMoves(TestRunSessions.Start(), null);

            Assert.That(_moves.QuantityForDrag(), Is.EqualTo(ItemTransferQuantity.All));
        }

        [Test]
        public void QuantityForDrag_WithAModifierHeld_IsThatModifiersQuantity()
        {
            ComposeMoves(TestRunSessions.Start(), ItemTransferQuantity.Half);

            Assert.That(_moves.QuantityForDrag(), Is.EqualTo(ItemTransferQuantity.Half));
        }

        [Test]
        public void TryFindOtherPanelUnder_OverTheOtherGrid_ReturnsItWithTheSlotUnderThePointer()
        {
            ComposeMoves(TestRunSessions.Start(), null);

            bool found = _moves.TryFindOtherPanelUnder(
                ItemPanel.Ground, _inventoryGrid.SlotWorldBound(2).center, out ItemPanel panel, out InventoryGridView other, out int hoveredSlot);

            Assert.That(found, Is.True);
            Assert.That(panel, Is.EqualTo(ItemPanel.Inventory));
            Assert.That(other, Is.SameAs(_inventoryGrid));
            Assert.That(hoveredSlot, Is.EqualTo(2));
        }

        [Test]
        public void TryFindOtherPanelUnder_OverNeitherGrid_ReturnsNothing()
        {
            ComposeMoves(TestRunSessions.Start(), null);

            bool found = _moves.TryFindOtherPanelUnder(
                ItemPanel.Ground, new Vector2(-10f, -10f), out ItemPanel _, out InventoryGridView other, out int hoveredSlot);

            Assert.That(found, Is.False);
            Assert.That(other, Is.Null);
            Assert.That(hoveredSlot, Is.EqualTo(-1));
        }

        [Test]
        public void TryFindOtherPanelUnder_OverItsOwnGrid_ReturnsNothing()
        {
            ComposeMoves(TestRunSessions.Start(), null);

            bool found = _moves.TryFindOtherPanelUnder(
                ItemPanel.Ground, _groundGrid.SlotWorldBound(0).center, out ItemPanel _, out InventoryGridView other, out int hoveredSlot);

            Assert.That(found, Is.False);
            Assert.That(other, Is.Null);
            Assert.That(hoveredSlot, Is.EqualTo(-1));
        }

        [Test]
        public void Resolve_WhileAFindWaitsForRoom_ReturnsTheStackToItsPlace()
        {
            ComposeMoves(TestRunSessions.WithPebbleWaitingForRoom(), null);

            GridDropOutcome outcome = _moves.Resolve(ItemPanel.Ground, ItemPanel.Inventory, 0, -1, ItemTransferQuantity.All);

            Assert.That(outcome, Is.SameAs(GridDropOutcome.ReturnToPlace));
        }

        [Test]
        public void Resolve_WithRoomInTheOtherPanel_IsATransfer()
        {
            ComposeMoves(TestRunSessions.WithOnTheGround(TestZoneDefinitions.Pebble, PebbleCount), null);

            GridDropOutcome outcome = _moves.Resolve(ItemPanel.Ground, ItemPanel.Inventory, PebbleSlotOnTheGround, -1, ItemTransferQuantity.All);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Transfer));
        }

        [Test]
        public void Apply_MovesTheStackToTheOtherPanel()
        {
            ComposeMoves(TestRunSessions.WithOnTheGround(TestZoneDefinitions.Pebble, PebbleCount), null);

            _moves.Apply(ItemPanel.Ground, ItemPanel.Inventory, PebbleSlotOnTheGround, -1, ItemTransferQuantity.All);

            Assert.That(_session.GroundInventory.Ground.CountOf(TestZoneDefinitions.Pebble), Is.Zero);
            Assert.That(_session.SurvivorInventory.Inventory.CountOf(TestZoneDefinitions.Pebble), Is.EqualTo(PebbleCount));
        }

        [Test]
        public void TrySendModifiedClick_WithNoModifierHeld_ReturnsFalseAndMovesNothing()
        {
            ComposeMoves(TestRunSessions.WithOnTheGround(TestZoneDefinitions.Pebble, PebbleCount), null);

            bool handled = _moves.TrySendModifiedClick(ItemPanel.Ground, PebbleSlotOnTheGround);

            Assert.That(handled, Is.False);
            Assert.That(_session.GroundInventory.Ground.CountOf(TestZoneDefinitions.Pebble), Is.EqualTo(PebbleCount));
        }

        [Test]
        public void TrySendModifiedClick_WithAModifierHeldAndNoRoom_IsHandledByTellingThePlayerAndMovesNothing()
        {
            RunSession session = TestRunSessions.WithOnTheGround(TestZoneDefinitions.Pebble, PebbleCount);
            session.SurvivorInventory.Inventory.AddRange(TestZoneDefinitions.DistinctItems(session.SurvivorInventory.Inventory.SlotCapacity));
            ComposeMoves(session, ItemTransferQuantity.All);

            bool handled = _moves.TrySendModifiedClick(ItemPanel.Ground, PebbleSlotOnTheGround);

            Assert.That(handled, Is.True);
            Assert.That(Tooltip.IsShowingBriefMessage, Is.True);
            Assert.That(_session.GroundInventory.Ground.CountOf(TestZoneDefinitions.Pebble), Is.EqualTo(PebbleCount));
        }

        [Test]
        public void PanelContentsRevision_ChangesWhenEitherPanelChanges()
        {
            ComposeMoves(TestRunSessions.Start(), null);
            long before = _moves.PanelContentsRevision;
            _session.GroundInventory.Ground.Receive(TestZoneDefinitions.Pebble, 1, -1);
            long afterGround = _moves.PanelContentsRevision;
            _session.SurvivorInventory.Inventory.Receive(TestZoneDefinitions.Pebble, 1, -1);

            Assert.That(afterGround, Is.Not.EqualTo(before));
            Assert.That(_moves.PanelContentsRevision, Is.Not.EqualTo(afterGround));
        }

        [Test]
        public void TryFindOtherPanelUnder_OverTheGridOfARegisteredContainer_ReturnsThatContainersPanel()
        {
            ComposeMoves(TestRunSessions.Start(), null);
            ItemPanel basket = ItemPanel.ForContainer("basket");
            var shapes = new InventoryGridShapes(SlotCapacityRules.Wireframe);
            InventoryGridView basketGrid = CreateGrid(
                shapes.ZoneFeatures, "zone-features-grid", new CountedCollection<ItemDefinition>(shapes.ZoneFeatures.SlotCount));
            _moves.Register(basket, basketGrid);

            bool found = _moves.TryFindOtherPanelUnder(
                ItemPanel.Ground, basketGrid.SlotWorldBound(1).center, out ItemPanel panel, out InventoryGridView view, out int hoveredSlot);

            Assert.That(found, Is.True);
            Assert.That(panel, Is.EqualTo(basket));
            Assert.That(view, Is.SameAs(basketGrid));
            Assert.That(hoveredSlot, Is.EqualTo(1));
        }
    }
}
