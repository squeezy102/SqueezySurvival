using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Zones
{
    /// <summary>Small zones with known content, so tests can predict every find.</summary>
    public static class TestZoneDefinitions
    {
        public static readonly ItemDefinition Pebble = new ItemDefinition("pebble", "Pebble", "a pebble", "pebbles", "A small pebble.", ItemCategory.StoneAndMinerals, 0.25m);
        public static readonly ItemDefinition Twig = new ItemDefinition("twig", "Twig", "a twig", "twigs", "A thin twig.", ItemCategory.Wood, 0.5m);
        public static readonly ZoneFeatureDefinition Shrub = new ZoneFeatureDefinition("shrub", "Shrub", "a shrub", "shrubs", "A low shrub.", ZoneFeaturePermanence.Permanent);
        public static readonly ZoneFeatureDefinition Pond = new ZoneFeatureDefinition("pond", "Pond", "a pond", "ponds", "A shallow pond.", ZoneFeaturePermanence.SemiPermanent);

        /// <summary>Distinct items, none of them a pebble or twig, for filling a grid.</summary>
        public static IReadOnlyList<ItemDefinition> DistinctItems(int count)
        {
            var items = new List<ItemDefinition>(count);
            for (int index = 0; index < count; index++)
            {
                items.Add(new ItemDefinition($"filler-{index}", $"Filler {index}", "a filler", "fillers", "A filler.", ItemCategory.Salvage, 1m));
            }

            return items;
        }

        /// <summary>Distinct features, neither a shrub nor a pond, for filling a grid.</summary>
        public static IReadOnlyList<ZoneFeatureDefinition> DistinctFeatures(int count)
        {
            var features = new List<ZoneFeatureDefinition>(count);
            for (int index = 0; index < count; index++)
            {
                features.Add(new ZoneFeatureDefinition($"filler-{index}", $"Filler {index}", "a filler", "fillers", "A filler.", ZoneFeaturePermanence.Permanent));
            }

            return features;
        }

        /// <summary>Pebble is weight 1 (roll 0), twig weight 1 (roll 1); shrub roll 0, pond roll 1.</summary>
        public static ZoneDefinition Create(int exploresToMap, params IReadOnlyList<ItemDefinition>[] scriptedFinds) => new ZoneDefinition(
            "test-cove",
            "Cove",
            exploresToMap,
            new WeightedTable<ItemDefinition>(new[]
            {
                new WeightedTableEntry<ItemDefinition>(Pebble, 1),
                new WeightedTableEntry<ItemDefinition>(Twig, 1),
            }),
            new WeightedTable<ZoneFeatureDefinition>(new[]
            {
                new WeightedTableEntry<ZoneFeatureDefinition>(Shrub, 1),
                new WeightedTableEntry<ZoneFeatureDefinition>(Pond, 1),
            }),
            scriptedFinds);
    }
}
