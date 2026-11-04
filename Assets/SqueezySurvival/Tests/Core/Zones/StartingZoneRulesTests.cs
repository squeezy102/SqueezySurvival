using System;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Tests.Content;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Zones
{
    [TestFixture]
    public sealed class StartingZoneRulesTests
    {
        [Test]
        public void SelectStartingZone_ContentWithTheZone_ReturnsItAmongOthers()
        {
            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(
                TestContentRecords.ZoneFile(TestContentRecords.Zone("cliff"), "Zones/Cliff.json"),
                TestContentRecords.ZoneFile(TestContentRecords.Zone("beach"), "Zones/Beach.json"));

            ZoneDefinition startingZone = new StartingZoneRules("beach").SelectStartingZone(catalog);

            Assert.That(startingZone.Id, Is.EqualTo("beach"));
        }

        [Test]
        public void SelectStartingZone_ContentWithoutTheZone_NamesTheMissingZone()
        {
            GameContentCatalog catalog = TestContentRecords.BuildWithValidItemsAndFeatures(
                TestContentRecords.ZoneFile(TestContentRecords.Zone("cliff"), "Zones/Cliff.json"));

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
                () => new StartingZoneRules("beach").SelectStartingZone(catalog));

            StringAssert.Contains("'beach'", exception.Message);
        }
    }
}
