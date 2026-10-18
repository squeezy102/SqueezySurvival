using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ThingTooltipContentFactoryTests
    {
        [Test]
        public void Create_ItemStack_ShowsNameCategoryDescriptionAndWeights()
        {
            ThingTooltipContent tooltip = ThingTooltipContentFactory.Create(new CountedEntry<ItemDefinition>(TestZoneDefinitions.Twig, 3, 0));

            Assert.That(tooltip.Title, Is.EqualTo("Twig"));
            Assert.That(tooltip.IconId, Is.EqualTo("twig"));
            Assert.That(tooltip.Subtitle, Is.EqualTo("Wood"));
            Assert.That(tooltip.Description, Is.EqualTo("A thin twig."));
            Assert.That(tooltip.DetailLines, Is.EqualTo(new[] { "Weight: 0.5 lb each", "Stack of 3: 1.5 lb" }));
        }

        [Test]
        public void Create_Feature_HasNoCategoryOrWeights()
        {
            ThingTooltipContent tooltip = ThingTooltipContentFactory.Create(new CountedEntry<ZoneFeatureDefinition>(TestZoneDefinitions.Pond, 1, 0));

            Assert.That(tooltip.Title, Is.EqualTo("Pond"));
            Assert.That(tooltip.Subtitle, Is.Empty);
            Assert.That(tooltip.DetailLines, Is.Empty);
        }
    }
}
