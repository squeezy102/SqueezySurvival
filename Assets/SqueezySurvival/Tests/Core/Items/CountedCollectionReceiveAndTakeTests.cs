using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedCollectionReceiveAndTakeTests
    {
        private static readonly ItemDefinition Stone = TestItemDefinitions.Stone;
        private static readonly ItemDefinition Stick = TestItemDefinitions.SmallStick;
        private static readonly ItemDefinition Shell = TestItemDefinitions.Seashell;

        private static CountedCollection<ItemDefinition> StoneInSlotTwo()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 3, 2);
            return collection;
        }

        [Test]
        public void SlotToReceive_KindAlreadyPresent_IsItsOwnStackWhateverSlotIsPreferred()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stone, 0), Is.EqualTo(2));
        }

        [Test]
        public void SlotToReceive_NewKindOverAnEmptySlot_IsThatSlot()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stick, 3), Is.EqualTo(3));
        }

        [Test]
        public void SlotToReceive_NewKindOverAnotherThing_IsTheFirstEmptySlot()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stick, 2), Is.EqualTo(0));
        }

        [Test]
        public void SlotToReceive_NewKindWithNoPreference_IsTheFirstEmptySlot()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stick, -1), Is.EqualTo(0));
        }

        [Test]
        public void SlotToReceive_NewKindWhenFull_IsMinusOne()
        {
            var collection = new CountedCollection<ItemDefinition>(1);
            collection.Receive(Stone, 1, -1);

            Assert.That(collection.SlotToReceive(Stick, 0), Is.EqualTo(-1));
        }

        [Test]
        public void SlotToReceive_KindPresentWhenFull_IsStillItsStack()
        {
            var collection = new CountedCollection<ItemDefinition>(1);
            collection.Receive(Stone, 1, -1);

            Assert.That(collection.SlotToReceive(Stone, -1), Is.EqualTo(0));
        }

        [TestCase(-2)]
        [TestCase(4)]
        public void SlotToReceive_PreferredSlotOutsideTheGrid_Throws(int preferredSlot)
        {
            Assert.That(() => StoneInSlotTwo().SlotToReceive(Stick, preferredSlot), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Receive_KindAlreadyPresent_AddsToItsCount()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();

            collection.Receive(Stone, 5, 0);

            Assert.That(collection.EntryAtSlot(2).Count, Is.EqualTo(8));
            Assert.That(collection.EntryAtSlot(0), Is.Null);
        }

        [Test]
        public void Receive_NewKind_LandsInThePreferredEmptySlot()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();

            collection.Receive(Shell, 2, 1);

            Assert.That(collection.EntryAtSlot(1).Definition, Is.SameAs(Shell));
            Assert.That(collection.EntryAtSlot(1).Count, Is.EqualTo(2));
        }

        [Test]
        public void Receive_WhenThereIsNoRoom_ThrowsAndChangesNothing()
        {
            var collection = new CountedCollection<ItemDefinition>(1);
            collection.Receive(Stone, 1, -1);

            Assert.That(() => collection.Receive(Stick, 1, -1), Throws.InvalidOperationException);
            Assert.That(collection.Entries.Count, Is.EqualTo(1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Receive_LessThanOne_Throws(int count)
        {
            Assert.That(() => StoneInSlotTwo().Receive(Stick, count, -1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Take_PartOfAStack_LeavesTheRestInPlace()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();

            CountedThing<ItemDefinition> taken = collection.Take(2, 2);

            Assert.That(taken.Definition, Is.SameAs(Stone));
            Assert.That(taken.Count, Is.EqualTo(2));
            Assert.That(collection.EntryAtSlot(2).Count, Is.EqualTo(1));
        }

        [Test]
        public void Take_TheWholeStack_EmptiesTheSlot()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();

            collection.Take(2, 3);

            Assert.That(collection.EntryAtSlot(2), Is.Null);
        }

        [TestCase(0)]
        [TestCase(4)]
        public void Take_NoneOrMoreThanTheStack_Throws(int count)
        {
            Assert.That(() => StoneInSlotTwo().Take(2, count), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Take_FromAnEmptySlot_Throws()
        {
            Assert.That(() => StoneInSlotTwo().Take(0, 1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SlotToReceive_StartOwnStackOverAnEmptySlot_IsThatSlotEvenWithTheKindElsewhere()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stone, 3, ItemArrivalStacking.StartOwnStack), Is.EqualTo(3));
        }

        [Test]
        public void SlotToReceive_StartOwnStackOverTheKindsStack_IsThatStack()
        {
            Assert.That(StoneInSlotTwo().SlotToReceive(Stone, 2, ItemArrivalStacking.StartOwnStack), Is.EqualTo(2));
        }

        [TestCase(1)]
        [TestCase(-1)]
        public void SlotToReceive_StartOwnStackWithNoSlotToTake_IsTheFirstEmptySlotNotTheKindsStack(int preferredSlot)
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();
            collection.Receive(Stick, 1, 1);

            Assert.That(collection.SlotToReceive(Stone, preferredSlot, ItemArrivalStacking.StartOwnStack), Is.EqualTo(0));
        }

        [Test]
        public void SlotToReceive_StartOwnStackWhenFullAndKindPresent_FallsBackToTheKindsStack()
        {
            var collection = new CountedCollection<ItemDefinition>(2);
            collection.Receive(Stone, 1, 0);
            collection.Receive(Stick, 1, 1);

            Assert.That(collection.SlotToReceive(Stone, -1, ItemArrivalStacking.StartOwnStack), Is.EqualTo(0));
        }

        [Test]
        public void SlotToReceive_StartOwnStackWhenFullAndKindAbsent_IsMinusOne()
        {
            var collection = new CountedCollection<ItemDefinition>(2);
            collection.Receive(Stick, 1, 0);
            collection.Receive(Shell, 1, 1);

            Assert.That(collection.SlotToReceive(Stone, -1, ItemArrivalStacking.StartOwnStack), Is.EqualTo(-1));
        }

        [Test]
        public void Receive_StartOwnStack_MakesASecondStackOfTheKind()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();

            collection.Receive(Stone, 2, 3, ItemArrivalStacking.StartOwnStack);

            Assert.That(collection.EntryAtSlot(2).Count, Is.EqualTo(3));
            Assert.That(collection.EntryAtSlot(3).Count, Is.EqualTo(2));
        }

        [TestCase(2, 3)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        public void CountInSlot_IsTheKindsCountInThatSlotOnly(int slot, int expected)
        {
            Assert.That(StoneInSlotTwo().CountInSlot(Stone, slot), Is.EqualTo(expected));
        }

        [Test]
        public void CountInSlot_SlotHoldingAnotherKind_IsZero()
        {
            Assert.That(StoneInSlotTwo().CountInSlot(Stick, 2), Is.Zero);
        }
    }
}
