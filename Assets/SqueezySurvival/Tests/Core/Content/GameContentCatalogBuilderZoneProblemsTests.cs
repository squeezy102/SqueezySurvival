using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;

namespace SqueezySurvival.Core.Tests.Content
{
    [TestFixture]
    public sealed class GameContentCatalogBuilderZoneProblemsTests
    {
        [Test]
        public void Build_ZoneWithoutAnId_NamesItByItsPositionAmongTheFiles()
        {
            ZoneRecord noId = TestContentRecords.Zone();
            noId.Id = string.Empty;

            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ValidItemsFile(),
                TestContentRecords.ValidZoneFeaturesFile(),
                TestContentRecords.ZoneFile(TestContentRecords.Zone("cliff"), "Zones/Cliff.json"),
                TestContentRecords.ZoneFile(noId, "Zones/Nameless.json"));

            Assert.That(problems, Is.EqualTo(new[] { "Zones/Nameless.json, zone at position 2: id is empty" }));
        }

        [Test]
        public void Build_ZoneIdUsedTwice_NamesTheFileThatHasItFirst()
        {
            IReadOnlyList<string> problems = TestContentRecords.ProblemsOf(
                TestContentRecords.ValidItemsFile(),
                TestContentRecords.ValidZoneFeaturesFile(),
                TestContentRecords.ZoneFile(TestContentRecords.Zone("beach"), "Zones/Beach.json"),
                TestContentRecords.ZoneFile(TestContentRecords.Zone("beach"), "Zones/BeachCopy.json"));

            Assert.That(problems, Is.EqualTo(new[] { "Zones/BeachCopy.json, zone 'beach': id is already used by Zones/Beach.json" }));
        }

        [Test]
        public void Build_ZoneWithABlankName_ReportsIt()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.Name = "  ";

            Assert.That(TestContentRecords.ProblemsOfZone(zone), Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach': name is empty" }));
        }

        [Test]
        public void Build_ZoneThatTakesNoExploresToMap_ReportsIt()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ExploresToMap = 0;

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach': explores to map must be at least 1 (it is 0)" }));
        }

        [Test]
        public void Build_ZoneThatTakesOneExploreToMap_Builds()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ExploresToMap = 1;

            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(TestContentRecords.ZoneFile(zone));

            Assert.That(catalog.Zones.Single().ExploresToMap, Is.EqualTo(1));
        }

        [Test]
        public void Build_UnknownItemInTheFindPool_NamesTheZoneAndTheId()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FindPool.Add(TestContentRecords.PoolEntry("stoen", 5));

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' find pool: unknown item id 'stoen'" }));
        }

        [Test]
        public void Build_UnknownFeatureInTheFeaturePool_NamesTheZoneAndTheId()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FeaturePool.Add(TestContentRecords.PoolEntry("pudle", 5));

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' feature pool: unknown feature id 'pudle'" }));
        }

        [Test]
        public void Build_PoolEntryWithoutAnId_ReportsItByPosition()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FindPool.Add(TestContentRecords.PoolEntry(string.Empty, 5));

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' find pool: entry 3 has no id" }));
        }

        [Test]
        public void Build_PoolEntryWithWeightBelowOne_ReportsIt()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FindPool[1].Weight = 0;

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' find pool: item 'small-stick' has weight 0, which must be at least 1" }));
        }

        [TestCase("find pool")]
        [TestCase("feature pool")]
        public void Build_PoolWithNoEntries_ReportsWhichPoolIsEmpty(string pool)
        {
            ZoneRecord zone = TestContentRecords.Zone();
            (pool == "find pool" ? zone.FindPool : zone.FeaturePool).Clear();

            Assert.That(TestContentRecords.ProblemsOfZone(zone), Is.EqualTo(new[] { $"Zones/Beach.json, zone 'beach' {pool}: is empty" }));
        }

        [Test]
        public void Build_PoolThatIsNull_ReportsItEmpty()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.FindPool = null;

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' find pool: is empty" }));
        }

        [TestCase("stoen")]
        [TestCase("")]
        public void Build_UnknownOrBlankItemInAScriptedExplore_NamesTheExplore(string itemId)
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ScriptedExplores.Add(new List<string> { "stone", itemId });

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { $"Zones/Beach.json, zone 'beach' scripted explore 2: unknown item id '{itemId}'" }));
        }

        [Test]
        public void Build_ScriptedExploreThatFindsNothing_ReportsIt()
        {
            ZoneRecord zone = TestContentRecords.Zone();
            zone.ScriptedExplores.Add(new List<string>());

            Assert.That(
                TestContentRecords.ProblemsOfZone(zone),
                Is.EqualTo(new[] { "Zones/Beach.json, zone 'beach' scripted explore 2: is empty" }));
        }
    }
}
