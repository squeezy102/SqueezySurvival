using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedThingTextFormatterTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.Stick;
        private static readonly ItemDefinition Seashell = PlaceholderBeachZoneContent.Seashell;

        [Test]
        public void FormatListLine_One_ShowsNameOnly()
        {
            Assert.That(CountedThingTextFormatter.FormatListLine(new CountedEntry<ItemDefinition>(Stick, 1)), Is.EqualTo("Stick"));
        }

        [Test]
        public void FormatListLine_Several_ShowsCount()
        {
            Assert.That(CountedThingTextFormatter.FormatListLine(new CountedEntry<ItemDefinition>(Stone, 2)), Is.EqualTo("Stone (2)"));
        }

        [Test]
        public void FormatSentencePhrase_One_UsesSingularPhrase()
        {
            Assert.That(CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone }), Is.EqualTo("a stone"));
        }

        [Test]
        public void FormatSentencePhrase_GroupsRepeatsAndJoinsWithAnd()
        {
            Assert.That(CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone, Stick, Stone }), Is.EqualTo("2 stones and a stick"));
        }

        [Test]
        public void FormatSentencePhrase_ThreeKinds_UsesCommasThenAnd()
        {
            Assert.That(
                CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone, Stick, Seashell }),
                Is.EqualTo("a stone, a stick and a seashell"));
        }
    }
}
