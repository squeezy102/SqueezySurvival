using NUnit.Framework;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Randomness;
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
            new RunSession(
                new NewRunConfiguration(DominantHand.Right, RunOptions.Standard),
                DayPhaseSchedule.Default,
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
        public void PassTime_RaisesClockAdvanced()
        {
            RunSession session = CreateSession();
            GameClock raised = null;
            session.ClockAdvanced += clock => raised = clock;

            session.PassTime(TimePassingAction.Wait);

            Assert.That(raised, Is.SameAs(session.Clock));
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
            Assert.That(session.EventLog.Entries[1].Time, Is.SameAs(session.Clock));
        }

        [Test]
        public void Explore_RaisesClockAdvanced()
        {
            RunSession session = CreateSession(1, 0, 99);
            GameClock raised = null;
            session.ClockAdvanced += clock => raised = clock;

            session.Explore();

            Assert.That(raised, Is.SameAs(session.Clock));
        }

        [Test]
        public void Explore_PlaceholderBeach_FirstExploresFindStonesSticksAndSeaweed()
        {
            RunSession session = CreateSession(PlaceholderBeachZoneContent.Beach);

            session.Explore();
            session.Explore();

            Zone beach = session.CurrentZone;
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Stone), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.Stick), Is.GreaterThan(0));
            Assert.That(beach.GroundPool.CountOf(PlaceholderBeachZoneContent.SeaweedStrand), Is.GreaterThan(0));
        }
    }
}
