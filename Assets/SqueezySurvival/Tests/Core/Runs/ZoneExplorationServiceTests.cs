using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Items;
using SqueezySurvival.Core.Tests.Randomness;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class ZoneExplorationServiceTests
    {
        private static readonly ItemDefinition Log = TestItemDefinitions.Log;

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
        public void Explore_CompletesWithTheExplorersMinutesAndLogsTheFinds()
        {
            var rules = new ExplorationRules(exploreMinutes: 25, minimumFindsPerExplore: 1, maximumFindsPerExplore: 1, featureChancePercent: 0);
            var completion = new RecordingSurvivorActionCompletionService();
            var zone = new Zone(TestZoneDefinitions.Create(4), SlotCapacityRules.Wireframe);
            var groundInventory = new GroundInventoryService(zone, new GameTimeAdvancer(GameClock.RunStart, System.Array.Empty<ITimeDependentSystem>()), new RunEventLog());
            var exploration = new ZoneExplorationService(
                zone,
                new ZoneExplorer(rules, new QueuedRandomNumberSource(1, 0, 99)),
                new SurvivorInventoryService(SlotCapacityRules.Wireframe),
                groundInventory,
                completion);

            exploration.Explore();

            Assert.That(completion.Completed[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(25)));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { "You explored the Cove and found a pebble." }));
        }

        [Test]
        public void Explore_WhenActionsCannotStart_ExploresNothing()
        {
            var completion = new RecordingSurvivorActionCompletionService { CanStartAction = false };
            var zone = new Zone(TestZoneDefinitions.Create(4), SlotCapacityRules.Wireframe);
            var groundInventory = new GroundInventoryService(zone, new GameTimeAdvancer(GameClock.RunStart, System.Array.Empty<ITimeDependentSystem>()), new RunEventLog());
            var exploration = new ZoneExplorationService(
                zone,
                new ZoneExplorer(ExplorationRules.Wireframe, new QueuedRandomNumberSource(1, 0, 99)),
                new SurvivorInventoryService(SlotCapacityRules.Wireframe),
                groundInventory,
                completion);

            Assert.That(exploration.CanExplore, Is.False);
            Assert.That(() => exploration.Explore(), Throws.InvalidOperationException);
            Assert.That(zone.ExplorationProgress.ExploresCompleted, Is.Zero);
        }

        [Test]
        public void Explore_ScriptedExplore_PutsTheScriptedFindsOnTheGround()
        {
            ZoneDefinition cove = TestZoneDefinitions.Create(4, new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Pebble, TestZoneDefinitions.Twig });
            RunSession session = TestRunSessions.Start(cove, System.Array.Empty<ITimeDependentSystem>());

            session.ZoneExploration.Explore();

            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Pebble), Is.EqualTo(2));
            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Twig), Is.EqualTo(1));
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
