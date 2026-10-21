using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class EventLogEntryTextFormatterTests
    {
        [Test]
        public void Format_ShowsDayTimeOfDayAndText()
        {
            var entry = new EventLogEntry(new GameClock(260), "You picked up 5 stones.");

            Assert.That(EventLogEntryTextFormatter.Format(entry), Is.EqualTo("Day 1, 6:04:20 AM: You picked up 5 stones."));
        }

        [Test]
        public void Format_OnALaterDay_ShowsThatDay()
        {
            var entry = new EventLogEntry(new GameClock(GameClock.SecondsPerDay), "You waited 15 minutes.");

            Assert.That(EventLogEntryTextFormatter.Format(entry), Is.EqualTo("Day 2, 6:00:00 AM: You waited 15 minutes."));
        }

        [Test]
        public void Format_Null_IsRefused()
        {
            Assert.That(() => EventLogEntryTextFormatter.Format(null), Throws.ArgumentNullException);
        }
    }
}
