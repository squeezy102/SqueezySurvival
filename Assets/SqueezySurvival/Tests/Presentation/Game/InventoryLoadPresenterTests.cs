using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class InventoryLoadPresenterTests : GameScreenUITestFixture
    {
        private SurvivorInventoryService _survivorInventory;
        private InventoryLoadPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _survivorInventory = new SurvivorInventoryService();
            _presenter = new InventoryLoadPresenter(rootVisualElement, _survivorInventory);
        }

        [TearDown]
        public void DisposePresenter() => _presenter.Dispose();

        [Test]
        public void New_ShowsAnEmptyLoad()
        {
            Assert.That(Element<Label>("inventory-load").text, Is.EqualTo("Load: 0 lb of 100 lb (0%), Normal"));
        }

        [Test]
        public void InventoryChanged_ShowsTheNewLoad()
        {
            _survivorInventory.Inventory.Receive(PlaceholderBeachZoneContent.Log, 1, -1);

            Assert.That(Element<Label>("inventory-load").text, Is.EqualTo("Load: 30 lb of 100 lb (30%), Normal"));
        }

        [Test]
        public void HandsChanged_ShowsTheNewLoad()
        {
            _survivorInventory.Hands.Receive(PlaceholderBeachZoneContent.Log, 1, 0);

            Assert.That(Element<Label>("inventory-load").text, Is.EqualTo("Load: 30 lb of 100 lb (30%), Normal"));
        }

        [Test]
        public void AfterDispose_StopsFollowingTheLoad()
        {
            _presenter.Dispose();

            _survivorInventory.Inventory.Receive(PlaceholderBeachZoneContent.Log, 1, -1);

            Assert.That(Element<Label>("inventory-load").text, Is.EqualTo("Load: 0 lb of 100 lb (0%), Normal"));
        }
    }
}
