using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemMoveDestinationPolicyTests
    {
        private static readonly ItemPanel Basket = ItemPanel.ForContainer("basket");
        private readonly ItemMoveDestinationPolicy _policy = new ItemMoveDestinationPolicy();

        [Test]
        public void DestinationsFor_TheGroundWithNoContainerOpen_IsTheInventory()
        {
            Assert.That(_policy.DestinationsFor(ItemPanel.Ground, null), Is.EqualTo(new[] { ItemPanel.Inventory }));
        }

        [Test]
        public void DestinationsFor_TheInventoryWithNoContainerOpen_IsTheGround()
        {
            Assert.That(_policy.DestinationsFor(ItemPanel.Inventory, null), Is.EqualTo(new[] { ItemPanel.Ground }));
        }

        [Test]
        public void DestinationsFor_TheGroundWithAContainerOpen_IsOnlyTheContainer()
        {
            Assert.That(_policy.DestinationsFor(ItemPanel.Ground, Basket), Is.EqualTo(new[] { Basket }));
        }

        [Test]
        public void DestinationsFor_TheInventoryWithAContainerOpen_IsOnlyTheContainer()
        {
            Assert.That(_policy.DestinationsFor(ItemPanel.Inventory, Basket), Is.EqualTo(new[] { Basket }));
        }

        [Test]
        public void DestinationsFor_TheOpenContainer_IsTheInventoryThenTheGround()
        {
            Assert.That(_policy.DestinationsFor(Basket, Basket), Is.EqualTo(new[] { ItemPanel.Inventory, ItemPanel.Ground }));
        }

        [Test]
        public void DestinationsFor_AContainerWhenNoneIsOpen_IsStillTheInventoryThenTheGround()
        {
            Assert.That(_policy.DestinationsFor(Basket, null), Is.EqualTo(new[] { ItemPanel.Inventory, ItemPanel.Ground }));
        }

        [Test]
        public void DestinationsFor_AnOpenContainerThatIsNotAContainerPanel_Throws()
        {
            Assert.That(() => _policy.DestinationsFor(ItemPanel.Ground, ItemPanel.Inventory), Throws.TypeOf<System.ArgumentException>());
        }
    }
}
