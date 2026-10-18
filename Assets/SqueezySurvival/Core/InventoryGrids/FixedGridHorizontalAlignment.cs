namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>Where a fixed grid sits across its area when the area is wider than the grid.</summary>
    public enum FixedGridHorizontalAlignment
    {
        /// <summary>Centered, like the ground and zone feature grids.</summary>
        Center,

        /// <summary>Against the left edge, like the inventory beside the hands.</summary>
        Left,
    }
}
