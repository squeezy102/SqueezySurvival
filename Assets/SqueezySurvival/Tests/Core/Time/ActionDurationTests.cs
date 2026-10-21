using System;
using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class ActionDurationTests
    {
        [TestCase(1, 60)]
        [TestCase(15, 900)]
        public void FromMinutes_CountsWholeMinutesInSeconds(int minutes, int expectedSeconds)
        {
            Assert.That(ActionDuration.FromMinutes(minutes).Seconds, Is.EqualTo(expectedSeconds));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void FromMinutes_LessThanOneMinute_Throws(int minutes)
        {
            Assert.That(() => ActionDuration.FromMinutes(minutes), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void FromSeconds_OneSecond_IsTheShortestAction()
        {
            Assert.That(ActionDuration.FromSeconds(1).Seconds, Is.EqualTo(1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void FromSeconds_LessThanOneSecond_Throws(int seconds)
        {
            Assert.That(() => ActionDuration.FromSeconds(seconds), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Equals_SameSecondsFromMinutesOrSeconds_AreEqual()
        {
            Assert.That(ActionDuration.FromMinutes(1), Is.EqualTo(ActionDuration.FromSeconds(60)));
            Assert.That(ActionDuration.FromMinutes(1), Is.Not.EqualTo(ActionDuration.FromSeconds(59)));
        }
    }
}
