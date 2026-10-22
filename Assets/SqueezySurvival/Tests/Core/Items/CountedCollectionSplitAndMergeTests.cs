using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedCollectionSplitAndMergeTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;

        private static CountedCollection<ItemDefinition> SevenStonesAndAStick()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 7, 0);
            collection.Receive(Stick, 1, 1);
            return collection;
        }

        [Test]
        public void SplitToSlot_PartOfAStack_MakesASecondStackOfTheKind()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();

            collection.SplitToSlot(0, 3, 4);

            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(3));
            Assert.That(collection.EntryAtSlot(3).Definition, Is.SameAs(Stone));
            Assert.That(collection.EntryAtSlot(3).Count, Is.EqualTo(4));
            Assert.That(collection.CountOf(Stone), Is.EqualTo(7));
        }

        [Test]
        public void SplitToSlot_AllOfAStack_MovesIt()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();

            collection.SplitToSlot(0, 3, 7);

            Assert.That(collection.EntryAtSlot(0), Is.Null);
            Assert.That(collection.EntryAtSlot(3).Count, Is.EqualTo(7));
        }

        [Test]
        public void SplitToSlot_OntoAnOccupiedSlot_Throws()
        {
            Assert.That(() => SevenStonesAndAStick().SplitToSlot(0, 1, 2), Throws.InvalidOperationException);
        }

        [TestCase(0)]
        [TestCase(8)]
        public void SplitToSlot_NoneOrMoreThanTheStack_Throws(int count)
        {
            Assert.That(() => SevenStonesAndAStick().SplitToSlot(0, 3, count), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void SplitToSlot_OntoItself_Throws()
        {
            Assert.That(() => SevenStonesAndAStick().SplitToSlot(0, 0, 1), Throws.ArgumentException);
        }

        [Test]
        public void MergeIntoSlot_PartOfAStack_MovesThatMany()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();
            collection.SplitToSlot(0, 3, 4);

            collection.MergeIntoSlot(3, 0, 1);

            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(4));
            Assert.That(collection.EntryAtSlot(3).Count, Is.EqualTo(3));
        }

        [Test]
        public void MergeIntoSlot_AllOfAStack_EmptiesItsSlot()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();
            collection.SplitToSlot(0, 3, 4);

            collection.MergeIntoSlot(3, 0, 4);

            Assert.That(collection.EntryAtSlot(3), Is.Null);
            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(7));
        }

        [Test]
        public void MergeIntoSlot_OntoAnotherKind_Throws()
        {
            Assert.That(() => SevenStonesAndAStick().MergeIntoSlot(0, 1, 1), Throws.InvalidOperationException);
        }

        [Test]
        public void MergeIntoSlot_OntoAnEmptySlot_Throws()
        {
            Assert.That(() => SevenStonesAndAStick().MergeIntoSlot(0, 3, 1), Throws.InvalidOperationException);
        }

        [Test]
        public void AddRange_KindSplitAcrossSlots_JoinsItsFirstStack()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();
            collection.SplitToSlot(0, 3, 4);

            collection.AddRange(new[] { Stone });

            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(4));
            Assert.That(collection.EntryAtSlot(3).Count, Is.EqualTo(4));
        }

        [Test]
        public void SlotToReceive_OverOneOfTheKindsStacks_IsThatStack()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();
            collection.SplitToSlot(0, 3, 4);

            Assert.That(collection.SlotToReceive(Stone, 3), Is.EqualTo(3));
            Assert.That(collection.SlotToReceive(Stone, 2), Is.EqualTo(0));
        }

        [Test]
        public void Sort_JoinsSplitStacksOfAKind()
        {
            CountedCollection<ItemDefinition> collection = SevenStonesAndAStick();
            collection.SplitToSlot(0, 3, 4);

            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));

            Assert.That(collection.Entries.Count, Is.EqualTo(2));
            Assert.That(collection.CountOf(Stone), Is.EqualTo(7));
            Assert.That(collection.EntryAtSlot(0).Definition, Is.SameAs(Stick));
            Assert.That(collection.EntryAtSlot(1).Count, Is.EqualTo(7));
        }
    }
}
