using System;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class GroundInventoryServiceTests
    {
        [Test]
        public void LeavePendingOverflowBehind_DropsTheFindLogsItIsGoneAndTakesNoTime()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();
            long secondsBefore = session.Clock.ElapsedSeconds;

            session.GroundInventory.LeavePendingOverflowBehind();

            Assert.That(session.GroundInventory.HasPendingOverflow, Is.False);
            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Pebble), Is.Zero);
            Assert.That(session.EventLog.Entries.Last().Text, Is.EqualTo("You left the pebble behind. It's gone for good."));
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(secondsBefore));
        }

        [Test]
        public void DiscardToMakeRoomForPendingOverflow_ReplacesTheChosenSlotAndLogsIt()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();

            session.GroundInventory.DiscardToMakeRoomForPendingOverflow(5);

            Assert.That(session.GroundInventory.HasPendingOverflow, Is.False);
            Assert.That(session.CurrentZone.GroundPool.EntryAtSlot(5).Definition, Is.SameAs(TestZoneDefinitions.Pebble));
            Assert.That(session.EventLog.Entries.Last().Text, Is.EqualTo("You threw away the filler 5 to make room for the pebble. It's gone for good."));
        }

        [Test]
        public void ResolvingWithNothingWaiting_IsRefused()
        {
            RunSession session = TestRunSessions.Start();

            Assert.That(() => session.GroundInventory.LeavePendingOverflowBehind(), Throws.InvalidOperationException);
            Assert.That(() => session.GroundInventory.DiscardToMakeRoomForPendingOverflow(0), Throws.InvalidOperationException);
        }

        [Test]
        public void QueueOverflow_Nothing_RaisesNothing()
        {
            RunSession session = TestRunSessions.Start();
            int changes = 0;
            session.GroundInventory.PendingOverflowChanged += () => changes++;

            session.GroundInventory.QueueOverflow(Array.Empty<CountedThing<ItemDefinition>>());

            Assert.That(changes, Is.Zero);
            Assert.That(session.GroundInventory.HasPendingOverflow, Is.False);
        }
    }
}
