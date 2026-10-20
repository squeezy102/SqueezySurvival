using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemMoveTextFormatterTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;

        private static ItemMovePreview Preview(int count, ItemPanel to, int destinationSlot = 0, int alreadyThere = 0, int seconds = 10) =>
            new ItemMovePreview(Stone, count, to == ItemPanel.Inventory ? ItemPanel.Ground : ItemPanel.Inventory, to, destinationSlot, alreadyThere, seconds);

        [Test]
        public void ToInventory_NewStack_IsAPickUp()
        {
            ItemMovePreview preview = Preview(5, ItemPanel.Inventory);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Pick up"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("Put 5 stones in your inventory. Takes 10 sec."));
            Assert.That(ItemMoveTextFormatter.FormatLog(preview), Is.EqualTo("You picked up 5 stones."));
        }

        [Test]
        public void ToGround_OneThing_IsAPutDown()
        {
            ItemMovePreview preview = Preview(1, ItemPanel.Ground, seconds: 2);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Put down"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("Put a stone on the ground. Takes 2 sec."));
            Assert.That(ItemMoveTextFormatter.FormatLog(preview), Is.EqualTo("You put a stone on the ground."));
        }

        [Test]
        public void KindAlreadyThere_IsAStack()
        {
            ItemMovePreview preview = Preview(5, ItemPanel.Inventory, alreadyThere: 12);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("Stack"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("Add 5 stones to the 12 in your inventory. Takes 10 sec."));
        }

        [Test]
        public void NoRoom_SaysSoWithoutATime()
        {
            ItemMovePreview preview = Preview(5, ItemPanel.Ground, destinationSlot: -1);

            Assert.That(ItemMoveTextFormatter.FormatTitle(preview), Is.EqualTo("No room"));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("There's no free slot on the ground."));
        }
    }
}
