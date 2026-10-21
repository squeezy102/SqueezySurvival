using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class ZoneExplorationServiceTests
    {
        private static readonly ItemDefinition Log = PlaceholderBeachZoneContent.Log;

        [Test]
        public void Explore_AdvancesFifteenMinutes()
        {
            RunSession session = TestRunSessions.Start(1, 0, 99);

            session.ZoneExploration.Explore();

            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(15));
        }

        [Test]
        public void Explore_AddsFindsToCurrentZoneAndLogsThemAtTheEndOfTheExplore()
        {
            RunSession session = TestRunSessions.Start(2, 1, 1, 99);

            session.ZoneExploration.Explore();

            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Twig), Is.EqualTo(2));
            Assert.That(session.EventLog.Entries[1].Text, Is.EqualTo("You explored the Cove and found 2 twigs."));
            Assert.That(session.EventLog.Entries[1].Time.ElapsedSeconds, Is.EqualTo(session.Clock.ElapsedSeconds));
        }

        [Test]
        public void Explore_TellsTimeDependentSystemsTheExploreMinutes()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = TestRunSessions.Start(TestZoneDefinitions.Create(4), new ITimeDependentSystem[] { system }, 1, 0, 99);

            session.ZoneExploration.Explore();

            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(session.ZoneExploration.ExploreMinutes)));
        }

        [Test]
        public void Explore_PlaceholderBeach_FirstExploresFindStonesSticksAndSeaweed()
        {
            RunSession session = TestRunSessions.Start(PlaceholderBeachZoneContent.Beach, System.Array.Empty<ITimeDependentSystem>());

            session.ZoneExploration.Explore();
            session.ZoneExploration.Explore();

            Zone beach = session.CurrentZone;
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Stone), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.SmallStick), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Seaweed), Is.GreaterThan(0));
        }

        [Test]
        public void Explore_GroundFull_FindWaitsForRoomAndIsAnnouncedOnce()
        {
            int changes = 0;
            RunSession session = TestRunSessions.Start(1, 0, 99);
            session.GroundInventory.PendingOverflowChanged += () => changes++;
            session.GroundInventory.Ground.AddRange(TestZoneDefinitions.DistinctItems(session.GroundInventory.Ground.SlotCapacity));

            session.ZoneExploration.Explore();

            Assert.That(session.GroundInventory.PendingOverflow.Select(thing => thing.Definition), Is.EqualTo(new[] { TestZoneDefinitions.Pebble }));
            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void Explore_WhileAFindWaitsForRoom_IsUnavailableAndRefused()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();
            long secondsBefore = session.Clock.ElapsedSeconds;

            Assert.That(session.ZoneExploration.CanExplore, Is.False);
            Assert.That(() => session.ZoneExploration.Explore(), Throws.InvalidOperationException);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(secondsBefore));
        }

        [Test]
        public void Explore_WhileImmobile_IsUnavailableAndRefused()
        {
            RunSession session = TestRunSessions.Start();
            session.SurvivorInventory.Inventory.Receive(Log, 4, -1);

            Assert.That(session.ZoneExploration.CanExplore, Is.False);
            Assert.That(() => session.ZoneExploration.Explore(), Throws.InvalidOperationException);
        }

        [Test]
        public void CanExplore_OverburdenedJustUnderTheLimit_IsTrue()
        {
            RunSession session = TestRunSessions.Start();
            session.SurvivorInventory.Inventory.Receive(Log, 3, -1);

            Assert.That(session.SurvivorInventory.Load.State, Is.EqualTo(SurvivorLoadState.Overburdened));
            Assert.That(session.ZoneExploration.CanExplore, Is.True);
        }
    }
}
