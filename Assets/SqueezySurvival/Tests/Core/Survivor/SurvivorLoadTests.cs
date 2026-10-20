using System;
using NUnit.Framework;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.Survivor
{
    [TestFixture]
    public sealed class SurvivorLoadTests
    {
        [TestCase(0, SurvivorLoadState.Normal)]
        [TestCase(74.99, SurvivorLoadState.Normal)]
        [TestCase(75, SurvivorLoadState.Overburdened)]
        [TestCase(99.99, SurvivorLoadState.Overburdened)]
        [TestCase(100, SurvivorLoadState.Immobile)]
        [TestCase(500000, SurvivorLoadState.Immobile)]
        public void State_FollowsTheLoadBands(decimal carriedPounds, SurvivorLoadState expected)
        {
            Assert.That(new SurvivorLoad(carriedPounds, 100m).State, Is.EqualTo(expected));
        }

        [Test]
        public void Percent_CanGoPastOneHundred()
        {
            Assert.That(new SurvivorLoad(150m, 100m).Percent, Is.EqualTo(150m));
        }

        [Test]
        public void Percent_IsAgainstTheLimit()
        {
            Assert.That(new SurvivorLoad(30m, 60m).Percent, Is.EqualTo(50m));
        }

        [Test]
        public void Constructor_NegativeWeight_IsRefused()
        {
            Assert.That(() => new SurvivorLoad(-1m, 100m), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_NoLimit_IsRefused(decimal limitPounds)
        {
            Assert.That(() => new SurvivorLoad(1m, limitPounds), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(34, "Load: 34 lb of 100 lb (34%), Normal")]
        [TestCase(99.99, "Load: 99.99 lb of 100 lb (99%), Overburdened")]
        [TestCase(250, "Load: 250 lb of 100 lb (250%), Immobile")]
        public void Format_ShowsWeightLimitPercentRoundedDownAndState(decimal carriedPounds, string expected)
        {
            Assert.That(SurvivorLoadTextFormatter.Format(new SurvivorLoad(carriedPounds, 100m)), Is.EqualTo(expected));
        }
    }
}
