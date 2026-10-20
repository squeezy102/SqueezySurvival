using System;
using NUnit.Framework;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Randomness;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunSessionItemMoveTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Log = PlaceholderBeachZoneContent.Log;

        private static RunSession SessionWithOnTheGround(ItemDefinition definition, int count)
        {
            var session = new RunSession(
                new NewRunConfiguration(DominantHand.Right, RunOptions.Standard),
                DayPhaseSchedule.Default,
                TestZoneDefinitions.Create(4),
                new ZoneExplorer(ExplorationRules.Wireframe, new QueuedRandomNumberSource()));
            session.CurrentZone.GroundPool.Receive(definition, count, -1);
            return session;
        }

        private static int SlotOf(CountedCollection<ItemDefinition> collection, ItemDefinition definition) =>
            collection.SlotToReceive(definition, -1);

        [Test]
        public void MoveItems_WholeStackToTheInventory_MovesItTakesItsSecondsAndLogs()
        {
            RunSession session = SessionWithOnTheGround(Stone, 5);
            int groundSlot = SlotOf(session.CurrentZone.GroundPool, Stone);

            session.MoveItems(ItemPanel.Ground, groundSlot, -1, ItemTransferQuantity.All);

            Assert.That(session.Inventory.CountOf(Stone), Is.EqualTo(5));
            Assert.That(session.CurrentZone.GroundPool.CountOf(Stone), Is.Zero);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(10));
            Assert.That(session.EventLog.Entries[session.EventLog.Entries.Count - 1].Text, Is.EqualTo("You picked up 5 stones."));
        }

        [Test]
        public void MoveItems_RaisesClockAdvancedWithTheNewTime()
        {
            RunSession session = SessionWithOnTheGround(Stone, 1);
            GameClock reported = null;
            session.ClockAdvanced += clock => reported = clock;

            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Stone), -1, ItemTransferQuantity.One);

            Assert.That(reported, Is.SameAs(session.Clock));
        }

        [TestCase(ItemTransferQuantity.Half, 4, 3)]
        [TestCase(ItemTransferQuantity.One, 1, 6)]
        public void MoveItems_PartOfAStack_LeavesTheRestBehind(ItemTransferQuantity quantity, int expectedMoved, int expectedLeft)
        {
            RunSession session = SessionWithOnTheGround(Stone, 7);

            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Stone), -1, quantity);

            Assert.That(session.Inventory.CountOf(Stone), Is.EqualTo(expectedMoved));
            Assert.That(session.CurrentZone.GroundPool.CountOf(Stone), Is.EqualTo(expectedLeft));
        }

        [Test]
        public void MoveItems_BackToTheGround_JoinsTheStackThereAndLogsPuttingDown()
        {
            RunSession session = SessionWithOnTheGround(Stone, 5);
            int groundSlot = SlotOf(session.CurrentZone.GroundPool, Stone);
            session.MoveItems(ItemPanel.Ground, groundSlot, -1, ItemTransferQuantity.Half);

            session.MoveItems(ItemPanel.Inventory, SlotOf(session.Inventory, Stone), -1, ItemTransferQuantity.One);

            Assert.That(session.CurrentZone.GroundPool.EntryAtSlot(groundSlot).Count, Is.EqualTo(3));
            Assert.That(session.EventLog.Entries[session.EventLog.Entries.Count - 1].Text, Is.EqualTo("You put a stone on the ground."));
        }

        [Test]
        public void PreviewItemMove_ChangesNothing()
        {
            RunSession session = SessionWithOnTheGround(Stone, 5);

            ItemMovePreview preview = session.PreviewItemMove(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Stone), -1, ItemTransferQuantity.All);

            Assert.That(preview.Count, Is.EqualTo(5));
            Assert.That(preview.Seconds, Is.EqualTo(10));
            Assert.That(session.Inventory.Entries, Is.Empty);
            Assert.That(session.Clock.ElapsedSeconds, Is.Zero);
        }

        [Test]
        public void MoveItems_InventoryFullOfOtherKinds_ThrowsAndChangesNothing()
        {
            RunSession session = SessionWithOnTheGround(Stone, 1);
            for (int slot = 0; slot < session.Inventory.SlotCapacity; slot++)
            {
                session.Inventory.Receive(
                    new ItemDefinition($"filler-{slot}", "Filler", "a filler", "fillers", "Fills a slot.", ItemCategory.Salvage, 0.1m), 1, slot);
            }

            Assert.That(session.PreviewItemMove(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All).CanMove, Is.False);
            Assert.That(() => session.MoveItems(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(session.CurrentZone.GroundPool.CountOf(Stone), Is.EqualTo(1));
            Assert.That(session.Clock.ElapsedSeconds, Is.Zero);
        }

        [Test]
        public void MoveItems_FromAnEmptySlot_Throws()
        {
            RunSession session = SessionWithOnTheGround(Stone, 1);

            Assert.That(() => session.MoveItems(ItemPanel.Inventory, 0, -1, ItemTransferQuantity.All), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Load_CountsTheInventoryAndBothHands()
        {
            RunSession session = SessionWithOnTheGround(Stone, 10);
            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Stone), -1, ItemTransferQuantity.All);
            session.Hands.Receive(Log, 1, 0);

            Assert.That(session.Load.CarriedPounds, Is.EqualTo(40m));
            Assert.That(session.Load.LimitPounds, Is.EqualTo(SurvivorLoad.PlaceholderLimitPounds));
        }

        [Test]
        public void MoveItems_PastTheWeightLimit_IsAllowed()
        {
            RunSession session = SessionWithOnTheGround(Log, 5);

            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Log), -1, ItemTransferQuantity.All);

            Assert.That(session.Load.CarriedPounds, Is.EqualTo(150m));
            Assert.That(session.Load.State, Is.EqualTo(SurvivorLoadState.Immobile));
        }

        [Test]
        public void Explore_WhileImmobile_IsUnavailableAndThrows()
        {
            RunSession session = SessionWithOnTheGround(Log, 4);
            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Log), -1, ItemTransferQuantity.All);

            Assert.That(session.CanExplore, Is.False);
            Assert.That(() => session.Explore(), Throws.InvalidOperationException);
        }

        [Test]
        public void CanExplore_JustUnderTheLimit_IsTrue()
        {
            RunSession session = SessionWithOnTheGround(Log, 3);
            session.MoveItems(ItemPanel.Ground, SlotOf(session.CurrentZone.GroundPool, Log), -1, ItemTransferQuantity.All);

            Assert.That(session.Load.State, Is.EqualTo(SurvivorLoadState.Overburdened));
            Assert.That(session.CanExplore, Is.True);
        }
    }
}
