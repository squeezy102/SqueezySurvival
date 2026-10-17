using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class GridPanelLayoutCalculatorTests
    {
        private const float Scrollbar = 12f;

        private static readonly GridPanelLayoutOptions FourColumns =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Vertical, 4, 2, false);

        private static readonly GridPanelLayoutOptions TwoRows =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Horizontal, 4, 2, false);

        [Test]
        public void Vertical_EverythingFits_CellsFillWidthExactlyWithoutScrolling()
        {
            // 4 cells and 3 gaps of 8 px across 424 px: (424 - 24) / 4 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 8, Scrollbar);

            Assert.That(layout.NeedsScrolling, Is.False);
            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentWidth, Is.EqualTo(424f));
            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
        }

        [Test]
        public void Vertical_Overflowing_ReservesScrollbarAndGapThenShrinksCells()
        {
            // 3 rows of 100 px cells need 316 px, more than 300, so the scrollbar and a gap take 20 px of width:
            // (424 - 20 - 24) / 4 = 95.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 9, Scrollbar);

            Assert.That(layout.NeedsScrolling, Is.True);
            Assert.That(layout.CellSize, Is.EqualTo(95f));
            Assert.That(layout.ContentWidth, Is.EqualTo(404f));
        }

        [Test]
        public void Vertical_FillsLeftToRightThenDown()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 5, Scrollbar);

            Assert.That(layout.PositionOf(3).Left, Is.EqualTo(324f));
            Assert.That(layout.PositionOf(3).Top, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Left, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Top, Is.EqualTo(108f));
        }

        [Test]
        public void Horizontal_CellsFillHeightAndFillTopToBottomThenRight()
        {
            // 2 cells and 1 gap across 208 px: (208 - 8) / 2 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 208f, 3, Scrollbar);

            Assert.That(layout.NeedsScrolling, Is.False);
            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
            Assert.That(layout.ContentWidth, Is.EqualTo(208f));
            Assert.That(layout.PositionOf(1).Top, Is.EqualTo(108f));
            Assert.That(layout.PositionOf(2).Left, Is.EqualTo(108f));
            Assert.That(layout.PositionOf(2).Top, Is.EqualTo(0f));
        }

        [Test]
        public void Horizontal_Overflowing_ScrollbarTakesHeight()
        {
            // 5 columns of 100 px need 532 px, more than 500, so the scrollbar and a gap take 20 px of height.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 208f, 10, Scrollbar);

            Assert.That(layout.NeedsScrolling, Is.True);
            Assert.That(layout.CellSize, Is.EqualTo(90f));
            Assert.That(layout.ContentHeight, Is.EqualTo(188f));
        }

        [Test]
        public void NoCells_DoesNotScroll()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 424f, 300f, 0, Scrollbar);

            Assert.That(layout.ContentHeight, Is.Zero);
            Assert.That(layout.NeedsScrolling, Is.False);
        }

        [Test]
        public void AreaNotLaidOutYet_GivesZeroSizedCells()
        {
            Assert.That(GridPanelLayoutCalculator.Calculate(FourColumns, float.NaN, float.NaN, 3, Scrollbar).CellSize, Is.Zero);
        }
    }
}
