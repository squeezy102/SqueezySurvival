namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>The size of each grid in the game (Docs/Design/UI/InventoryGrids.md, The grids).</summary>
    public static class InventoryGridShapes
    {
        /// <summary>The zone's ground pool: 12 x 3.</summary>
        public static readonly FixedGridShape Ground = new FixedGridShape(12, 3);

        /// <summary>The zone's features: 12 x 3.</summary>
        public static readonly FixedGridShape ZoneFeatures = new FixedGridShape(12, 3);

        /// <summary>What the survivor carries: 8 slots in one row.</summary>
        public static readonly FixedGridShape Inventory = new FixedGridShape(8, 1);

        /// <summary>The tool hand and offhand.</summary>
        public static readonly FixedGridShape Hands = new FixedGridShape(2, 1);
    }
}
