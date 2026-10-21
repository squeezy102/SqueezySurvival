using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
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
    }
}
