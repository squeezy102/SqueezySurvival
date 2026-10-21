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
        public void SecondsToMove_IsTwoSecondsAPoundRoundedUpAndAtLeastOne(decimal pounds, int expected)
        {
            Assert.That(ItemMoveTimeRules.SecondsToMove(pounds), Is.EqualTo(expected));
        }

        [Test]
        public void SecondsToMove_NegativeWeight_IsRefused()
        {
            Assert.That(() => ItemMoveTimeRules.SecondsToMove(-1m), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
