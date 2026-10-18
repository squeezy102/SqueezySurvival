using NUnit.Framework;
using SqueezySurvival.Core.InventoryGrids;

namespace SqueezySurvival.Core.Tests.InventoryGrids
{
    [TestFixture]
    public sealed class FixedGridLayoutCalculatorTests
    {
        // 4 x 2 cells of 40 px with 8 px gaps make a grid of 184 x 88 px.
        private static readonly FixedGridShape FourByTwo = new FixedGridShape(4, 2);

        [Test]
        public void Calculate_WideArea_HeightLimitsCellsAndGridIsCenteredAcross()
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 284f, 88f);

            Assert.That(layout.CellSize, Is.EqualTo(40f));
            Assert.That((layout.GridLeft, layout.GridTop), Is.EqualTo((50f, 0f)));
        }

        [Test]
        public void Calculate_TallArea_WidthLimitsCellsAndGridIsCenteredDown()
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 184f, 188f);

            Assert.That(layout.CellSize, Is.EqualTo(40f));
            Assert.That((layout.GridLeft, layout.GridTop), Is.EqualTo((0f, 50f)));
        }

        [Test]
        public void Calculate_BeforeLayout_HasNoCells()
        {
            Assert.That(FixedGridLayoutCalculator.Calculate(FourByTwo, float.NaN, float.NaN).CellSize, Is.Zero);
        }

        [Test]
        public void PositionOf_FillsLeftToRightThenDown()
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 184f, 88f);

            GridCellPosition position = layout.PositionOf(5);

            Assert.That((position.Left, position.Top), Is.EqualTo((48f, 48f)));
        }

        [TestCase(47f, 10f, 0)]
        [TestCase(48f, 10f, 1)]
        [TestCase(183f, 87f, 7)]
        public void SlotAt_EachCellOwnsTheGapAfterIt(float left, float top, int expectedSlot)
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 184f, 88f);

            Assert.That(layout.SlotAt(left, top, out _, out _), Is.EqualTo(expectedSlot));
        }

        // The grid is centered 50 px in from each side of a 284 px area: 49 is in the margin, 50 is its edge.
        [TestCase(49f, -1)]
        [TestCase(50f, 0)]
        [TestCase(234f, 3)]
        [TestCase(235f, -1)]
        public void SlotAt_TheMarginAroundACenteredGrid_IsNoSlot(float left, int expectedSlot)
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 284f, 88f);

            Assert.That(layout.SlotAt(left, 10f, out _, out _), Is.EqualTo(expectedSlot));
        }

        [Test]
        public void SlotAt_GivesWhereInTheCellThePointIs()
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 184f, 88f);

            layout.SlotAt(58f, 78f, out float xFraction, out float yFraction);

            Assert.That((xFraction, yFraction), Is.EqualTo((0.25f, 0.75f)));
        }

        [Test]
        public void Calculate_LeftAligned_PutsTheGridAgainstTheLeftEdge()
        {
            FixedGridLayout layout = FixedGridLayoutCalculator.Calculate(FourByTwo, 284f, 88f, FixedGridHorizontalAlignment.Left);

            Assert.That((layout.CellSize, layout.GridLeft), Is.EqualTo((40f, 0f)));
        }

        [Test]
        public void WidthForHeight_GivesTheWidthWhoseCellsTheHeightLimits()
        {
            float width = FixedGridLayoutCalculator.WidthForHeight(FourByTwo, 88f);

            Assert.That(width, Is.EqualTo(184f));
            Assert.That(FixedGridLayoutCalculator.Calculate(FourByTwo, width, 88f).CellSize, Is.EqualTo(40f));
        }
    }
}
