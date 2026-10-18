using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class GameClockFormatterTests
    {
        [TestCase(0, "12:00 AM")]
        [TestCase(7 * 60 + 45, "7:45 AM")]
        [TestCase(12 * 60, "12:00 PM")]
        [TestCase(23 * 60 + 5, "11:05 PM")]
        public void FormatTimeOfDay_UsesTwelveHourClock(int minuteOfDay, string expected)
        {
            Assert.That(GameClockFormatter.FormatTimeOfDay(minuteOfDay), Is.EqualTo(expected));
        }

        [Test]
        public void FormatHudClockRows_AtRunStart_ShowsDayTimeAndPhaseOnePerRow()
        {
            Assert.That(GameClockFormatter.FormatHudClockRows(GameClock.RunStart, DayPhaseSchedule.Default),
                Is.EqualTo(new[] { "Day 1", "6:00 AM", "Dawn" }));
        }

        [TestCase(15, "15 min")]
        [TestCase(60, "1 hr")]
        [TestCase(90, "1 hr 30 min")]
        public void FormatDuration_UsesHoursAndMinutes(int minutes, string expected)
        {
            Assert.That(GameClockFormatter.FormatDuration(minutes), Is.EqualTo(expected));
        }
    }
}
