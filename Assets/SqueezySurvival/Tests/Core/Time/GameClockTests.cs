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
        public void AdvanceMinutes_PastMidnight_StartsNextDay()
        {
            GameClock clock = GameClock.RunStart.AdvanceMinutes(18 * 60);

            Assert.That(clock.Day, Is.EqualTo(2));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(0));
            Assert.That(clock.SecondOfDay, Is.EqualTo(0));
        }

        [Test]
        public void AdvanceMinutes_CountsWholeMinutesInSeconds()
        {
            GameClock clock = GameClock.RunStart.AdvanceMinutes(15);

            Assert.That(clock.ElapsedSeconds, Is.EqualTo(15 * 60));
            Assert.That(clock.ElapsedMinutes, Is.EqualTo(15));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void AdvanceMinutes_ByLessThanOneMinute_Throws(int minutes)
        {
            Assert.That(() => GameClock.RunStart.AdvanceMinutes(minutes), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AdvanceSeconds_OneSecond_MovesTheClockOneSecondButNotAMinute()
        {
            GameClock clock = GameClock.RunStart.AdvanceSeconds(1);

            Assert.That(clock.ElapsedSeconds, Is.EqualTo(1));
            Assert.That(clock.ElapsedMinutes, Is.EqualTo(0));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(6 * 60));
        }

        [TestCase(59, 6 * 60, 59)]
        [TestCase(60, 6 * 60 + 1, 0)]
        [TestCase(61, 6 * 60 + 1, 1)]
        public void AdvanceSeconds_AroundAWholeMinute_RollsTheMinuteOverAtSixty(int seconds, int expectedMinuteOfDay, int expectedSecondOfMinute)
        {
            GameClock clock = GameClock.RunStart.AdvanceSeconds(seconds);

            Assert.That(clock.MinuteOfDay, Is.EqualTo(expectedMinuteOfDay));
            Assert.That(clock.SecondOfDay % GameClock.SecondsPerMinute, Is.EqualTo(expectedSecondOfMinute));
        }

        [Test]
        public void AdvanceSeconds_PastMidnight_StartsNextDay()
        {
            GameClock clock = GameClock.RunStart.AdvanceMinutes((18 * 60) - 1).AdvanceSeconds(60);

            Assert.That(clock.Day, Is.EqualTo(2));
            Assert.That(clock.SecondOfDay, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AdvanceSeconds_ByLessThanOneSecond_Throws(int seconds)
        {
            Assert.That(() => GameClock.RunStart.AdvanceSeconds(seconds), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WithNegativeElapsed_Throws()
        {
            Assert.That(() => new GameClock(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
