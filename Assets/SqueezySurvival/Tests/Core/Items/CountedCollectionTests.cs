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

        [Test]
        public void AddRange_GroupsByKindInOrderFirstAdded()
        {
            var collection = new CountedCollection<ItemDefinition>();

            collection.AddRange(new[] { Stone, Stick, Stone });

            Assert.That(collection.Entries.Count, Is.EqualTo(2));
            Assert.That(collection.Entries[0].Definition, Is.SameAs(Stone));
            Assert.That(collection.Entries[0].Count, Is.EqualTo(2));
            Assert.That(collection.CountOf(Stick), Is.EqualTo(1));
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

        [TestCase(2, 0, new[] { "stick", "stone", "seashell" })]
        [TestCase(0, 3, new[] { "seashell", "stick", "stone" })]
        [TestCase(0, 2, new[] { "seashell", "stone", "stick" })]
        public void Move_InsertsBeforeTheEntryAtInsertionIndex(int from, int insertion, string[] expectedIds)
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Stone, PlaceholderBeachZoneContent.Seashell, Stick });

            collection.Move(from, insertion);

            Assert.That(collection.Entries.Select(entry => entry.Definition.Id), Is.EqualTo(expectedIds));
        }

        [Test]
        public void Move_ToSamePlace_DoesNotRaiseChanged()
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Stone, Stick });
            int changes = 0;
            collection.Changed += () => changes++;

            collection.Move(0, 1);

            Assert.That(changes, Is.Zero);
        }

        [Test]
        public void AddRange_AfterMove_KeepsOrderAndAddsNewKindsAtEnd()
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Stone, Stick });
            collection.Move(1, 0);

            collection.AddRange(new[] { Stone, PlaceholderBeachZoneContent.Seashell });

            Assert.That(collection.Entries.Select(entry => entry.Definition.Id), Is.EqualTo(new[] { "stick", "stone", "seashell" }));
            Assert.That(collection.CountOf(Stone), Is.EqualTo(2));
        }
    }
}
