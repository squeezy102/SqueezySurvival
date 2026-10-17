using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Zones
{
    /// <summary>Small zones with known content, so tests can predict every find.</summary>
    public static class TestZoneDefinitions
    {
        public static readonly ItemDefinition Pebble = new ItemDefinition("pebble", "Pebble", "a pebble", "pebbles");
        public static readonly ItemDefinition Twig = new ItemDefinition("twig", "Twig", "a twig", "twigs");
        public static readonly ZoneFeatureDefinition Shrub = new ZoneFeatureDefinition("shrub", "Shrub", "a shrub", "shrubs", ZoneFeaturePermanence.Permanent);
        public static readonly ZoneFeatureDefinition Pond = new ZoneFeatureDefinition("pond", "Pond", "a pond", "ponds", ZoneFeaturePermanence.SemiPermanent);

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
