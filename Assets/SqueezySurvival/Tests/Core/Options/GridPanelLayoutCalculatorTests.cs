using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class GridPanelLayoutCalculatorTests
    {
        // The scrollbar and the gap before it always take 20 px: 12 + 8.
        private const float Scrollbar = 12f;

        private static readonly GridPanelLayoutOptions FourColumns =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Vertical, 4, 2, false);

        private static readonly GridPanelLayoutOptions TwoRows =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Horizontal, 4, 2, false);

        [Test]
        public void Vertical_CellsFillWidthLeftAfterScrollbar()
        {
            // (444 - 20 - 3 gaps of 8) / 4 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 8, Scrollbar);

            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentWidth, Is.EqualTo(424f));
            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
        }

        [Test]
        public void CellSize_DoesNotChangeWhenCellsStartToOverflow()
        {
            GridPanelLayout fits = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 8, Scrollbar);
            GridPanelLayout overflows = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 40, Scrollbar);

            Assert.That(fits.NeedsScrolling, Is.False);
            Assert.That(overflows.NeedsScrolling, Is.True);
            Assert.That(overflows.CellSize, Is.EqualTo(fits.CellSize));
        }

        [Test]
        public void Vertical_FillsLeftToRightThenDown()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 5, Scrollbar);

            Assert.That(layout.PositionOf(3).Left, Is.EqualTo(324f));
            Assert.That(layout.PositionOf(3).Top, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Left, Is.EqualTo(0f));
            Assert.That(layout.PositionOf(4).Top, Is.EqualTo(108f));
        }

        [Test]
        public void Horizontal_CellsFillHeightLeftAfterScrollbarAndFillTopToBottomThenRight()
        {
            // (228 - 20 - 1 gap of 8) / 2 = 100.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 228f, 3, Scrollbar);

            Assert.That(layout.NeedsScrolling, Is.False);
            Assert.That(layout.CellSize, Is.EqualTo(100f));
            Assert.That(layout.ContentHeight, Is.EqualTo(208f));
            Assert.That(layout.ContentWidth, Is.EqualTo(208f));
            Assert.That(layout.PositionOf(1).Top, Is.EqualTo(108f));
            Assert.That(layout.PositionOf(2).Left, Is.EqualTo(108f));
        }

        [Test]
        public void Horizontal_WiderThanArea_NeedsScrolling()
        {
            // 5 columns of 100 px need 532 px, more than 500.
            Assert.That(GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 228f, 10, Scrollbar).NeedsScrolling, Is.True);
        }

        [Test]
        public void NoCells_DoesNotScroll()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 0, Scrollbar);

            Assert.That(layout.ContentHeight, Is.Zero);
            Assert.That(layout.NeedsScrolling, Is.False);
        }

        [Test]
        public void AreaNotLaidOutYet_GivesZeroSizedCells()
        {
            Assert.That(GridPanelLayoutCalculator.Calculate(FourColumns, float.NaN, float.NaN, 3, Scrollbar).CellSize, Is.Zero);
        }

        [Test]
        public void CellIndexAt_FindsCellAndFractionWithin()
        {
            // 100 px cells, 108 px pitch; the point is 3/4 across and 1/4 down the second row's second cell.
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 6, Scrollbar);

            int index = layout.CellIndexAt(108f + 75f, 108f + 25f, 6, out float x, out float y);

            Assert.That(index, Is.EqualTo(5));
            Assert.That(x, Is.EqualTo(0.75f));
            Assert.That(y, Is.EqualTo(0.25f));
        }

        [Test]
        public void CellIndexAt_PastLastCell_IsMinusOne()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(FourColumns, 444f, 300f, 6, Scrollbar);

            Assert.That(layout.CellIndexAt(330f, 120f, 6, out _, out _), Is.EqualTo(-1));
        }

        [Test]
        public void CellIndexAt_Horizontal_CountsDownColumns()
        {
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(TwoRows, 500f, 228f, 3, Scrollbar);

            Assert.That(layout.CellIndexAt(150f, 50f, 3, out _, out _), Is.EqualTo(2));
            Assert.That(layout.CellIndexAt(150f, 150f, 3, out _, out _), Is.EqualTo(-1));
        }

        [Test]
        public void DisplayedSlotCount_FewItems_FillsTheVisibleArea()
        {
            // 100 px cells in 300 px of height: 2 whole rows of 4 are visible.
            Assert.That(GridPanelLayoutCalculator.DisplayedSlotCount(FourColumns, 444f, 300f, Scrollbar, 3), Is.EqualTo(8));
        }

        [Test]
        public void DisplayedSlotCount_FullLastRow_AddsARowOfEmptySlots()
        {
            // 8 occupied slots fill both visible rows, so a third row offers somewhere to drop.
            Assert.That(GridPanelLayoutCalculator.DisplayedSlotCount(FourColumns, 444f, 300f, Scrollbar, 8), Is.EqualTo(12));
        }
    }
}
