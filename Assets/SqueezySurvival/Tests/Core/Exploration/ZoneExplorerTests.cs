using NUnit.Framework;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Tests.Randomness;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Exploration
{
    [TestFixture]
    public sealed class ZoneExplorerTests
    {
        // 1 to 3 finds per explore, 25% feature chance: random calls are count, one pick per find, feature roll,
        // then a feature pick when the roll is under 25.
        private static readonly ExplorationRules Rules = new ExplorationRules(15, 1, 3, 25);

        private static ExplorationOutcome ExploreOnce(Zone zone, params int[] randomNumbers)
        {
            var randomNumberSource = new QueuedRandomNumberSource(randomNumbers);
            ExplorationOutcome outcome = new ZoneExplorer(Rules, randomNumberSource).Explore(zone);
            Assert.That(randomNumberSource.RemainingCount, Is.Zero, "Every queued random number should be used.");
            return outcome;
        }

        [Test]
        public void Explore_RaisesProgressByOneExplore()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));

            ExploreOnce(zone, 1, 0, 99);

            Assert.That(zone.ExplorationProgress.ExploresCompleted, Is.EqualTo(1));
            Assert.That(zone.ExplorationProgress.WholePercent, Is.EqualTo(25));
        }

        [Test]
        public void Explore_DrawsRandomNumberOfFindsIntoGroundPool()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));

            ExplorationOutcome outcome = ExploreOnce(zone, 3, 0, 1, 0, 99);

            Assert.That(outcome.ItemsFound, Is.EqualTo(new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Twig, TestZoneDefinitions.Pebble }));
            Assert.That(zone.GroundPool.CountOf(TestZoneDefinitions.Pebble), Is.EqualTo(2));
            Assert.That(zone.GroundPool.CountOf(TestZoneDefinitions.Twig), Is.EqualTo(1));
        }

        [Test]
        public void Explore_FeatureRollUnderChance_RevealsFeature()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));

            ExplorationOutcome outcome = ExploreOnce(zone, 1, 0, 24, 1);

            Assert.That(outcome.FeaturesFound, Is.EqualTo(new[] { TestZoneDefinitions.Pond }));
            Assert.That(zone.Features.CountOf(TestZoneDefinitions.Pond), Is.EqualTo(1));
        }

        [Test]
        public void Explore_FeatureRollAtChance_RevealsNothing()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));

            ExplorationOutcome outcome = ExploreOnce(zone, 1, 0, 25);

            Assert.That(outcome.FeaturesFound, Is.Empty);
            Assert.That(zone.Features.Entries, Is.Empty);
        }

        [Test]
        public void Explore_ScriptedExplore_GivesScriptedFindsWithoutRandomness()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4, new[] { TestZoneDefinitions.Twig, TestZoneDefinitions.Twig }));

            ExplorationOutcome outcome = ExploreOnce(zone);

            Assert.That(outcome.ItemsFound, Is.EqualTo(new[] { TestZoneDefinitions.Twig, TestZoneDefinitions.Twig }));
            Assert.That(outcome.FeaturesFound, Is.Empty);
        }

        [Test]
        public void Explore_AfterScriptedExplores_DrawsRandomly()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4, new[] { TestZoneDefinitions.Twig }));
            ExploreOnce(zone);

            ExplorationOutcome outcome = ExploreOnce(zone, 1, 0, 99);

            Assert.That(outcome.ItemsFound, Is.EqualTo(new[] { TestZoneDefinitions.Pebble }));
        }

        [Test]
        public void Explore_FullyExploredZone_StillFindsItemsAndFeatures()
        {
            var zone = new Zone(TestZoneDefinitions.Create(1));
            ExploreOnce(zone, 1, 0, 99);

            ExplorationOutcome outcome = ExploreOnce(zone, 2, 1, 1, 0, 0);

            Assert.That(outcome.ItemsFound.Count, Is.EqualTo(2));
            Assert.That(outcome.FeaturesFound, Is.EqualTo(new[] { TestZoneDefinitions.Shrub }));
            Assert.That(zone.ExplorationProgress.WholePercent, Is.EqualTo(100));
        }

        [Test]
        public void Explore_ThatMapsZoneFully_ReportsCompletion()
        {
            var zone = new Zone(TestZoneDefinitions.Create(2));
            Assert.That(ExploreOnce(zone, 1, 0, 99).CompletedTheZone, Is.False);

            Assert.That(ExploreOnce(zone, 1, 0, 99).CompletedTheZone, Is.True);
            Assert.That(ExploreOnce(zone, 1, 0, 99).CompletedTheZone, Is.False);
        }

        [Test]
        public void Explore_RaisesZoneChangeEvents()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));
            int progressChanges = 0, groundPoolChanges = 0, featureChanges = 0;
            zone.ExplorationProgressChanged += _ => progressChanges++;
            zone.GroundPool.Changed += (_, _) => groundPoolChanges++;
            zone.Features.Changed += (_, _) => featureChanges++;

            ExploreOnce(zone, 1, 0, 0, 0);

            Assert.That(new[] { progressChanges, groundPoolChanges, featureChanges }, Is.EqualTo(new[] { 1, 1, 1 }));
        }

        [Test]
        public void Explore_GroundFull_NewKindIsReturnedAsOverflowNotAdded()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));
            zone.GroundPool.AddRange(TestZoneDefinitions.DistinctItems(zone.GroundPool.SlotCapacity));

            ExplorationOutcome outcome = ExploreOnce(zone, 2, 0, 0, 99);

            Assert.That(outcome.GroundOverflow.Count, Is.EqualTo(1));
            Assert.That((outcome.GroundOverflow[0].Definition, outcome.GroundOverflow[0].Count), Is.EqualTo((TestZoneDefinitions.Pebble, 2)));
            Assert.That(zone.GroundPool.CountOf(TestZoneDefinitions.Pebble), Is.Zero);
        }

        [Test]
        public void Explore_FeaturesFull_NewKindIsNotRevealed()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4));
            zone.Features.AddRange(TestZoneDefinitions.DistinctFeatures(zone.Features.SlotCapacity));

            ExplorationOutcome outcome = ExploreOnce(zone, 1, 0, 0, 0);

            Assert.That(outcome.FeaturesFound, Is.Empty);
            Assert.That(zone.Features.CountOf(TestZoneDefinitions.Shrub), Is.Zero);
        }
    }
}
