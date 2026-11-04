using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunSessionFactoryTests
    {
        [Test]
        public void StartNewRun_StartsInUnexploredStartingZoneAtDawnOfDayOne()
        {
            RunSession session = TestRunSessions.Start();

            Assert.That(session.CurrentZone.Name, Is.EqualTo("Cove"));
            Assert.That(session.CurrentZone.ExplorationProgress.ExploresCompleted, Is.Zero);
            Assert.That(session.Clock.Day, Is.EqualTo(1));
            Assert.That(session.CurrentDayPhase, Is.EqualTo(DayPhase.Dawn));
        }

        [Test]
        public void StartNewRun_LogsWakingUpAsTheOnlyEntry()
        {
            RunSession session = TestRunSessions.Start();

            Assert.That(session.EventLog.Entries, Has.Count.EqualTo(1));
            Assert.That(session.EventLog.Entries[0].Text, Is.EqualTo(RunSessionFactory.WakeUpText));
        }

        [Test]
        public void StartNewRun_ServicesShareOneGround()
        {
            RunSession session = TestRunSessions.Start();

            Assert.That(session.GroundInventory.Ground, Is.SameAs(session.CurrentZone.GroundPool));
            Assert.That(session.InventoryTransfers.ContentsOf(ItemPanel.Ground), Is.SameAs(session.CurrentZone.GroundPool));
            Assert.That(session.InventoryTransfers.ContentsOf(ItemPanel.Inventory), Is.SameAs(session.SurvivorInventory.Inventory));
        }

        [Test]
        public void StartNewRun_ActionsAdvanceTheRunClockAndTellTimeDependentSystemsOnce()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = TestRunSessions.Start(TestZoneDefinitions.Create(4), new ITimeDependentSystem[] { system });
            GameTimeAdvance raised = null;
            session.GameTime.TimeAdvanced += (sender, advance) => raised = advance;

            session.TimePassing.PassTime(TimePassingAction.Wait);

            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(15));
            Assert.That(system.Advances, Has.Count.EqualTo(1));
            Assert.That(raised.Current, Is.SameAs(session.Clock));
            Assert.That(session.EventLog.Entries[1].Text, Is.EqualTo("You waited 15 minutes."));
        }
    }
}
