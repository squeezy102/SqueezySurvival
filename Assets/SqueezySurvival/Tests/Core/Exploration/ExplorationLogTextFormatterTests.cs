using NUnit.Framework;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Exploration
{
    [TestFixture]
    public sealed class ExplorationLogTextFormatterTests
    {
        private static ExplorationOutcome Outcome(int exploresBefore, int exploresToMap, ItemDefinition[] items, ZoneFeatureDefinition[] features) =>
            new ExplorationOutcome(
                items,
                features,
                new CountedThing<ItemDefinition>[0],
                new ExplorationProgress(exploresBefore, exploresToMap),
                new ExplorationProgress(exploresBefore + 1, exploresToMap));

        [Test]
        public void Format_ItemsOnly_WritesOneEntry()
        {
            ExplorationOutcome outcome = Outcome(0, 4, new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Pebble }, new ZoneFeatureDefinition[0]);

            Assert.That(ExplorationLogTextFormatter.Format("Cove", outcome), Is.EqualTo(new[] { "You explored the Cove and found 2 pebbles." }));
        }

        [Test]
        public void Format_WithFeature_AddsFeatureEntry()
        {
            ExplorationOutcome outcome = Outcome(0, 4, new[] { TestZoneDefinitions.Twig }, new[] { TestZoneDefinitions.Pond });

            Assert.That(ExplorationLogTextFormatter.Format("Cove", outcome)[1], Is.EqualTo("You came across a pond."));
        }

        [Test]
        public void Format_ExploreThatMapsZone_AddsCompletionEntry()
        {
            ExplorationOutcome outcome = Outcome(3, 4, new[] { TestZoneDefinitions.Twig }, new ZoneFeatureDefinition[0]);

            Assert.That(
                ExplorationLogTextFormatter.Format("Cove", outcome)[1],
                Is.EqualTo("You have explored every corner of the Cove, though there is always more to find."));
        }
    }
}
