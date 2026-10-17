using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class GridPanelLayoutCalculatorTests
    {
        private static readonly GridPanelLayoutOptions FourColumns =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Vertical, 4, 2, false);

        private static readonly GridPanelLayoutOptions TwoRows =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Horizontal, 4, 2, false);

        [Test]
        public void Vertical_CellsFillViewportWidthExactly()
        {
            // 4 cells and 3 gaps of 8 px across 424 px: (424 - 24) / 4 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 1);

            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentWidth, Is.EqualTo(424f));
        }

        [Test]
        public void Vertical_ContentHeightGrowsByRow()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 5);

            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
        }

        [Test]
        public void Vertical_FillsLeftToRightThenDown()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 5);

            Assert.That(layout.PositionOf(3).Left, Is.EqualTo(324f));
            Assert.That(layout.PositionOf(3).Top, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Left, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Top, Is.EqualTo(108f));
        }

        [Test]
        public void Horizontal_CellsFillViewportHeightAndFillTopToBottomThenRight()
        {
            // 2 cells and 1 gap across 208 px: (208 - 8) / 2 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 208f, 3);

            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
            Assert.That(layout.ContentWidth, Is.EqualTo(208f));
            Assert.That(layout.PositionOf(1).Top, Is.EqualTo(108f));
            Assert.That(layout.PositionOf(2).Left, Is.EqualTo(108f));
            Assert.That(layout.PositionOf(2).Top, Is.EqualTo(0f));
        }

        [Test]
        public void NoCells_HasNoScrollingExtent()
        {
            Assert.That(GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 0).ContentHeight, Is.Zero);
        }

        [Test]
        public void ViewportNotLaidOutYet_GivesZeroSizedCells()
        {
            Assert.That(GridPanelLayoutCalculator.Calculate(FourColumns, float.NaN, float.NaN, 3).CellSize, Is.Zero);
        }
    }
}
