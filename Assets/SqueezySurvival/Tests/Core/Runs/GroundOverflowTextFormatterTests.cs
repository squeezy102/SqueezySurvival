using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class GroundOverflowTextFormatterTests
    {
        private static readonly CountedThing<ItemDefinition> OnePebble = new CountedThing<ItemDefinition>(TestZoneDefinitions.Pebble, 1);
        private static readonly CountedThing<ItemDefinition> TwoPebbles = new CountedThing<ItemDefinition>(TestZoneDefinitions.Pebble, 2);
        private static readonly CountedEntry<ItemDefinition> ThreeTwigs = new CountedEntry<ItemDefinition>(TestZoneDefinitions.Twig, 3, 0);
        private static readonly CountedEntry<ItemDefinition> OneTwig = new CountedEntry<ItemDefinition>(TestZoneDefinitions.Twig, 1, 0);

        [Test]
        public void Question_NamesTheFindWithItsCount()
        {
            Assert.That(GroundOverflowTextFormatter.FormatQuestion(OnePebble), Is.EqualTo("There's no room on the ground for the pebble."));
            Assert.That(GroundOverflowTextFormatter.FormatQuestion(TwoPebbles), Is.EqualTo("There's no room on the ground for the 2 pebbles."));
        }

        [Test]
        public void LeaveBehind_WarnsItIsGoneForGood()
        {
            Assert.That(GroundOverflowTextFormatter.FormatLeaveBehindChoice(TwoPebbles), Is.EqualTo("Leave the 2 pebbles behind"));
            Assert.That(GroundOverflowTextFormatter.FormatLeaveBehindWarning(TwoPebbles), Is.EqualTo("They will be gone for good."));
            Assert.That(GroundOverflowTextFormatter.FormatLeftBehindLog(OnePebble), Is.EqualTo("You left the pebble behind. It's gone for good."));
        }

        [Test]
        public void DiscardHint_NamesTheFindAndHowToGoBack()
        {
            Assert.That(
                GroundOverflowTextFormatter.FormatDiscardHint(TwoPebbles),
                Is.EqualTo("Click something on the ground to discard for the 2 pebbles. Esc: go back."));
        }

        [Test]
        public void DiscardPreview_NamesWhatGoesAndWarnsItIsGoneForGood()
        {
            Assert.That(
                GroundOverflowTextFormatter.FormatDiscardPreview(ThreeTwigs, OnePebble),
                Is.EqualTo("Discard the twig (3) to make room for the pebble. They will be gone for good."));
            Assert.That(
                GroundOverflowTextFormatter.FormatDiscardPreview(OneTwig, OnePebble),
                Is.EqualTo("Discard the twig to make room for the pebble. It will be gone for good."));
        }

        [Test]
        public void DiscardedLog_SaysWhatWasThrownAwayForWhat()
        {
            Assert.That(
                GroundOverflowTextFormatter.FormatDiscardedLog(ThreeTwigs, TwoPebbles),
                Is.EqualTo("You threw away the 3 twigs to make room for the 2 pebbles. They're gone for good."));
        }
    }
}
