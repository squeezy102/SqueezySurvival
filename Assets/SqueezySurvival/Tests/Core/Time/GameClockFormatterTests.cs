using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class GameClockFormatterTests
    {
        // Seconds after the 6:00 AM run start: midnight, a morning time with seconds, noon, and the last second of the day.
        [TestCase(18 * 3600, "12:00:00 AM")]
        [TestCase((1 * 3600) + (45 * 60) + 20, "7:45:20 AM")]
        [TestCase(6 * 3600, "12:00:00 PM")]
        [TestCase((18 * 3600) - 1, "11:59:59 PM")]
        public void FormatTimeOfDay_UsesTwelveHourClockWithSeconds(long secondsAfterRunStart, string expected)
        {
            Assert.That(GameClockFormatter.FormatTimeOfDay(new GameClock(secondsAfterRunStart)), Is.EqualTo(expected));
        }

        [Test]
        public void FormatTimeOfDay_Null_IsRefused()
        {
            Assert.That(() => GameClockFormatter.FormatTimeOfDay(null), Throws.ArgumentNullException);
        }

        [Test]
        public void FormatHudClockRows_AtRunStart_ShowsDayTimeAndPhaseOnePerRow()
        {
            Assert.That(GameClockFormatter.FormatHudClockRows(GameClock.RunStart, DayPhaseSchedule.Default),
                Is.EqualTo(new[] { "Day 1", "6:00:00 AM", "Dawn" }));
        }

        [TestCase(15, "15 min")]
        [TestCase(60, "1 hr")]
        [TestCase(90, "1 hr 30 min")]
        public void FormatDuration_UsesHoursAndMinutes(int minutes, string expected)
        {
            Assert.That(GameClockFormatter.FormatDuration(minutes), Is.EqualTo(expected));
        }

        [TestCase(1, "1 sec")]
        [TestCase(59, "59 sec")]
        [TestCase(60, "1 min")]
        [TestCase(65, "1 min 5 sec")]
        public void FormatDurationSeconds_UsesSecondsThenMinutes(int seconds, string expected)
        {
            Assert.That(GameClockFormatter.FormatDurationSeconds(seconds), Is.EqualTo(expected));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void FormatDurationSeconds_BelowOneSecond_IsRefused(int seconds)
        {
            Assert.That(() => GameClockFormatter.FormatDurationSeconds(seconds), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }
    }
}
