using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class ItemMoveDropOutcomeFactoryTests
    {
        private static ItemMovePreview Preview(int destinationSlot, int alreadyThere = 0) =>
            new ItemMovePreview(PlaceholderBeachZoneContent.Stone, 5, ItemPanel.Ground, ItemPanel.Inventory, destinationSlot, alreadyThere, 10);

        [Test]
        public void Create_Room_IsATransferOutlinedWhereTheThingsLand()
        {
            GridDropOutcome outcome = ItemMoveDropOutcomeFactory.Create(Preview(destinationSlot: 4), hoveredSlot: 1);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Transfer));
            Assert.That(outcome.TargetSlot, Is.EqualTo(4));
            Assert.That(outcome.AnchorSlot, Is.EqualTo(4));
            Assert.That(outcome.ChangesSomething, Is.True);
            Assert.That(outcome.Title, Is.EqualTo("Pick up"));
            Assert.That(outcome.Description, Is.EqualTo("Put 5 stones in your inventory. Takes 10 sec."));
        }

        [Test]
        public void Create_JoiningAStack_IsTitledStack()
        {
            Assert.That(ItemMoveDropOutcomeFactory.Create(Preview(destinationSlot: 0, alreadyThere: 3), hoveredSlot: 6).Title, Is.EqualTo("Stack"));
        }

        [Test]
        public void Create_NoRoom_ChangesNothingAndIsShownWhereThePointerIs()
        {
            GridDropOutcome outcome = ItemMoveDropOutcomeFactory.Create(Preview(destinationSlot: -1), hoveredSlot: 2);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.NoRoom));
            Assert.That(outcome.ChangesSomething, Is.False);
            Assert.That(outcome.AnchorSlot, Is.EqualTo(2));
            Assert.That(outcome.Title, Is.EqualTo("No room"));
        }

        [Test]
        public void Create_NullPreview_IsRefused()
        {
            Assert.That(() => ItemMoveDropOutcomeFactory.Create(null, 0), Throws.ArgumentNullException);
        }
    }
}
