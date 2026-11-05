using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemMovePreviewContainerDestinationTests
    {
        private static readonly ItemPanel Basket = ItemPanel.ForContainer("basket");

        [Test]
        public void Constructor_ToAContainerWithoutAName_Throws()
        {
            Assert.That(
                () => new ItemMovePreview(TestItemDefinitions.Stone, 5, ItemPanel.Ground, Basket, 0, 0, 10),
                Throws.TypeOf<System.ArgumentException>());
        }

        [Test]
        public void Constructor_ToAContainerWithAName_KeepsTheName()
        {
            var preview = new ItemMovePreview(TestItemDefinitions.Stone, 5, ItemPanel.Ground, Basket, 0, 0, 10, "woven palm basket");

            Assert.That(preview.DestinationName, Is.EqualTo("woven palm basket"));
        }

        [Test]
        public void Constructor_ToTheGround_HasNoDestinationName()
        {
            var preview = new ItemMovePreview(TestItemDefinitions.Stone, 5, ItemPanel.Inventory, ItemPanel.Ground, 0, 0, 10);

            Assert.That(preview.DestinationName, Is.Null);
        }
    }
}
