using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridSlotNeighborhoodTests
    {
        // A vertical grid of 4 columns: slot 5 is row 1, column 1.
        [TestCase(5, CellDropZone.Left, 4)]
        [TestCase(5, CellDropZone.Right, 6)]
        [TestCase(5, CellDropZone.Top, 1)]
        [TestCase(5, CellDropZone.Bottom, 9)]
        [TestCase(4, CellDropZone.Left, -1)]
        [TestCase(7, CellDropZone.Right, -1)]
        [TestCase(2, CellDropZone.Top, -1)]
        public void NeighborOf_VerticalGrid(int slot, CellDropZone edge, int expected)
        {
            Assert.That(new GridSlotNeighborhood(GridPanelScrollDirection.Vertical, 4).NeighborOf(slot, edge), Is.EqualTo(expected));
        }

        // A horizontal grid of 3 rows: slot 4 is column 1, row 1.
        [TestCase(4, CellDropZone.Top, 3)]
        [TestCase(4, CellDropZone.Bottom, 5)]
        [TestCase(4, CellDropZone.Left, 1)]
        [TestCase(4, CellDropZone.Right, 7)]
        [TestCase(5, CellDropZone.Bottom, -1)]
        [TestCase(1, CellDropZone.Left, -1)]
        public void NeighborOf_HorizontalGrid(int slot, CellDropZone edge, int expected)
        {
            Assert.That(new GridSlotNeighborhood(GridPanelScrollDirection.Horizontal, 3).NeighborOf(slot, edge), Is.EqualTo(expected));
        }
    }
}
