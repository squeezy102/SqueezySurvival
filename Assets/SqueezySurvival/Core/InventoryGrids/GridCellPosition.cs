namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>The top-left corner of a grid cell, in pixels from the grid area's top-left corner. Immutable.</summary>
    public readonly struct GridCellPosition
    {
        /// <summary>Creates a position.</summary>
        public GridCellPosition(float left, float top)
        {
            Left = left;
            Top = top;
        }

        /// <summary>Pixels from the left edge.</summary>
        public float Left { get; }

        /// <summary>Pixels from the top edge.</summary>
        public float Top { get; }
    }
}
