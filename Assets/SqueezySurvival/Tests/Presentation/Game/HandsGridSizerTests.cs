using NUnit.Framework;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>The hands grid is as wide as its cells need at the inventory row's height (#51).</summary>
    [TestFixture]
    public sealed class HandsGridSizerTests : GameScreenUITestFixture
    {
        private const float TallRowPixels = 204f;
        private const float ShortRowPixels = 120f;

        private FixedGridShape _handsShape;
        private VisualElement _handsGrid;
        private VisualElement _inventoryRow;

        [SetUp]
        public void FindTheHandsGridAndItsRow()
        {
            _handsShape = new InventoryGridShapes(SlotCapacityRules.Wireframe).Hands;
            _handsGrid = Element<VisualElement>("hands-grid");
            _inventoryRow = _handsGrid.parent;
        }

        // In the game the row's height is whatever is left of the panel (it grows from a zero basis), so the test pins it.
        private void GiveTheRowAHeight(float pixels)
        {
            _inventoryRow.style.flexGrow = 0;
            _inventoryRow.style.flexShrink = 0;
            _inventoryRow.style.flexBasis = pixels;
            simulate.FrameUpdate();
            Assert.That(_inventoryRow.layout.height, Is.EqualTo(pixels).Within(0.5f), "The row did not take the height the test gave it.");
        }

        private float WidthNow => _handsGrid.style.width.value.value;

        [Test]
        public void New_AfterTheRowWasLaidOut_SizesTheGridAtOnce()
        {
            GiveTheRowAHeight(TallRowPixels);

            new HandsGridSizer(_handsGrid, _handsShape);

            Assert.That(WidthNow, Is.EqualTo(FixedGridLayoutCalculator.WidthForHeight(_handsShape, _inventoryRow.layout.height)));
        }

        [Test]
        public void TheRowChangingHeight_ResizesTheGrid()
        {
            GiveTheRowAHeight(TallRowPixels);
            new HandsGridSizer(_handsGrid, _handsShape);

            GiveTheRowAHeight(ShortRowPixels);

            Assert.That(WidthNow, Is.EqualTo(FixedGridLayoutCalculator.WidthForHeight(_handsShape, _inventoryRow.layout.height)));
        }
    }
}
