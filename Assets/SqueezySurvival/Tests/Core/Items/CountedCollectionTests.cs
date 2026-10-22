using System;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedCollectionTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;
        private static readonly ItemDefinition Shell = PlaceholderBeachZoneContent.Seashell;
        private static readonly ItemDefinition Conch = PlaceholderBeachZoneContent.Conch;

        private static CountedCollection<ItemDefinition> StoneStickShell(int slotCapacity = 8)
        {
            var collection = new CountedCollection<ItemDefinition>(slotCapacity);
            collection.AddRange(new[] { Stone, Stick, Shell });
            return collection;
        }

        [Test]
        public void AddRange_GroupsByKindAndGivesNewKindsTheNextSlot()
        {
            var collection = new CountedCollection<ItemDefinition>(8);

            collection.AddRange(new[] { Stone, Stick, Stone });

            Assert.That(collection.EntryAtSlot(0).Definition, Is.SameAs(Stone));
            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(2));
            Assert.That(collection.EntryAtSlot(1).Definition, Is.SameAs(Stick));
        }

        [Test]
        public void AddRange_IntoTheLastEmptySlot_FitsAndFills()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 4);

            Assert.That(collection.AddRange(new[] { Conch }), Is.Empty);
            Assert.That(collection.IsFull, Is.True);
        }

        [Test]
        public void AddRange_WhenFull_ReturnsNewKindsGroupedInArrivalOrderAndAddsMoreOfPresentKinds()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 3);
            var coconut = PlaceholderBeachZoneContent.Coconut;

            var overflow = collection.AddRange(new[] { Conch, Stone, coconut, Conch });

            Assert.That(overflow.Select(thing => (thing.Definition.Id, thing.Count)), Is.EqualTo(new[] { ("conch", 2), ("coconut", 1) }));
            Assert.That(collection.CountOf(Stone), Is.EqualTo(2));
            Assert.That(collection.CountOf(Conch), Is.Zero);
        }

        [Test]
        public void HasRoomFor_WhenFull_OnlyKindsAlreadyPresent()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 3);

            Assert.That(collection.HasRoomFor(Stick), Is.True);
            Assert.That(collection.HasRoomFor(Conch), Is.False);
        }

        [Test]
        public void MoveToSlot_EmptySlot_MovesOnlyThatEntryAndLeavesAGap()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();

            collection.MoveToSlot(0, 5);

            Assert.That(collection.Entries.Select(entry => (entry.Slot, entry.Definition.Id)), Is.EqualTo(new[] { (1, "small-stick"), (2, "seashell"), (5, "stone") }));
        }

        [Test]
        public void MoveToSlot_OccupiedSlot_SwapsTheTwoAndMovesNothingElse()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();

            collection.MoveToSlot(2, 0);

            Assert.That(collection.Entries.Select(entry => (entry.Slot, entry.Definition.Id)), Is.EqualTo(new[] { (0, "seashell"), (1, "small-stick"), (2, "stone") }));
        }

        [TestCase(7, true)]
        [TestCase(8, false)]
        public void MoveToSlot_LastSlotIsAllowedAndPastItIsNot(int toSlot, bool allowed)
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 8);

            TestDelegate move = () => collection.MoveToSlot(0, toSlot);

            if (allowed)
            {
                Assert.That(move, Throws.Nothing);
            }
            else
            {
                Assert.That(move, Throws.TypeOf<ArgumentOutOfRangeException>());
            }
        }

        [Test]
        public void AddRange_NewKind_FillsTheFirstGapWithoutMovingAnything()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.MoveToSlot(1, 4);

            collection.AddRange(new[] { Stone, PlaceholderBeachZoneContent.Coconut });

            Assert.That(collection.EntryAtSlot(1).Definition, Is.SameAs(PlaceholderBeachZoneContent.Coconut));
            Assert.That(collection.EntryAtSlot(4).Definition, Is.SameAs(Stick));
            Assert.That(collection.CountOf(Stone), Is.EqualTo(2));
        }

        [Test]
        public void ReplaceSlot_ThrowsAwayWhatIsThereAndPutsTheNewKindInItsPlace()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 3);
            collection.AddRange(new[] { Stick });

            CountedEntry<ItemDefinition> discarded = collection.ReplaceSlot(1, new CountedThing<ItemDefinition>(Conch, 2));

            Assert.That((discarded.Definition.Id, discarded.Count), Is.EqualTo(("small-stick", 2)));
            Assert.That(collection.Entries.Select(entry => (entry.Slot, entry.Definition.Id, entry.Count)), Is.EqualTo(new[] { (0, "stone", 1), (1, "conch", 2), (2, "seashell", 1) }));
        }

        [Test]
        public void ReplaceSlot_WithAKindAlreadyPresent_Throws()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();

            Assert.That(() => collection.ReplaceSlot(1, new CountedThing<ItemDefinition>(Stone, 1)), Throws.InvalidOperationException);
        }

        [Test]
        public void Sort_PacksIntoTheFirstSlotsInTheGivenOrder()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.MoveToSlot(0, 6);

            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));

            Assert.That(collection.Entries.Select(entry => (entry.Slot, entry.Definition.Id)), Is.EqualTo(new[] { (0, "seashell"), (1, "small-stick"), (2, "stone") }));
        }
    }
}
