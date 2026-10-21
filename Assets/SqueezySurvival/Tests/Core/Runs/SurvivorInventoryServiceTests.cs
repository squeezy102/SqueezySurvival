using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class SurvivorInventoryServiceTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Log = PlaceholderBeachZoneContent.Log;

        [Test]
        public void New_CarriesNothingAndCanMove()
        {
            var survivorInventory = new SurvivorInventoryService();

            Assert.That(survivorInventory.Load.CarriedPounds, Is.Zero);
            Assert.That(survivorInventory.CanMove, Is.True);
        }

        [Test]
        public void Load_CountsTheInventoryAndBothHands()
        {
            var survivorInventory = new SurvivorInventoryService();
            survivorInventory.Inventory.Receive(Stone, 10, -1);
            survivorInventory.Hands.Receive(Log, 1, 0);

            Assert.That(survivorInventory.Load.CarriedPounds, Is.EqualTo(40m));
            Assert.That(survivorInventory.Load.LimitPounds, Is.EqualTo(SurvivorLoad.PlaceholderLimitPounds));
        }

        [TestCase(3, true)]
        [TestCase(4, false)]
        public void CanMove_FalseOnlyAtOrPastTheLimit(int logs, bool expected)
        {
            var survivorInventory = new SurvivorInventoryService();
            survivorInventory.Inventory.Receive(Log, logs, -1);

            Assert.That(survivorInventory.CanMove, Is.EqualTo(expected));
        }
    }
}
