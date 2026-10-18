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
        // Stone and minerals: stone (1 lb), pumice (0.25 lb). Wood: stick (0.5 lb). Shells: seashell (0.1 lb).
        private static string[] SortedIds(ThingSortOrder order)
        {
            var collection = new CountedCollection<ItemDefinition>(8);
            collection.AddRange(new[]
            {
                PlaceholderBeachZoneContent.Seashell,
                PlaceholderBeachZoneContent.Stone,
                PlaceholderBeachZoneContent.Pumice, PlaceholderBeachZoneContent.Pumice, PlaceholderBeachZoneContent.Pumice,
                PlaceholderBeachZoneContent.Pumice, PlaceholderBeachZoneContent.Pumice,
                PlaceholderBeachZoneContent.Stick, PlaceholderBeachZoneContent.Stick, PlaceholderBeachZoneContent.Stick,
            });
            collection.Sort(new ThingSortComparer<ItemDefinition>(order));
            return collection.Entries.Select(entry => entry.Definition.Id).ToArray();
        }

        [Test]
        public void ByType_GroupsInCategoryOrderThenByName()
        {
            Assert.That(SortedIds(ThingSortOrder.ByType), Is.EqualTo(new[] { "stick", "pumice", "stone", "seashell" }));
        }

        [Test]
        public void Alphabetical_SortsByName()
        {
            Assert.That(SortedIds(ThingSortOrder.Alphabetical), Is.EqualTo(new[] { "pumice", "seashell", "stick", "stone" }));
        }

        [Test]
        public void ByQuantity_PutsLargestCountFirstThenByName()
        {
            Assert.That(SortedIds(ThingSortOrder.ByQuantity), Is.EqualTo(new[] { "pumice", "stick", "seashell", "stone" }));
        }

        [Test]
        public void ByWeight_PutsHeaviestStackFirst()
        {
            // Stacks: stick 1.5 lb, pumice 1.25 lb, stone 1 lb, seashell 0.1 lb.
            Assert.That(SortedIds(ThingSortOrder.ByWeight), Is.EqualTo(new[] { "stick", "pumice", "stone", "seashell" }));
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
