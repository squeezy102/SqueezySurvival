using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Content
{
    [TestFixture]
    public sealed class GameContentCatalogBuilderTests
    {
        [Test]
        public void Build_ValidItem_KeepsEveryValueTheFileGives()
        {
            ItemRecord record = TestContentRecords.Item("cone-snail");
            record.Category = ItemCategory.Shells;
            record.WeightPounds = 0.75m;
            record.FoundWarning = "Leave it alone.";

            GameContentCatalog catalog = GameContentCatalogBuilder.Build(
                TestContentRecords.ItemsFile(record), TestContentRecords.ValidZoneFeaturesFile(), new ParsedZoneFile[0]);

            ItemDefinition item = catalog.Items.Single();
            Assert.That(
                (item.Id, item.Name, item.SingularPhrase, item.PluralName, item.Description),
                Is.EqualTo((record.Id, record.Name, record.SingularPhrase, record.PluralName, record.Description)));
            Assert.That((item.Category, item.WeightPounds, item.FoundWarning), Is.EqualTo((ItemCategory.Shells, 0.75m, "Leave it alone.")));
        }

        [Test]
        public void Build_ItemWithoutAFoundWarning_HasNone()
        {
            ItemRecord record = TestContentRecords.Item("stone");
            record.FoundWarning = null;

            GameContentCatalog catalog = GameContentCatalogBuilder.Build(
                TestContentRecords.ItemsFile(record), TestContentRecords.ValidZoneFeaturesFile(), new ParsedZoneFile[0]);

            Assert.That(catalog.Items.Single().FoundWarning, Is.Empty);
        }

        [Test]
        public void Build_ValidZoneFeature_KeepsEveryValueTheFileGives()
        {
            ZoneFeatureRecord record = TestContentRecords.Feature("rock-outcrop");
            record.Permanence = ZoneFeaturePermanence.Permanent;

            GameContentCatalog catalog = GameContentCatalogBuilder.Build(
                TestContentRecords.ValidItemsFile(), TestContentRecords.ZoneFeaturesFile(record), new ParsedZoneFile[0]);

            ZoneFeatureDefinition feature = catalog.ZoneFeatures.Single();
            Assert.That(
                (feature.Id, feature.Name, feature.SingularPhrase, feature.PluralName, feature.Description),
                Is.EqualTo((record.Id, record.Name, record.SingularPhrase, record.PluralName, record.Description)));
            Assert.That(feature.Permanence, Is.EqualTo(ZoneFeaturePermanence.Permanent));
        }

        [Test]
        public void Build_ValidZone_ResolvesItsPoolsAndScriptedExploresToTheDefinitions()
        {
            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(TestContentRecords.ZoneFile(TestContentRecords.Zone("beach")));

            ItemDefinition stone = catalog.Items[0];
            ItemDefinition stick = catalog.Items[1];
            ZoneDefinition zone = catalog.Zones.Single();
            Assert.That((zone.Id, zone.Name, zone.ExploresToMap), Is.EqualTo(("beach", "Name of beach", 24)));
            Assert.That(zone.FindPool.Entries.Select(entry => (entry.Outcome, entry.Weight)), Is.EqualTo(new[] { (stone, 30), (stick, 1) }));
            Assert.That(zone.FeaturePool.Entries.Select(entry => (entry.Outcome, entry.Weight)), Is.EqualTo(new[] { (catalog.ZoneFeatures.Single(), 2) }));
            Assert.That(zone.ScriptedFinds.Single(), Is.EqualTo(new[] { stone, stick }));
        }

        [Test]
        public void Build_ZoneWithoutScriptedExplores_HasNone()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ScriptedExplores = new List<List<string>>();

            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(TestContentRecords.ZoneFile(zone));

            Assert.That(catalog.Zones.Single().ScriptedFinds, Is.Empty);
        }

        [Test]
        public void Build_ZoneWithNullScriptedExplores_HasNone()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ScriptedExplores = null;

            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(TestContentRecords.ZoneFile(zone));

            Assert.That(catalog.Zones.Single().ScriptedFinds, Is.Empty);
        }

        [Test]
        public void Build_FilesWithNullLists_HaveNothingInThem()
        {
            GameContentCatalog catalog = GameContentCatalogBuilder.Build(
                new ItemsFileRecord { Items = null }, new ZoneFeaturesFileRecord { ZoneFeatures = null }, new ParsedZoneFile[0]);

            Assert.That(catalog.Items, Is.Empty);
            Assert.That(catalog.ZoneFeatures, Is.Empty);
        }

        [Test]
        public void Build_SeveralZoneFiles_BuildsEveryZoneInTheOrderGiven()
        {
            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(
                TestContentRecords.ZoneFile(TestContentRecords.Zone("cliff"), "Zones/Cliff.json"),
                TestContentRecords.ZoneFile(TestContentRecords.Zone("beach"), "Zones/Beach.json"));

            Assert.That(catalog.Zones.Select(zone => zone.Id), Is.EqualTo(new[] { "cliff", "beach" }));
        }

        [Test]
        public void Build_ProblemsInEveryFile_ReportsAllOfThemTogether()
        {
            ItemRecord item = TestContentRecords.Item("stone");
            item.Description = string.Empty;
            ZoneFeatureRecord feature = TestContentRecords.Feature("puddle");
            feature.Permanence = null;
            ZoneRecord zone = TestContentRecords.Zone("beach");
            zone.ExploresToMap = 0;

            GameContentValidationException exception = Assert.Throws<GameContentValidationException>(() => GameContentCatalogBuilder.Build(
                TestContentRecords.ItemsFile(item, TestContentRecords.Item("small-stick")),
                TestContentRecords.ZoneFeaturesFile(feature),
                new[] { TestContentRecords.ZoneFile(zone) }));

            string[] expected =
            {
                "Items.json, item 'stone': description is empty",
                "ZoneFeatures.json, feature 'puddle': permanence is missing",
                "Zones/Beach.json, zone 'beach': explores to map must be at least 1 (it is 0)",
            };
            Assert.That(exception.Problems, Is.EqualTo(expected));
            foreach (string problem in expected)
            {
                StringAssert.Contains(problem, exception.Message);
            }
        }

        [Test]
        public void Build_ZoneNamingAnItemThatHasAProblem_ReportsOnlyTheItem()
        {
            ItemRecord stone = TestContentRecords.Item("stone");
            stone.Description = string.Empty;

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ItemsFile(stone, TestContentRecords.Item("small-stick")),
                TestContentRecords.ValidZoneFeaturesFile(),
                TestContentRecords.ZoneFile(TestContentRecords.Zone()));

            Assert.That(problems, Is.EqualTo(new[] { "Items.json, item 'stone': description is empty" }));
        }

        [Test]
        public void Build_ZoneWhoseOnlyPoolEntryIsAnItemWithAProblem_ReportsOnlyTheItem()
        {
            ItemRecord stone = TestContentRecords.Item("stone");
            stone.Description = string.Empty;
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FindPool = new List<ZonePoolEntryRecord> { TestContentRecords.PoolEntry("stone", 1) };
            zone.ScriptedExplores = new List<List<string>> { new List<string> { "stone" } };

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ItemsFile(stone), TestContentRecords.ValidZoneFeaturesFile(), TestContentRecords.ZoneFile(zone));

            Assert.That(problems, Is.EqualTo(new[] { "Items.json, item 'stone': description is empty" }));
        }
    }
}
