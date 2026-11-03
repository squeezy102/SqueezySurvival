using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Content
{
    [TestFixture]
    public sealed class GameContentCatalogBuilderItemAndFeatureProblemsTests
    {
        [TestCase("name")]
        [TestCase("singular phrase")]
        [TestCase("plural name")]
        [TestCase("description")]
        public void Build_ItemWithBlankText_ReportsTheFileTheItemAndTheField(string field)
        {
            ItemRecord item = TestContentRecords.Item("stone");
            switch (field)
            {
                case "name":
                    item.Name = "  ";
                    break;
                case "singular phrase":
                    item.SingularPhrase = "  ";
                    break;
                case "plural name":
                    item.PluralName = "  ";
                    break;
                default:
                    item.Description = "  ";
                    break;
            }

            Assert.That(TestContentRecords.ProblemsOfItems(item), Is.EqualTo(new[] { $"Items.json, item 'stone': {field} is empty" }));
        }

        [Test]
        public void Build_ZoneFeatureWithAnEmptyDescription_ReportsTheFileTheFeatureAndTheField()
        {
            ZoneFeatureRecord feature = TestContentRecords.Feature("puddle");
            feature.Description = string.Empty;

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ValidItemsFile(), TestContentRecords.ZoneFeaturesFile(feature));

            Assert.That(problems, Is.EqualTo(new[] { "ZoneFeatures.json, feature 'puddle': description is empty" }));
        }

        [Test]
        public void Build_ItemWithoutAnId_NamesItByItsPosition()
        {
            ItemRecord noId = TestContentRecords.Item("anything");
            noId.Id = string.Empty;

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOfItems(TestContentRecords.Item("stone"), noId);

            Assert.That(problems, Is.EqualTo(new[] { "Items.json, item at position 2: id is empty" }));
        }

        [Test]
        public void Build_ItemEntryThatIsNull_ReportsItByPosition()
        {
            IReadOnlyList<string> problems = TestContentRecords.ProblemsOfItems(TestContentRecords.Item("stone"), null);

            Assert.That(problems, Is.EqualTo(new[] { "Items.json, item at position 2: the entry is empty" }));
        }

        [Test]
        public void Build_ItemIdUsedTwice_ReportsTheLaterOne()
        {
            IReadOnlyList<string> problems = TestContentRecords.ProblemsOfItems(TestContentRecords.Item("stone"), TestContentRecords.Item("stone"));

            Assert.That(problems, Is.EqualTo(new[] { "Items.json, item 'stone': id is already used by an earlier item" }));
        }

        [Test]
        public void Build_ZoneFeatureIdUsedTwice_ReportsTheLaterOne()
        {
            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ValidItemsFile(),
                TestContentRecords.ZoneFeaturesFile(TestContentRecords.Feature("puddle"), TestContentRecords.Feature("puddle")));

            Assert.That(problems, Is.EqualTo(new[] { "ZoneFeatures.json, feature 'puddle': id is already used by an earlier feature" }));
        }

        [Test]
        public void Build_ItemWithoutACategory_ReportsItMissing()
        {
            ItemRecord item = TestContentRecords.Item("stone");
            item.Category = null;

            Assert.That(TestContentRecords.ProblemsOfItems(item), Is.EqualTo(new[] { "Items.json, item 'stone': category is missing" }));
        }

        [Test]
        public void Build_ItemWithACategoryNumberNoNameStandsFor_ReportsItUnknown()
        {
            ItemRecord item = TestContentRecords.Item("stone");
            item.Category = (ItemCategory)99;

            Assert.That(TestContentRecords.ProblemsOfItems(item), Is.EqualTo(new[] { "Items.json, item 'stone': category is not a known value" }));
        }

        [Test]
        public void Build_ZoneFeatureWithoutPermanence_ReportsItMissing()
        {
            ZoneFeatureRecord feature = TestContentRecords.Feature("puddle");
            feature.Permanence = null;

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ValidItemsFile(), TestContentRecords.ZoneFeaturesFile(feature));

            Assert.That(problems, Is.EqualTo(new[] { "ZoneFeatures.json, feature 'puddle': permanence is missing" }));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Build_ItemWeightThatIsNotMoreThanZero_ReportsIt(int weight)
        {
            ItemRecord item = TestContentRecords.Item("stone");
            item.WeightPounds = weight;

            Assert.That(
                TestContentRecords.ProblemsOfItems(item),
                Is.EqualTo(new[] { $"Items.json, item 'stone': weight must be more than 0 (it is {weight})" }));
        }

        [Test]
        public void Build_ItemWeightJustMoreThanZero_Builds()
        {
            ItemRecord item = TestContentRecords.Item("feather");
            item.WeightPounds = 0.01m;

            GameContentCatalog catalog = GameContentCatalogBuilder.Build(
                TestContentRecords.ItemsFile(item), TestContentRecords.ValidZoneFeaturesFile(), new ParsedZoneFile[0]);

            Assert.That(catalog.Items.Single().WeightPounds, Is.EqualTo(0.01m));
        }
    }
}
