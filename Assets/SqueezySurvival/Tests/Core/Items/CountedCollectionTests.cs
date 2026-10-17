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
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.Stick;
        private static readonly ItemDefinition Shell = PlaceholderBeachZoneContent.Seashell;

        private static CountedCollection<ItemDefinition> StoneStickShell()
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Stone, Stick, Shell });
            return collection;
        }

        [Test]
        public void AddRange_GroupsByKindAndGivesNewKindsTheNextSlot()
        {
            var collection = new CountedCollection<ItemDefinition>();

            collection.AddRange(new[] { Stone, Stick, Stone });

            Assert.That(collection.EntryAtSlot(0).Definition, Is.SameAs(Stone));
            Assert.That(collection.EntryAtSlot(0).Count, Is.EqualTo(2));
            Assert.That(collection.EntryAtSlot(1).Definition, Is.SameAs(Stick));
        }

        [Test]
        public void AddRange_RaisesChangedOnce()
        {
            var collection = new CountedCollection<ItemDefinition>();
            int changes = 0;
            collection.Changed += () => changes++;

            collection.AddRange(new[] { Stone, Stick });

            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void AddRange_WithNothing_DoesNotRaiseChanged()
        {
            var collection = new CountedCollection<ItemDefinition>();
            int changes = 0;
            collection.Changed += () => changes++;

            collection.AddRange(new ItemDefinition[0]);

            Assert.That(changes, Is.Zero);
        }

        [Test]
        public void MoveToSlot_EmptySlot_MovesOnlyThatEntryAndLeavesAGap()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();

            collection.MoveToSlot(0, 5);

            Assert.That(collection.EntryAtSlot(0), Is.Null);
            Assert.That(collection.EntryAtSlot(1).Definition, Is.SameAs(Stick));
            Assert.That(collection.EntryAtSlot(5).Definition, Is.SameAs(Stone));
            Assert.That(collection.SlotCount, Is.EqualTo(6));
        }

        [Test]
        public void MoveToSlot_OccupiedSlot_ShiftsLaterThingsIntoTheVacatedGap()
        {
            // Rock, paper, scissors, lizard, spock: lizard before paper.
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Stone, Stick, Shell, PlaceholderBeachZoneContent.Coconut, PlaceholderBeachZoneContent.Conch });

            collection.MoveToSlot(3, 1);

            Assert.That(collection.Entries.Select(entry => entry.Definition.Id), Is.EqualTo(new[] { "stone", "coconut", "stick", "seashell", "conch" }));
            Assert.That(collection.EntryAtSlot(4).Definition, Is.SameAs(PlaceholderBeachZoneContent.Conch));
        }

        [Test]
        public void MoveToSlot_OccupiedSlot_StopsShiftingAtTheFirstGap()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.MoveToSlot(2, 6);

            collection.MoveToSlot(6, 0);

            Assert.That(collection.Entries.Select(entry => (entry.Slot, entry.Definition.Id)), Is.EqualTo(new[]
            {
                (0, "seashell"),
                (1, "stone"),
                (2, "stick"),
            }));
        }

        [Test]
        public void MoveToSlot_SameSlot_DoesNotRaiseChanged()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            int changes = 0;
            collection.Changed += () => changes++;

            collection.MoveToSlot(1, 1);

            Assert.That(changes, Is.Zero);
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
    }
}
