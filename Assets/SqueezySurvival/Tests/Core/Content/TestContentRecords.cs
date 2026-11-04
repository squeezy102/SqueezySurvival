using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Content
{
    /// <summary>
    /// Valid content records for the builder's tests to break one detail at a time. The valid files hold the items
    /// "stone" and "small-stick", the feature "puddle", and the zone "beach", which names all three.
    /// </summary>
    public static class TestContentRecords
    {
        public static ItemRecord Item(string id) => new ItemRecord
        {
            Id = id,
            Name = "Name of " + id,
            SingularPhrase = "a " + id,
            PluralName = id + "s",
            Description = "Description of " + id + ".",
            Category = ItemCategory.StoneAndMinerals,
            WeightPounds = 1.5m,
        };

        public static ZoneFeatureRecord Feature(string id) => new ZoneFeatureRecord
        {
            Id = id,
            Name = "Name of " + id,
            SingularPhrase = "a " + id,
            PluralName = id + "s",
            Description = "Description of " + id + ".",
            Permanence = ZoneFeaturePermanence.SemiPermanent,
        };

        public static ZonePoolEntryRecord PoolEntry(string id, int weight) => new ZonePoolEntryRecord { Id = id, Weight = weight };

        /// <summary>A zone whose find pool is stone (weight 30) and small-stick (weight 1), whose feature pool is puddle, and whose one scripted explore finds a stone and a small-stick.</summary>
        public static ZoneRecord Zone(string id = "beach") => new ZoneRecord
        {
            Id = id,
            Name = "Name of " + id,
            ExploresToMap = 24,
            FindPool = new List<ZonePoolEntryRecord> { PoolEntry("stone", 30), PoolEntry("small-stick", 1) },
            FeaturePool = new List<ZonePoolEntryRecord> { PoolEntry("puddle", 2) },
            ScriptedExplores = new List<List<string>> { new List<string> { "stone", "small-stick" } },
        };

        public static ItemsFileRecord ItemsFile(params ItemRecord[] items) => new ItemsFileRecord { Items = new List<ItemRecord>(items) };

        public static ZoneFeaturesFileRecord ZoneFeaturesFile(params ZoneFeatureRecord[] features) =>
            new ZoneFeaturesFileRecord { ZoneFeatures = new List<ZoneFeatureRecord>(features) };

        public static ParsedZoneFile ZoneFile(ZoneRecord zone, string fileName = "Zones/Beach.json") => new ParsedZoneFile(fileName, zone);

        public static ItemsFileRecord ValidItemsFile() => ItemsFile(Item("stone"), Item("small-stick"));

        public static ZoneFeaturesFileRecord ValidZoneFeaturesFile() => ZoneFeaturesFile(Feature("puddle"));

        /// <summary>Builds the valid items and features with the given zone files.</summary>
        public static GameContentCatalog BuildWithValidItemsAndFeatures(params ParsedZoneFile[] zoneFiles) =>
            GameContentCatalogBuilder.Build(ValidItemsFile(), ValidZoneFeaturesFile(), zoneFiles);

        /// <summary>Every problem the builder reports for the given files.</summary>
        public static IReadOnlyList<string> ProblemsOf(ItemsFileRecord items, ZoneFeaturesFileRecord features, params ParsedZoneFile[] zoneFiles) =>
            Assert.Throws<GameContentValidationException>(() => GameContentCatalogBuilder.Build(items, features, zoneFiles)).Problems;

        /// <summary>Every problem the builder reports for the given item records, with valid features and no zones.</summary>
        public static IReadOnlyList<string> ProblemsOfItems(params ItemRecord[] items) =>
            ProblemsOf(ItemsFile(items), ValidZoneFeaturesFile());

        /// <summary>Every problem the builder reports for the given zone, with valid items and features.</summary>
        public static IReadOnlyList<string> ProblemsOfZone(ZoneRecord zone) =>
            ProblemsOf(ValidItemsFile(), ValidZoneFeaturesFile(), ZoneFile(zone));
    }
}
