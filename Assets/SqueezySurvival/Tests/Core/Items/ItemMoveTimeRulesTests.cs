using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemMoveTimeRulesTests
    {
        [TestCase(0, 1)]
        [TestCase(0.1, 1)]
        [TestCase(0.5, 1)]
        [TestCase(0.51, 2)]
        [TestCase(1, 2)]
        [TestCase(5, 10)]
        public void SecondsToMove_Wireframe_IsTwoSecondsAPoundRoundedUpAndAtLeastOne(decimal pounds, int expected)
        {
            Assert.That(ItemMoveTimeRules.Wireframe.SecondsToMove(pounds), Is.EqualTo(expected));
        }

        [Test]
        public void SecondsToMove_UsesTheRulesRate()
        {
            var rules = new ItemMoveTimeRules(secondsPerPound: 3m);

            Assert.That(rules.SecondsToMove(5m), Is.EqualTo(15));
        }

        [Test]
        public void SecondsToMove_NegativeWeight_IsRefused()
        {
            Assert.That(() => ItemMoveTimeRules.Wireframe.SecondsToMove(-1m), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_RateOfZeroOrLess_IsRefused(decimal secondsPerPound)
        {
            Assert.That(() => new ItemMoveTimeRules(secondsPerPound), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
