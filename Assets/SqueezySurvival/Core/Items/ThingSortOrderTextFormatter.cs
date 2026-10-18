using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>The label shown for each sort order in a grid's sort menu (Docs/Design/UI/InventoryGrids.md, Sorting).</summary>
    public static class ThingSortOrderTextFormatter
    {
        /// <summary>The order's label, such as "By type".</summary>
        public static string Format(ThingSortOrder order)
        {
            switch (order)
            {
                case ThingSortOrder.ByType:
                    return "By type";
                case ThingSortOrder.Alphabetical:
                    return "Alphabetical";
                case ThingSortOrder.ByQuantity:
                    return "By quantity";
                case ThingSortOrder.ByWeight:
                    return "By weight";
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, "Unknown sort order.");
            }
        }
    }
}
