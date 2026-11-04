using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Content;

namespace SqueezySurvival.Presentation.Tests.Content
{
    [TestFixture]
    public sealed class GameContentJsonReaderTests
    {
        private const string ValidItemsJson = @"{
  ""Items"": [
    {
      ""Id"": ""cone-snail"",
      ""Name"": ""Cone snail"",
      ""SingularPhrase"": ""a cone snail"",
      ""PluralName"": ""cone snails"",
      ""Description"": ""A pretty shell."",
      ""Category"": ""Shells"",
      ""WeightPounds"": 0.25,
      ""FoundWarning"": ""Leave it alone.""
    }
  ]
}";

        private readonly GameContentJsonReader _reader = new GameContentJsonReader();

        [Test]
        public void ReadItems_ValidFile_ReadsEveryValue()
        {
            ItemRecord item = _reader.ReadItems(ValidItemsJson).Items[0];

            Assert.That(
                (item.Id, item.Name, item.SingularPhrase, item.PluralName, item.Description, item.Category, item.WeightPounds, item.FoundWarning),
                Is.EqualTo(("cone-snail", "Cone snail", "a cone snail", "cone snails", "A pretty shell.", (ItemCategory?)ItemCategory.Shells, 0.25m, "Leave it alone.")));
        }

        [Test]
        public void ReadItems_FileLeavingOutOptionalAndEnumKeys_ReportsThemAsAbsent()
        {
            ItemRecord item = _reader.ReadItems(@"{ ""Items"": [ { ""Id"": ""stone"" } ] }").Items[0];

            Assert.That(item.Category, Is.Null);
            Assert.That(item.FoundWarning, Is.Empty);
        }

        [Test]
        public void ReadItems_KeyThatMatchesNoProperty_FailsNamingTheFileAndTheKey()
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(
                () => _reader.ReadItems(@"{ ""Items"": [ { ""Id"": ""stone"", ""Descripton"": ""A stone."" } ] }"));

            Assert.That(exception.FileName, Is.EqualTo("Items.json"));
            StringAssert.StartsWith("Items.json: ", exception.Message);
            StringAssert.Contains("Descripton", exception.Message);
        }

        [Test]
        public void ReadItems_CategoryWrittenAsANumber_Fails()
        {
            Assert.Throws<GameContentFormatException>(
                () => _reader.ReadItems(@"{ ""Items"": [ { ""Id"": ""stone"", ""Category"": 2 } ] }"));
        }

        [Test]
        public void ReadItems_CategoryNameTheGameDoesNotKnow_Fails()
        {
            Assert.Throws<GameContentFormatException>(
                () => _reader.ReadItems(@"{ ""Items"": [ { ""Id"": ""stone"", ""Category"": ""Gemstones"" } ] }"));
        }

        [Test]
        public void ReadItems_TextThatIsNotJson_FailsNamingTheFile()
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(() => _reader.ReadItems(@"{ ""Items"": [ "));

            Assert.That(exception.FileName, Is.EqualTo("Items.json"));
        }

        [TestCase("")]
        [TestCase("null")]
        public void ReadItems_FileWithNothingInIt_FailsSayingItIsEmpty(string json)
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(() => _reader.ReadItems(json));

            Assert.That(exception.Message, Is.EqualTo("Items.json: the file is empty."));
        }

        [Test]
        public void ReadZoneFeatures_ValidFile_ReadsEveryValueWithPermanenceByName()
        {
            const string json = @"{
  ""ZoneFeatures"": [
    {
      ""Id"": ""puddle"",
      ""Name"": ""Puddle"",
      ""SingularPhrase"": ""a puddle"",
      ""PluralName"": ""puddles"",
      ""Description"": ""Rain water."",
      ""Permanence"": ""SemiPermanent""
    }
  ]
}";

            ZoneFeatureRecord feature = _reader.ReadZoneFeatures(json).ZoneFeatures[0];

            Assert.That(
                (feature.Id, feature.Name, feature.SingularPhrase, feature.PluralName, feature.Description, feature.Permanence),
                Is.EqualTo(("puddle", "Puddle", "a puddle", "puddles", "Rain water.", (ZoneFeaturePermanence?)ZoneFeaturePermanence.SemiPermanent)));
        }

        [Test]
        public void ReadZoneFeatures_KeyThatMatchesNoProperty_FailsNamingTheFile()
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(
                () => _reader.ReadZoneFeatures(@"{ ""ZoneFeatures"": [ { ""Id"": ""puddle"", ""Permanance"": ""SemiPermanent"" } ] }"));

            Assert.That(exception.FileName, Is.EqualTo("ZoneFeatures.json"));
        }

        [Test]
        public void ReadZone_ValidFile_ReadsEveryValueAndNamesTheFile()
        {
            const string json = @"{
  ""Id"": ""beach"",
  ""Name"": ""Beach"",
  ""ExploresToMap"": 24,
  ""FindPool"": [ { ""Id"": ""stone"", ""Weight"": 30 } ],
  ""FeaturePool"": [ { ""Id"": ""puddle"", ""Weight"": 2 } ],
  ""ScriptedExplores"": [ [ ""stone"", ""stone"", ""small-stick"" ], [ ""seaweed"" ] ]
}";

            ParsedZoneFile file = _reader.ReadZone("Beach", json);

            Assert.That(file.FileName, Is.EqualTo("Zones/Beach.json"));
            ZoneRecord zone = file.Zone;
            Assert.That((zone.Id, zone.Name, zone.ExploresToMap), Is.EqualTo(("beach", "Beach", 24)));
            Assert.That(zone.FindPool.ConvertAll(entry => (entry.Id, entry.Weight)), Is.EqualTo(new[] { ("stone", 30) }));
            Assert.That(zone.FeaturePool.ConvertAll(entry => (entry.Id, entry.Weight)), Is.EqualTo(new[] { ("puddle", 2) }));
            Assert.That(zone.ScriptedExplores, Is.EqualTo(new[] { new[] { "stone", "stone", "small-stick" }, new[] { "seaweed" } }));
        }

        [Test]
        public void ReadZone_KeyThatMatchesNoProperty_FailsNamingTheZoneFile()
        {
            GameContentFormatException exception = Assert.Throws<GameContentFormatException>(
                () => _reader.ReadZone("Beach", @"{ ""Id"": ""beach"", ""FindPol"": [] }"));

            Assert.That(exception.FileName, Is.EqualTo("Zones/Beach.json"));
            StringAssert.Contains("FindPol", exception.Message);
        }
    }
}
