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
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void AdvanceMinutes_ByLessThanOneMinute_Throws(int minutes)
        {
            Assert.That(() => GameClock.RunStart.AdvanceMinutes(minutes), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void AdvanceSeconds_ByLessThanOneSecond_Throws(int seconds)
        {
            Assert.That(() => GameClock.RunStart.AdvanceSeconds(seconds), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AdvanceSeconds_OneSecond_MovesTheSecondsButNotTheMinute()
        {
            GameClock clock = GameClock.RunStart.AdvanceSeconds(1);

            Assert.That(clock.ElapsedSeconds, Is.EqualTo(1));
            Assert.That(clock.ElapsedMinutes, Is.EqualTo(0));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(6 * 60));
            Assert.That(clock.SecondOfDay, Is.EqualTo((6 * 3600) + 1));
        }

        [TestCase(59, 0)]
        [TestCase(60, 1)]
        [TestCase(61, 1)]
        public void ElapsedMinutes_CountsOnlyWholeMinutes(int seconds, long expectedMinutes)
        {
            Assert.That(GameClock.RunStart.AdvanceSeconds(seconds).ElapsedMinutes, Is.EqualTo(expectedMinutes));
        }

        [Test]
        public void AdvanceSeconds_ToLastSecondBeforeMidnight_StaysOnDayOne()
        {
            GameClock lastSecond = GameClock.RunStart.AdvanceSeconds((18 * 3600) - 1);

            Assert.That(lastSecond.Day, Is.EqualTo(1));
            Assert.That(lastSecond.SecondOfDay, Is.EqualTo(GameClock.SecondsPerDay - 1));
            Assert.That(lastSecond.AdvanceSeconds(1).Day, Is.EqualTo(2));
        }

        [Test]
        public void AdvanceMinutes_IsTheSameAsSixtySecondsAMinute()
        {
            Assert.That(GameClock.RunStart.AdvanceMinutes(15).ElapsedSeconds, Is.EqualTo(15 * 60));
        }

        [Test]
        public void Constructor_WithNegativeElapsed_Throws()
        {
            Assert.That(() => new GameClock(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
