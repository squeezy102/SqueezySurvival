using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.Icons;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A panel's Sort button and the arrow beside it (Docs/Design/UI/InventoryGrids.md, Sorting). Core sorts; these
    /// tests check the buttons ask for the right order and the label names it.
    /// </summary>
    [TestFixture]
    public sealed class InventoryGridSortControllerTests : GameScreenUITestFixture
    {
        private RecordingInventoryGridContents _contents;
        private InventoryGridSortController _controller;
        private Button _sortButton;
        private Button _sortMenuButton;

        [SetUp]
        public void CreateController()
        {
            _contents = new RecordingInventoryGridContents();
            _sortButton = Element<Button>("ground-pool-sort-button");
            _sortMenuButton = Element<Button>("ground-pool-sort-menu-button");
            _controller = new InventoryGridSortController(_contents, _sortButton, _sortMenuButton);
            simulate.FrameUpdate();
        }

        [TearDown]
        public void DisposeController() => _controller.Dispose();

        [Test]
        public void New_LabelsTheSortButtonWithTheTypeOrder()
        {
            Assert.That(_sortButton.text, Is.EqualTo("Sort: " + ThingSortOrderTextFormatter.Format(ThingSortOrder.ByType)));
        }

        [Test]
        public void SortButton_Click_SortsByTypeUntilAnotherOrderIsChosen()
        {
            simulate.Click(_sortButton);

            Assert.That(_contents.Sorts, Is.EqualTo(new[] { ThingSortOrder.ByType }));
        }

        [Test]
        public void ChoosingAnOrderFromTheMenu_SortsAtOnceAndMakesItThePanelsOrder()
        {
            simulate.Click(_sortMenuButton);
            simulate.FrameUpdate();
            string alphabetical = ThingSortOrderTextFormatter.Format(ThingSortOrder.Alphabetical);
            Label menuItem = rootVisualElement.Query<Label>().Where(label => label.text == alphabetical).First();
            Assert.That(menuItem, Is.Not.Null, "The sort menu did not open with an Alphabetical item.");

            simulate.Click(menuItem);
            simulate.FrameUpdate();
            simulate.Click(_sortButton);

            Assert.That(_contents.Sorts, Is.EqualTo(new[] { ThingSortOrder.Alphabetical, ThingSortOrder.Alphabetical }));
            Assert.That(_sortButton.text, Is.EqualTo("Sort: " + alphabetical));
        }

        [Test]
        public void AfterDispose_TheButtonsDoNothingAndTheArrowIsGone()
        {
            _controller.Dispose();

            simulate.Click(_sortButton);

            Assert.That(_contents.Sorts, Is.Empty);
            Assert.That(_sortMenuButton.Children().OfType<DropdownArrowIconElement>(), Is.Empty);
        }
    }
}
