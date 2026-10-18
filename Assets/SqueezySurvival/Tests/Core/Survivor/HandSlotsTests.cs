using NUnit.Framework;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.Survivor
{
    [TestFixture]
    public sealed class HandSlotsTests
    {
        [TestCase(0, Hand.Left)]
        [TestCase(1, Hand.Right)]
        public void HandAt_LeftHandIsOnTheLeft(int slot, Hand expected)
        {
            Assert.That(HandSlots.HandAt(slot), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void HandAt_OutsideTheTwoSlots_IsRefused(int slot)
        {
            Assert.That(() => HandSlots.HandAt(slot), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }

        [TestCase(DominantHand.Left, Hand.Left)]
        [TestCase(DominantHand.Right, Hand.Right)]
        public void ToolHand_IsTheDominantHand(DominantHand dominantHand, Hand expected)
        {
            Assert.That(HandSlots.ToolHand(dominantHand), Is.EqualTo(expected));
        }
    }
}
