using System;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Randomness;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunSessionTests
    {
        private static RunSession CreateSession(params int[] randomNumbers) => CreateSession(TestZoneDefinitions.Create(4), randomNumbers);

        private static RunSession CreateSession(ZoneDefinition startingZone, params int[] randomNumbers) =>
            CreateSession(startingZone, Array.Empty<ITimeDependentSystem>(), randomNumbers);

        private static RunSession CreateSession(ZoneDefinition startingZone, ITimeDependentSystem[] timeDependentSystems, params int[] randomNumbers) =>
            new RunSession(
                new NewRunConfiguration(DominantHand.Right, RunOptions.Standard),
                DayPhaseSchedule.Default,
                new GameTimeAdvancer(GameClock.RunStart, timeDependentSystems),
                startingZone,
                new ZoneExplorer(ExplorationRules.Wireframe, new QueuedRandomNumberSource(randomNumbers)));

        [Test]
        public void NewSession_StartsInUnexploredStartingZoneAtDawnOfDayOne()
        {
            RunSession session = CreateSession();

            Assert.That(session.CurrentZone.Name, Is.EqualTo("Cove"));
            Assert.That(session.CurrentZone.ExplorationProgress.ExploresCompleted, Is.Zero);
            Assert.That(session.Clock.Day, Is.EqualTo(1));
            Assert.That(session.CurrentDayPhase, Is.EqualTo(DayPhase.Dawn));
        }

        [Test]
        public void NewSession_LogsWakingUp()
        {
            Assert.That(CreateSession().EventLog.Entries[0].Text, Is.EqualTo(RunSession.WakeUpText));
        }

        [Test]
        public void PassTime_Wait_AdvancesFifteenMinutesAndLogs()
        {
            RunSession session = CreateSession();

            session.PassTime(TimePassingAction.Wait);

            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(15));
            Assert.That(session.EventLog.Entries[1].Text, Is.EqualTo("You waited 15 minutes."));
        }

        [Test]
        public void PassTime_Rest_AdvancesOneHourIntoMorning()
        {
            RunSession session = CreateSession();

            session.PassTime(TimePassingAction.Rest);

            Assert.That(session.Clock.MinuteOfDay, Is.EqualTo(7 * 60));
            Assert.That(session.CurrentDayPhase, Is.EqualTo(DayPhase.Morning));
        }

        [Test]
        public void PassTime_Wait_TellsTimeDependentSystemsFifteenMinutes()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = CreateSession(TestZoneDefinitions.Create(4), new ITimeDependentSystem[] { system });

            session.PassTime(TimePassingAction.Wait);

            Assert.That(system.Advances, Has.Count.EqualTo(1));
            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(15)));
        }

        [Test]
        public void PassTime_RaisesTimeAdvancedWithTheNewClock()
        {
            RunSession session = CreateSession();
            GameTimeAdvance raised = null;
            session.GameTime.TimeAdvanced += (sender, advance) => raised = advance;

            session.PassTime(TimePassingAction.Wait);

            Assert.That(raised.Current, Is.SameAs(session.Clock));
        }

        [Test]
        public void Explore_AdvancesFifteenMinutes()
        {
            RunSession session = CreateSession(1, 0, 99);

            session.Explore();

            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(15));
        }

        [Test]
        public void Explore_AddsFindsToCurrentZoneAndLogsThem()
        {
            RunSession session = CreateSession(2, 1, 1, 99);

            session.Explore();

            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Twig), Is.EqualTo(2));
            Assert.That(session.EventLog.Entries[1].Text, Is.EqualTo("You explored the Cove and found 2 twigs."));
            Assert.That(session.EventLog.Entries[1].Time.ElapsedSeconds, Is.EqualTo(session.Clock.ElapsedSeconds));
        }

        [Test]
        public void Explore_TellsTimeDependentSystemsTheExploreMinutes()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = CreateSession(TestZoneDefinitions.Create(4), new ITimeDependentSystem[] { system }, 1, 0, 99);

            session.Explore();

            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(session.ExploreMinutes)));
        }

        [Test]
        public void Explore_PlaceholderBeach_FirstExploresFindStonesSticksAndSeaweed()
        {
            RunSession session = CreateSession(PlaceholderBeachZoneContent.Beach);

            session.Explore();
            session.Explore();

            Zone beach = session.CurrentZone;
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Stone), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.SmallStick), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Seaweed), Is.GreaterThan(0));
        }

        // One find, a pebble, and no feature.
        private static RunSession SessionWithPebbleWaitingForRoom()
        {
            RunSession session = CreateSession(1, 0, 99);
            session.CurrentZone.GroundPool.AddRange(TestZoneDefinitions.DistinctItems(session.CurrentZone.GroundPool.SlotCapacity));
            session.Explore();
            return session;
        }

        [Test]
        public void Explore_GroundFull_FindWaitsForRoom()
        {
            int changes = 0;
            RunSession session = CreateSession(1, 0, 99);
            session.PendingGroundOverflowChanged += () => changes++;
            session.CurrentZone.GroundPool.AddRange(TestZoneDefinitions.DistinctItems(session.CurrentZone.GroundPool.SlotCapacity));

            session.Explore();

            Assert.That(session.PendingGroundOverflow.Select(thing => thing.Definition), Is.EqualTo(new[] { TestZoneDefinitions.Pebble }));
            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void WhileAFindWaitsForRoom_TimeTakingActionsAreRefused()
        {
            RunSession session = SessionWithPebbleWaitingForRoom();
            long minutesBefore = session.Clock.ElapsedMinutes;

            Assert.That(() => session.Explore(), Throws.InvalidOperationException);
            Assert.That(() => session.PassTime(TimePassingAction.Wait), Throws.InvalidOperationException);
            Assert.That(() => session.MoveItems(ItemPanel.Ground, 0, -1, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(minutesBefore));
        }

        [Test]
        public void LeavePendingGroundOverflowBehind_DropsTheFindAndLogsItIsGone()
        {
            RunSession session = SessionWithPebbleWaitingForRoom();

            session.LeavePendingGroundOverflowBehind();

            Assert.That(session.HasPendingGroundOverflow, Is.False);
            Assert.That(session.CurrentZone.GroundPool.CountOf(TestZoneDefinitions.Pebble), Is.Zero);
            Assert.That(session.EventLog.Entries.Last().Text, Is.EqualTo("You left the pebble behind. It's gone for good."));
        }

        [Test]
        public void DiscardToMakeRoomForPendingGroundOverflow_ReplacesTheChosenSlotAndLogsIt()
        {
            RunSession session = SessionWithPebbleWaitingForRoom();

            session.DiscardToMakeRoomForPendingGroundOverflow(5);

            Assert.That(session.HasPendingGroundOverflow, Is.False);
            Assert.That(session.CurrentZone.GroundPool.EntryAtSlot(5).Definition, Is.SameAs(TestZoneDefinitions.Pebble));
            Assert.That(session.EventLog.Entries.Last().Text, Is.EqualTo("You threw away the filler 5 to make room for the pebble. It's gone for good."));
        }

        [Test]
        public void ResolvingWithNothingWaiting_IsRefused()
        {
            RunSession session = CreateSession();

            Assert.That(() => session.LeavePendingGroundOverflowBehind(), Throws.InvalidOperationException);
            Assert.That(() => session.DiscardToMakeRoomForPendingGroundOverflow(0), Throws.InvalidOperationException);
        }
    }
}
