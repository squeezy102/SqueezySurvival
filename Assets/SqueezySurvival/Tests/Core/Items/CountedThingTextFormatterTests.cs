using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class CountedThingTextFormatterTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;
        private static readonly ItemDefinition Seashell = PlaceholderBeachZoneContent.Seashell;

        [Test]
        public void FormatCountBadge_One_IsEmpty()
        {
            Assert.That(CountedThingTextFormatter.FormatCountBadge(1), Is.Empty);
        }

        [Test]
        public void FormatCountBadge_Several_ShowsNumber()
        {
            Assert.That(CountedThingTextFormatter.FormatCountBadge(12), Is.EqualTo("12"));
        }

        [Test]
        public void FormatSentencePhrase_One_UsesSingularPhrase()
        {
            Assert.That(CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone }), Is.EqualTo("a stone"));
        }

        [Test]
        public void FormatSentencePhrase_GroupsRepeatsAndJoinsWithAnd()
        {
            Assert.That(CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone, Stick, Stone }), Is.EqualTo("2 stones and a small stick"));
        }

        [Test]
        public void FormatSentencePhrase_ThreeKinds_UsesCommasThenAnd()
        {
            Assert.That(
                CountedThingTextFormatter.FormatSentencePhrase(new[] { Stone, Stick, Seashell }),
                Is.EqualTo("a stone, a small stick and a seashell"));
        }
    }
}
