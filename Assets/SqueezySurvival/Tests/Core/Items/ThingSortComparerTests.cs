using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ThingSortComparerTests
    {
        // Wood: small stick (0.25 lb) x3. Stone and minerals: coral (0.2 lb) x4, stone (1 lb). Shells: seashell (0.1 lb).
        private static string[] SortedIds(ThingSortOrder order)
        {
            var collection = new CountedCollection<ItemDefinition>(8);
            collection.AddRange(new[]
            {
                PlaceholderBeachZoneContent.Seashell,
                PlaceholderBeachZoneContent.Stone,
                PlaceholderBeachZoneContent.Coral, PlaceholderBeachZoneContent.Coral, PlaceholderBeachZoneContent.Coral,
                PlaceholderBeachZoneContent.Coral,
                PlaceholderBeachZoneContent.SmallStick, PlaceholderBeachZoneContent.SmallStick, PlaceholderBeachZoneContent.SmallStick,
            });
            collection.Sort(new ThingSortComparer<ItemDefinition>(order));
            return collection.Entries.Select(entry => entry.Definition.Id).ToArray();
        }

        [Test]
        public void ByType_GroupsInCategoryOrderThenByName()
        {
            Assert.That(SortedIds(ThingSortOrder.ByType), Is.EqualTo(new[] { "small-stick", "coral", "stone", "seashell" }));
        }

        [Test]
        public void Alphabetical_SortsByName()
        {
            Assert.That(SortedIds(ThingSortOrder.Alphabetical), Is.EqualTo(new[] { "coral", "seashell", "small-stick", "stone" }));
        }

        [Test]
        public void ByQuantity_PutsLargestCountFirstThenByName()
        {
            Assert.That(SortedIds(ThingSortOrder.ByQuantity), Is.EqualTo(new[] { "coral", "small-stick", "seashell", "stone" }));
        }

        [Test]
        public void ByWeight_PutsHeaviestStackFirst()
        {
            // Stacks: stone 1 lb, coral 0.8 lb, small stick 0.75 lb, seashell 0.1 lb.
            Assert.That(SortedIds(ThingSortOrder.ByWeight), Is.EqualTo(new[] { "stone", "coral", "small-stick", "seashell" }));
        }

        [Test]
        public void ByType_ThingsWithoutCategory_SortByName()
        {
            var features = new CountedCollection<ZoneFeatureDefinition>(4);
            features.AddRange(new[] { TestZoneDefinitions.Shrub, TestZoneDefinitions.Pond });

            features.Sort(new ThingSortComparer<ZoneFeatureDefinition>(ThingSortOrder.ByType));

            Assert.That(features.Entries.Select(entry => entry.Definition.Id), Is.EqualTo(new[] { "pond", "shrub" }));
        }
    }
}
