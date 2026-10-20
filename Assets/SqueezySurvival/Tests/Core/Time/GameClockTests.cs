using System;
using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class GameClockTests
    {
        [Test]
        public void RunStart_IsDayOneAtSixAm()
        {
            Assert.That(GameClock.RunStart.Day, Is.EqualTo(1));
            Assert.That(GameClock.RunStart.MinuteOfDay, Is.EqualTo(6 * 60));
        }

        [Test]
        public void Advance_Minutes_PastMidnight_StartsNextDay()
        {
            GameClock clock = GameClock.RunStart.Advance(ActionDuration.FromMinutes(18 * 60));

            Assert.That(clock.Day, Is.EqualTo(2));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(0));
            Assert.That(clock.SecondOfDay, Is.EqualTo(0));
        }

        [Test]
        public void Advance_Seconds_OneSecond_MovesTheClockOneSecondButNotAMinute()
        {
            GameClock clock = GameClock.RunStart.Advance(ActionDuration.FromSeconds(1));

            Assert.That(clock.ElapsedSeconds, Is.EqualTo(1));
            Assert.That(clock.ElapsedMinutes, Is.EqualTo(0));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(6 * 60));
        }

        [TestCase(59, 6 * 60, 59)]
        [TestCase(60, 6 * 60 + 1, 0)]
        [TestCase(61, 6 * 60 + 1, 1)]
        public void Advance_Seconds_AroundAWholeMinute_RollsTheMinuteOverAtSixty(int seconds, int expectedMinuteOfDay, int expectedSecondOfMinute)
        {
            GameClock clock = GameClock.RunStart.Advance(ActionDuration.FromSeconds(seconds));

            Assert.That(clock.MinuteOfDay, Is.EqualTo(expectedMinuteOfDay));
            Assert.That(clock.SecondOfDay % GameClock.SecondsPerMinute, Is.EqualTo(expectedSecondOfMinute));
        }

        [Test]
        public void Advance_Seconds_PastMidnight_StartsNextDay()
        {
            GameClock clock = GameClock.RunStart.Advance(ActionDuration.FromMinutes((18 * 60) - 1)).Advance(ActionDuration.FromSeconds(60));

            Assert.That(clock.Day, Is.EqualTo(2));
            Assert.That(clock.SecondOfDay, Is.EqualTo(0));
        }

        [Test]
        public void Advance_NullDuration_Throws()
        {
            Assert.That(() => GameClock.RunStart.Advance(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_WithNegativeElapsed_Throws()
        {
            Assert.That(() => new GameClock(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
