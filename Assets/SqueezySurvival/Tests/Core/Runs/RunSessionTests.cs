using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunSessionTests
    {
        private static RunSession CreateSession() =>
            new RunSession(new NewRunConfiguration(DominantHand.Right, RunOptions.Standard), DayPhaseSchedule.Default);

        [Test]
        public void NewSession_StartsOnBeachAtDawnOfDayOne()
        {
            RunSession session = CreateSession();

            Assert.That(session.CurrentZoneName, Is.EqualTo("Beach"));
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
    }
}
