using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class TimePassingServiceTests
    {
        [Test]
        public void PassTime_Wait_AdvancesFifteenMinutesAndLogs()
        {
            RunSession session = TestRunSessions.Start();

            session.TimePassing.PassTime(TimePassingAction.Wait);

            Assert.That(session.Clock.ElapsedMinutes, Is.EqualTo(15));
            Assert.That(session.EventLog.Entries[1].Text, Is.EqualTo("You waited 15 minutes."));
        }

        [Test]
        public void PassTime_Rest_AdvancesOneHourIntoMorning()
        {
            RunSession session = TestRunSessions.Start();

            session.TimePassing.PassTime(TimePassingAction.Rest);

            Assert.That(session.Clock.MinuteOfDay, Is.EqualTo(7 * 60));
            Assert.That(session.CurrentDayPhase, Is.EqualTo(DayPhase.Morning));
        }

        [Test]
        public void PassTime_Wait_TellsTimeDependentSystemsFifteenMinutes()
        {
            var system = new RecordingTimeDependentSystem();
            RunSession session = TestRunSessions.Start(TestZoneDefinitions.Create(4), new ITimeDependentSystem[] { system });

            session.TimePassing.PassTime(TimePassingAction.Wait);

            Assert.That(system.Advances, Has.Count.EqualTo(1));
            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(15)));
        }

        [Test]
        public void PassTime_RaisesTimeAdvancedWithTheNewClock()
        {
            RunSession session = TestRunSessions.Start();
            GameTimeAdvance raised = null;
            session.GameTime.TimeAdvanced += (sender, advance) => raised = advance;

            session.TimePassing.PassTime(TimePassingAction.Wait);

            Assert.That(raised.Current, Is.SameAs(session.Clock));
        }

        [Test]
        public void PassTime_WhileAFindWaitsForRoom_IsUnavailableAndRefused()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();
            long secondsBefore = session.Clock.ElapsedSeconds;

            Assert.That(session.TimePassing.CanPassTime, Is.False);
            Assert.That(() => session.TimePassing.PassTime(TimePassingAction.Wait), Throws.InvalidOperationException);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(secondsBefore));
        }
    }
}
