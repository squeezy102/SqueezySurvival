using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedCollectionReceiveAndTakeTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;
        private static readonly ItemDefinition Shell = PlaceholderBeachZoneContent.Seashell;

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
        public void Receive_KindAlreadyPresent_AddsToItsCountAndRaisesChangedOnce()
        {
            CountedCollection<ItemDefinition> collection = StoneInSlotTwo();
            int changes = 0;
            collection.Changed += () => changes++;

            collection.Receive(Stone, 5, 0);

            Assert.That(collection.EntryAtSlot(2).Count, Is.EqualTo(8));
            Assert.That(collection.EntryAtSlot(0), Is.Null);
            Assert.That(changes, Is.EqualTo(1));
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
            int changes = 0;
            collection.Changed += () => changes++;

            collection.Take(2, 3);

            Assert.That(collection.EntryAtSlot(2), Is.Null);
            Assert.That(changes, Is.EqualTo(1));
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
    }
}
