using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class OrderedListMoveTests
    {
        [TestCase(1, 4, 3)]
        [TestCase(3, 1, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(2, 3, 2)]
        public void ResultingIndex_AccountsForRemovalFirst(int from, int insertion, int expected)
        {
            Assert.That(OrderedListMove.ResultingIndex(from, insertion), Is.EqualTo(expected));
        }

        // Moving entry 1 to slot 3 of [A B C D E] previews [A C D B E].
        [TestCase(0, 0)]
        [TestCase(1, 3)]
        [TestCase(2, 1)]
        [TestCase(4, 4)]
        public void SlotAfterMove_Forward_ShiftsEntriesBetweenBack(int index, int expectedSlot)
        {
            Assert.That(OrderedListMove.SlotAfterMove(index, 1, 3), Is.EqualTo(expectedSlot));
        }

        // Moving entry 3 to slot 1 of [A B C D E] previews [A D B C E].
        [TestCase(1, 2)]
        [TestCase(3, 1)]
        [TestCase(4, 4)]
        public void SlotAfterMove_Backward_ShiftsEntriesBetweenForward(int index, int expectedSlot)
        {
            Assert.That(OrderedListMove.SlotAfterMove(index, 3, 1), Is.EqualTo(expectedSlot));
        }

        [Test]
        public void IndexAtSlotAfterMove_InvertsSlotAfterMove()
        {
            for (int from = 0; from < 5; from++)
            {
                for (int to = 0; to < 5; to++)
                {
                    for (int index = 0; index < 5; index++)
                    {
                        int slot = OrderedListMove.SlotAfterMove(index, from, to);
                        Assert.That(OrderedListMove.IndexAtSlotAfterMove(slot, from, to), Is.EqualTo(index), $"from {from} to {to} index {index}");
                    }
                }
            }
        }
    }
}
