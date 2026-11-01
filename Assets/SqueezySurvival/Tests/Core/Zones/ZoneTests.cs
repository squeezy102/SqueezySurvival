using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Zones
{
    [TestFixture]
    public sealed class ZoneTests
    {
        [Test]
        public void New_SizesTheGroundAndFeaturesByTheCapacityRules()
        {
            var zone = new Zone(TestZoneDefinitions.Create(4), new SlotCapacityRules(survivorInventorySlots: 1, handSlots: 1, groundSlots: 6, zoneFeatureSlots: 9));

            Assert.That(zone.GroundPool.SlotCapacity, Is.EqualTo(6));
            Assert.That(zone.Features.SlotCapacity, Is.EqualTo(9));
        }

        [Test]
        public void New_RejectsMissingCapacityRules()
        {
            Assert.Throws<ArgumentNullException>(() => new Zone(TestZoneDefinitions.Create(4), null));
        }
    }
}
