using System;
using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Items;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class InventoryTransferServiceTests
    {
        private static readonly ItemDefinition Stone = TestItemDefinitions.Stone;
        private static readonly ItemDefinition Log = TestItemDefinitions.Log;

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
        public void Transfer_TakesTheSecondsItsMoveTimeRulesGive()
        {
            var completion = new RecordingSurvivorActionCompletionService();
            var survivorInventory = new SurvivorInventoryService(SlotCapacityRules.Wireframe);
            var zone = new Zone(TestZoneDefinitions.Create(4), SlotCapacityRules.Wireframe);
            var groundInventory = new GroundInventoryService(zone, new GameTimeAdvancer(GameClock.RunStart, Array.Empty<ITimeDependentSystem>()), new RunEventLog());
            var transfers = new InventoryTransferService(survivorInventory, groundInventory, completion, new ItemMoveTimeRules(secondsPerPound: 3m), new ItemArrivalStackingSetting());
            groundInventory.Ground.Receive(Stone, 5, -1);

            transfers.Transfer(ItemPanel.Ground, TestRunSessions.SlotOf(groundInventory.Ground, Stone), -1, ItemTransferQuantity.All);

            int expectedSeconds = new ItemMoveTimeRules(secondsPerPound: 3m).SecondsToMove(Stone.WeightPounds * 5);
            Assert.That(completion.Completed[0].Duration, Is.EqualTo(ActionDuration.FromSeconds(expectedSeconds)));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { "You picked up 5 stones." }));
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

        private static InventoryTransferService CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
            IItemArrivalStackingSetting setting,
            out ISurvivorInventoryService survivorInventory,
            out IGroundInventoryService groundInventory)
        {
            var zone = new Zone(TestZoneDefinitions.Create(4), SlotCapacityRules.Wireframe);
            survivorInventory = new SurvivorInventoryService(SlotCapacityRules.Wireframe);
            groundInventory = new GroundInventoryService(zone, new GameTimeAdvancer(GameClock.RunStart, Array.Empty<ITimeDependentSystem>()), new RunEventLog());
            survivorInventory.Inventory.Receive(Stone, 12, 5);
            groundInventory.Ground.Receive(Stone, 5, 0);
            return new InventoryTransferService(
                survivorInventory,
                groundInventory,
                new RecordingSurvivorActionCompletionService(),
                ItemMoveTimeRules.Wireframe,
                setting);
        }

        [Test]
        public void Transfer_AutoStackOverAnEmptySlot_JoinsTheKindsStack()
        {
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                new ItemArrivalStackingSetting(), out ISurvivorInventoryService inventory, out _);

            transfers.Transfer(ItemPanel.Ground, 0, 1, ItemTransferQuantity.All);

            Assert.That(inventory.Inventory.EntryAtSlot(5).Count, Is.EqualTo(17));
            Assert.That(inventory.Inventory.EntryAtSlot(1), Is.Null);
        }

        [Test]
        public void Transfer_StartOwnStackOverAnEmptySlot_TakesThatSlotAndLeavesTheKindsStackAlone()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                setting, out ISurvivorInventoryService inventory, out _);

            transfers.Transfer(ItemPanel.Ground, 0, 1, ItemTransferQuantity.All);

            Assert.That(inventory.Inventory.EntryAtSlot(1).Count, Is.EqualTo(5));
            Assert.That(inventory.Inventory.EntryAtSlot(5).Count, Is.EqualTo(12));
        }

        [Test]
        public void Transfer_StartOwnStackOverTheKindsStack_StillJoinsIt()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                setting, out ISurvivorInventoryService inventory, out _);

            transfers.Transfer(ItemPanel.Ground, 0, 5, ItemTransferQuantity.All);

            Assert.That(inventory.Inventory.EntryAtSlot(5).Count, Is.EqualTo(17));
        }

        [Test]
        public void Transfer_StartOwnStackWithNoSlotChosen_TakesTheFirstEmptySlot()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                setting, out ISurvivorInventoryService inventory, out _);

            transfers.Transfer(ItemPanel.Ground, 0, -1, ItemTransferQuantity.One);

            Assert.That(inventory.Inventory.EntryAtSlot(0).Count, Is.EqualTo(1));
            Assert.That(inventory.Inventory.EntryAtSlot(5).Count, Is.EqualTo(12));
        }

        [Test]
        public void PreviewTransfer_StartOwnStackOverAnEmptySlot_IsANewStackNotAStack()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(setting, out _, out _);

            ItemMovePreview preview = transfers.PreviewTransfer(ItemPanel.Ground, 0, 1, ItemTransferQuantity.All);

            Assert.That(preview.DestinationSlot, Is.EqualTo(1));
            Assert.That(preview.JoinsStack, Is.False);
            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Pick up"));
        }

        [Test]
        public void PreviewTransfer_StartOwnStackOverTheKindsStack_JoinsItAndNamesItsCount()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(setting, out _, out _);

            ItemMovePreview preview = transfers.PreviewTransfer(ItemPanel.Ground, 0, 5, ItemTransferQuantity.All);

            Assert.That(preview.JoinsStack, Is.True);
            Assert.That(preview.AlreadyThereCount, Is.EqualTo(12));
        }

        [Test]
        public void Transfer_ReadsTheSettingAtEachMove()
        {
            var setting = new ItemArrivalStackingSetting { Stacking = ItemArrivalStacking.StartOwnStack };
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                setting, out ISurvivorInventoryService inventory, out _);
            transfers.Transfer(ItemPanel.Ground, 0, 1, ItemTransferQuantity.One);

            setting.Stacking = ItemArrivalStacking.AutoStack;
            transfers.Transfer(ItemPanel.Ground, 0, 2, ItemTransferQuantity.One);

            Assert.That(inventory.Inventory.EntryAtSlot(1).Count, Is.EqualTo(2));
            Assert.That(inventory.Inventory.EntryAtSlot(2), Is.Null);
            Assert.That(inventory.Inventory.EntryAtSlot(5).Count, Is.EqualTo(12));
        }

        [Test]
        public void PreviewTransfer_KindSplitAcrossStacks_NamesTheCountOfTheStackItJoins()
        {
            InventoryTransferService transfers = CreateTransfersWithStoneOnGroundAndInInventorySlotFive(
                new ItemArrivalStackingSetting(), out ISurvivorInventoryService inventory, out _);
            inventory.Inventory.SplitToSlot(5, 3, 4);

            ItemMovePreview preview = transfers.PreviewTransfer(ItemPanel.Ground, 0, -1, ItemTransferQuantity.One);

            Assert.That(preview.DestinationSlot, Is.EqualTo(3));
            Assert.That(preview.AlreadyThereCount, Is.EqualTo(4));
        }
    }
}
