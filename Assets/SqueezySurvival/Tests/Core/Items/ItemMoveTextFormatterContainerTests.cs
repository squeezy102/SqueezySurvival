using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemMoveTextFormatterContainerTests
    {
        private const string ContainerName = "woven palm basket";
        private static readonly ItemPanel Basket = ItemPanel.ForContainer("basket");

        private static ItemMovePreview IntoTheBasket(int destinationSlot = 0, int alreadyThere = 0) =>
            new ItemMovePreview(TestItemDefinitions.Stone, 5, ItemPanel.Ground, Basket, destinationSlot, alreadyThere, 10, ContainerName);

        [Test]
        public void ToAContainer_NewStack_IsAPutIn()
        {
            ItemMovePreview preview = IntoTheBasket();

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Put in"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("Put 5 stones in the woven palm basket. Takes 10 sec."));
        }

        [Test]
        public void ToAContainer_JoiningAStack_IsAStackNamingTheContainer()
        {
            ItemMovePreview preview = IntoTheBasket(alreadyThere: 12);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Stack"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("Add 5 stones to the 12 in the woven palm basket. Takes 10 sec."));
        }

        [Test]
        public void ToAContainer_WithNoRoom_SaysThereIsNoFreeSlotInIt()
        {
            ItemMovePreview preview = IntoTheBasket(destinationSlot: -1);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("No room"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("There's no free slot in the woven palm basket."));
        }

        [Test]
        public void ToAContainer_LogsPuttingThingsInIt()
        {
            Assert.That(ItemMoveTextFormatter.FormatLog(IntoTheBasket()), Is.EqualTo("You put 5 stones in the woven palm basket."));
        }
    }
}
