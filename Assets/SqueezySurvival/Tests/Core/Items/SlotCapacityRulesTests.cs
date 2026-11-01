using System;
using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class SlotCapacityRulesTests
    {
        [Test]
        public void Wireframe_MatchesTheDesignDocuments()
        {
            // Docs/Design/Systems/Inventory.md (Slots) and Docs/Design/UI/InventoryGrids.md (The grids).
            Assert.That(SlotCapacityRules.Wireframe.SurvivorInventorySlots, Is.EqualTo(8));
            Assert.That(SlotCapacityRules.Wireframe.HandSlots, Is.EqualTo(2));
            Assert.That(SlotCapacityRules.Wireframe.GroundSlots, Is.EqualTo(36));
            Assert.That(SlotCapacityRules.Wireframe.ZoneFeatureSlots, Is.EqualTo(36));
        }

        [Test]
        public void Constructor_KeepsEachCapacityInItsOwnProperty()
        {
            var rules = new SlotCapacityRules(survivorInventorySlots: 1, handSlots: 2, groundSlots: 3, zoneFeatureSlots: 4);

            Assert.That(rules.SurvivorInventorySlots, Is.EqualTo(1));
            Assert.That(rules.HandSlots, Is.EqualTo(2));
            Assert.That(rules.GroundSlots, Is.EqualTo(3));
            Assert.That(rules.ZoneFeatureSlots, Is.EqualTo(4));
        }

        [TestCase(0, 1, 1, 1, "survivorInventorySlots")]
        [TestCase(1, 0, 1, 1, "handSlots")]
        [TestCase(1, 1, 0, 1, "groundSlots")]
        [TestCase(1, 1, 1, 0, "zoneFeatureSlots")]
        [TestCase(-1, 1, 1, 1, "survivorInventorySlots")]
        public void Constructor_RejectsACapacityBelowOne(int inventory, int hands, int ground, int features, string parameterName)
        {
            var thrown = Assert.Throws<ArgumentOutOfRangeException>(() => new SlotCapacityRules(inventory, hands, ground, features));

            Assert.That(thrown.ParamName, Is.EqualTo(parameterName));
        }
    }
}
