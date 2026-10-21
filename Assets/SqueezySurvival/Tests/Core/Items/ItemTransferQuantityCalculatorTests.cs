using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemTransferQuantityCalculatorTests
    {
        [TestCase(1, ItemTransferQuantity.All, 1)]
        [TestCase(50, ItemTransferQuantity.All, 50)]
        [TestCase(1, ItemTransferQuantity.Half, 1)]
        [TestCase(2, ItemTransferQuantity.Half, 1)]
        [TestCase(7, ItemTransferQuantity.Half, 4)]
        [TestCase(8, ItemTransferQuantity.Half, 4)]
        [TestCase(1, ItemTransferQuantity.One, 1)]
        [TestCase(50, ItemTransferQuantity.One, 1)]
        public void CountToMove_TakesAllHalfRoundedUpOrOne(int stackCount, ItemTransferQuantity quantity, int expected)
        {
            Assert.That(ItemTransferQuantityCalculator.CountToMove(stackCount, quantity), Is.EqualTo(expected));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CountToMove_EmptyStack_IsRefused(int stackCount)
        {
            Assert.That(() => ItemTransferQuantityCalculator.CountToMove(stackCount, ItemTransferQuantity.All), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
