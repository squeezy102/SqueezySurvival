namespace SqueezySurvival.Core.Items
{
    /// <summary>The orders a grid can be sorted in (Docs/Design/UI/InventoryGrids.md, Sorting).</summary>
    public enum ThingSortOrder
    {
        /// <summary>Grouped by item category, in category order, then by name. The default.</summary>
        ByType,

        /// <summary>By name.</summary>
        Alphabetical,

        /// <summary>Largest count first, then by name.</summary>
        ByQuantity,

        /// <summary>Heaviest stack first, then by name.</summary>
        ByWeight,
    }
}
