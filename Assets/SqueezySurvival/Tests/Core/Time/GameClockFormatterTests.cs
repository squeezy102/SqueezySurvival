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
                Is.EqualTo(new[] { "Day 1", "6:00:00 AM", "Dawn" }));
        }

        [Test]
        public void FormatHudClockRows_SecondsIntoAMinute_ShowsTheSeconds()
        {
            GameClock clock = GameClock.RunStart.AdvanceSeconds(134);

            Assert.That(GameClockFormatter.FormatHudClockRows(clock, DayPhaseSchedule.Default)[1], Is.EqualTo("6:02:14 AM"));
        }

        [TestCase(0, "12:00:00 AM")]
        [TestCase(9, "12:00:09 AM")]
        [TestCase((7 * 3600) + (45 * 60) + 12, "7:45:12 AM")]
        [TestCase(12 * 3600, "12:00:00 PM")]
        [TestCase(GameClock.SecondsPerDay - 1, "11:59:59 PM")]
        public void FormatTimeOfDayWithSeconds_UsesTwelveHourClockWithSeconds(int secondOfDay, string expected)
        {
            Assert.That(GameClockFormatter.FormatTimeOfDayWithSeconds(secondOfDay), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(GameClock.SecondsPerDay)]
        public void FormatTimeOfDayWithSeconds_OutsideTheDay_Throws(int secondOfDay)
        {
            Assert.That(() => GameClockFormatter.FormatTimeOfDayWithSeconds(secondOfDay), Throws.TypeOf<System.ArgumentOutOfRangeException>());
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
        [TestCase(80, "1 min 20 sec")]
        [TestCase(3600, "1 hr")]
        [TestCase(3605, "1 hr 5 sec")]
        [TestCase(5400, "1 hr 30 min")]
        [TestCase(3661, "1 hr 1 min 1 sec")]
        public void FormatDurationSeconds_UsesTheLargestUnitsAndLeavesOutZeros(int seconds, string expected)
        {
            Assert.That(GameClockFormatter.FormatDurationSeconds(seconds), Is.EqualTo(expected));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void FormatDurationSeconds_UnderOneSecond_Throws(int seconds)
        {
            Assert.That(() => GameClockFormatter.FormatDurationSeconds(seconds), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }
    }
}
