using System;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Presentation.Content;

namespace SqueezySurvival.Presentation.Tests.Content
{
    [TestFixture]
    public sealed class GameContentCatalogLoaderTests
    {
        private const string ItemsJson = @"{ ""Items"": [
  { ""Id"": ""stone"", ""Name"": ""Stone"", ""SingularPhrase"": ""a stone"", ""PluralName"": ""stones"", ""Description"": ""A stone."", ""Category"": ""StoneAndMinerals"", ""WeightPounds"": 1.0 } ] }";

        private const string ZoneFeaturesJson = @"{ ""ZoneFeatures"": [
  { ""Id"": ""puddle"", ""Name"": ""Puddle"", ""SingularPhrase"": ""a puddle"", ""PluralName"": ""puddles"", ""Description"": ""Rain water."", ""Permanence"": ""SemiPermanent"" } ] }";

        private const string BeachJson = @"{ ""Id"": ""beach"", ""Name"": ""Beach"", ""ExploresToMap"": 24,
  ""FindPool"": [ { ""Id"": ""stone"", ""Weight"": 30 } ], ""FeaturePool"": [ { ""Id"": ""puddle"", ""Weight"": 2 } ],
  ""ScriptedExplores"": [ [ ""stone"" ] ] }";

        private readonly GameContentCatalogLoader _loader = new GameContentCatalogLoader(new GameContentJsonReader());

        [Test]
        public void Load_ValidFiles_BuildsTheCatalogTheFilesDescribe()
        {
            GameContentCatalog catalog = _loader.Load(ItemsJson, ZoneFeaturesJson, new[] { new ZoneJsonText("Beach", BeachJson) });

            Assert.That(catalog.Items.Select(item => item.Id), Is.EqualTo(new[] { "stone" }));
            Assert.That(catalog.ZoneFeatures.Select(feature => feature.Id), Is.EqualTo(new[] { "puddle" }));
            Assert.That(catalog.TryGetZone("beach", out var beach), Is.True);
            Assert.That(beach.FindPool.Entries.Single().Outcome, Is.SameAs(catalog.Items[0]));
        }

        [Test]
        public void Load_ZoneFileThatIsNotValidJson_FailsNamingThatFile()
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(
                () => _loader.Load(ItemsJson, ZoneFeaturesJson, new[] { new ZoneJsonText("Beach", "{ not json") }));

            Assert.That(exception.FileName, Is.EqualTo("Zones/Beach.json"));
        }

        [Test]
        public void Load_ValuesTheBuilderRejects_FailsWithTheBuildersProblems()
        {
            string itemWithoutDescription = ItemsJson.Replace(@"""Description"": ""A stone.""", @"""Description"": """"");

            GameContentValidationException exception = Assert.Throws<GameContentValidationException>(
                () => _loader.Load(itemWithoutDescription, ZoneFeaturesJson, new[] { new ZoneJsonText("Beach", BeachJson) }));

            Assert.That(exception.Problems, Is.EqualTo(new[] { "Items.json, item 'stone': description is empty" }));
        }

        [Test]
        public void Constructor_NoReader_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new GameContentCatalogLoader(null));
        }

        [Test]
        public void Load_NoZoneFiles_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _loader.Load(ItemsJson, ZoneFeaturesJson, null));
        }
    }
}
