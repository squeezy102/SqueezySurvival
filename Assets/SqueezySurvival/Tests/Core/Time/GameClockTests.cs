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
        public void Advance_PastMidnight_StartsNextDay()
        {
            GameClock clock = GameClock.RunStart.Advance(18 * 60);

            Assert.That(clock.Day, Is.EqualTo(2));
            Assert.That(clock.MinuteOfDay, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Advance_ByLessThanOneMinute_Throws(int minutes)
        {
            Assert.That(() => GameClock.RunStart.Advance(minutes), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WithNegativeElapsed_Throws()
        {
            Assert.That(() => new GameClock(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
