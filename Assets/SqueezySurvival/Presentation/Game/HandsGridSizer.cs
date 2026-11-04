using System;
using SqueezySurvival.Core.InventoryGrids;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Keeps the hands grid as wide as its cells need at the inventory row's height, so its cells match the inventory's.
    /// </summary>
    public sealed class HandsGridSizer
    {
        private readonly VisualElement _handsGrid;
        private readonly FixedGridShape _handsShape;

        /// <summary>Sizes <paramref name="handsGrid"/>, shaped <paramref name="handsShape"/>, now and whenever its row changes size.</summary>
        public HandsGridSizer(VisualElement handsGrid, FixedGridShape handsShape)
        {
            _handsGrid = handsGrid ?? throw new ArgumentNullException(nameof(handsGrid));
            _handsShape = handsShape ?? throw new ArgumentNullException(nameof(handsShape));

            // Composition runs after the first layout pass, whose geometry event has already fired, so the width is
            // also set from the current layout (#51).
            VisualElement inventoryRow = handsGrid.parent;
            inventoryRow.RegisterCallback<GeometryChangedEvent>(changed => Size(changed.newRect.height));
            Size(inventoryRow.layout.height);
        }

        private void Size(float rowHeight)
        {
            // Before its first layout pass an element's height is NaN; the geometry event sizes it once known.
            if (!float.IsNaN(rowHeight))
            {
                _handsGrid.style.width = FixedGridLayoutCalculator.WidthForHeight(_handsShape, rowHeight);
            }
        }
    }
}
