using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class InventoryTransferServiceTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Log = PlaceholderBeachZoneContent.Log;

        [Test]
        public void Transfer_WholeStackToTheInventory_MovesItTakesItsSecondsAndLogs()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 5);
            int groundSlot = TestRunSessions.SlotOf(session.GroundInventory.Ground, Stone);

            session.InventoryTransfers.Transfer(ItemPanel.Ground, groundSlot, -1, ItemTransferQuantity.All);

            Assert.That(session.SurvivorInventory.Inventory.CountOf(Stone), Is.EqualTo(5));
            Assert.That(session.GroundInventory.Ground.CountOf(Stone), Is.Zero);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(10));
            Assert.That(session.EventLog.Entries[session.EventLog.Entries.Count - 1].Text, Is.EqualTo("You picked up 5 stones."));
        }

        [Test]
        public void Transfer_TellsTimeDependentSystemsTheMovesSeconds()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 5, system);

            session.InventoryTransfers.Transfer(ItemPanel.Ground, TestRunSessions.SlotOf(session.GroundInventory.Ground, Stone), -1, ItemTransferQuantity.All);

            Assert.That(system.Advances, Has.Count.EqualTo(1));
            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromSeconds(10)));
        }

        [TestCase(ItemTransferQuantity.Half, 4, 3)]
        [TestCase(ItemTransferQuantity.One, 1, 6)]
        public void Transfer_PartOfAStack_LeavesTheRestBehind(ItemTransferQuantity quantity, int expectedMoved, int expectedLeft)
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 7);

            session.InventoryTransfers.Transfer(ItemPanel.Ground, TestRunSessions.SlotOf(session.GroundInventory.Ground, Stone), -1, quantity);

            Assert.That(session.SurvivorInventory.Inventory.CountOf(Stone), Is.EqualTo(expectedMoved));
            Assert.That(session.GroundInventory.Ground.CountOf(Stone), Is.EqualTo(expectedLeft));
        }

        [Test]
        public void Transfer_BackToTheGround_JoinsTheStackThereAndLogsPuttingDown()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 5);
            int groundSlot = TestRunSessions.SlotOf(session.GroundInventory.Ground, Stone);
            session.InventoryTransfers.Transfer(ItemPanel.Ground, groundSlot, -1, ItemTransferQuantity.Half);

            session.InventoryTransfers.Transfer(ItemPanel.Inventory, TestRunSessions.SlotOf(session.SurvivorInventory.Inventory, Stone), -1, ItemTransferQuantity.One);

            Assert.That(session.GroundInventory.Ground.EntryAtSlot(groundSlot).Count, Is.EqualTo(3));
            Assert.That(session.EventLog.Entries[session.EventLog.Entries.Count - 1].Text, Is.EqualTo("You put a stone on the ground."));
        }

        [Test]
        public void PreviewTransfer_ChangesNothing()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 5);

            ItemMovePreview preview = session.InventoryTransfers.PreviewTransfer(
                ItemPanel.Ground, TestRunSessions.SlotOf(session.GroundInventory.Ground, Stone), -1, ItemTransferQuantity.All);

            Assert.That(preview.Count, Is.EqualTo(5));
            Assert.That(preview.Seconds, Is.EqualTo(10));
            Assert.That(session.SurvivorInventory.Inventory.Entries, Is.Empty);
            Assert.That(session.Clock.ElapsedSeconds, Is.Zero);
        }

        [Test]
        public void Transfer_InventoryFullOfOtherKinds_ThrowsAndChangesNothing()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 1);
            CountedCollection<ItemDefinition> inventory = session.SurvivorInventory.Inventory;
            for (int slot = 0; slot < inventory.SlotCapacity; slot++)
            {
                inventory.Receive(new ItemDefinition($"filler-{slot}", "Filler", "a filler", "fillers", "Fills a slot.", ItemCategory.Salvage, 0.1m), 1, slot);
            }

            Assert.That(session.InventoryTransfers.PreviewTransfer(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All).CanMove, Is.False);
            Assert.That(() => session.InventoryTransfers.Transfer(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(session.GroundInventory.Ground.CountOf(Stone), Is.EqualTo(1));
            Assert.That(session.Clock.ElapsedSeconds, Is.Zero);
        }

        [Test]
        public void Transfer_FromAnEmptySlot_Throws()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Stone, 1);

            Assert.That(
                () => session.InventoryTransfers.Transfer(ItemPanel.Inventory, 0, -1, ItemTransferQuantity.All),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Transfer_PastTheWeightLimit_IsAllowed()
        {
            RunSession session = TestRunSessions.WithOnTheGround(Log, 5);

            session.InventoryTransfers.Transfer(ItemPanel.Ground, TestRunSessions.SlotOf(session.GroundInventory.Ground, Log), -1, ItemTransferQuantity.All);

            Assert.That(session.SurvivorInventory.Load.CarriedPounds, Is.EqualTo(150m));
            Assert.That(session.SurvivorInventory.Load.State, Is.EqualTo(SurvivorLoadState.Immobile));
        }

        [Test]
        public void Transfer_WhileAFindWaitsForRoom_IsUnavailableAndRefused()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();
            long secondsBefore = session.Clock.ElapsedSeconds;

            Assert.That(session.InventoryTransfers.CanTransfer, Is.False);
            Assert.That(() => session.InventoryTransfers.Transfer(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(secondsBefore));
        }
    }
}
