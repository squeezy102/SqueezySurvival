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
    }
}
