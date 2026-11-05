using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class ItemPanelTests
    {
        [Test]
        public void GroundAndInventory_AreEachEqualToThemselvesAndDifferentFromEachOther()
        {
            Assert.That(ItemPanel.Ground == ItemPanel.Ground, Is.True);
            Assert.That(ItemPanel.Inventory == ItemPanel.Inventory, Is.True);
            Assert.That(ItemPanel.Ground != ItemPanel.Inventory, Is.True);
        }

        [Test]
        public void ForContainer_TheSameId_IsTheSamePanel()
        {
            Assert.That(ItemPanel.ForContainer("basket"), Is.EqualTo(ItemPanel.ForContainer("basket")));
            Assert.That(ItemPanel.ForContainer("basket").GetHashCode(), Is.EqualTo(ItemPanel.ForContainer("basket").GetHashCode()));
        }

        [Test]
        public void ForContainer_DifferentIds_AreDifferentPanels()
        {
            Assert.That(ItemPanel.ForContainer("basket") == ItemPanel.ForContainer("chest"), Is.False);
        }

        [Test]
        public void ForContainer_IsNeitherTheGroundNorTheInventory()
        {
            Assert.That(ItemPanel.ForContainer("basket") == ItemPanel.Ground, Is.False);
            Assert.That(ItemPanel.ForContainer("basket") == ItemPanel.Inventory, Is.False);
            Assert.That(ItemPanel.ForContainer("basket").Kind, Is.EqualTo(ItemPanelKind.Container));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void ForContainer_WithABlankId_Throws(string containerId)
        {
            Assert.That(() => ItemPanel.ForContainer(containerId), Throws.TypeOf<System.ArgumentException>());
        }

        [Test]
        public void ForContainer_WithNoId_Throws()
        {
            Assert.That(() => ItemPanel.ForContainer(null), Throws.TypeOf<System.ArgumentException>());
        }

        [Test]
        public void ContainerId_OfTheGroundAndTheInventory_IsNull()
        {
            Assert.That(ItemPanel.Ground.ContainerId, Is.Null);
            Assert.That(ItemPanel.Inventory.ContainerId, Is.Null);
        }

        [Test]
        public void ContainerId_OfAContainer_IsItsId()
        {
            Assert.That(ItemPanel.ForContainer("basket").ContainerId, Is.EqualTo("basket"));
        }
    }
}
