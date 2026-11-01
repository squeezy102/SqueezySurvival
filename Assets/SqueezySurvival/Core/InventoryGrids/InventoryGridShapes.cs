using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>
    /// The size of each grid in the game (Docs/Design/UI/InventoryGrids.md, The grids). The slot counts come from
    /// <see cref="SlotCapacityRules"/>; the only layout choice made here is how many columns each grid has, and the rows
    /// follow. A capacity that does not fill whole rows is rejected rather than silently rounded.
    /// </summary>
    public sealed class InventoryGridShapes
    {
        private const int GroundColumns = 12;
        private const int ZoneFeatureColumns = 12;
        private const int SurvivorInventoryColumns = 8;
        private const int HandColumns = 2;

        /// <summary>Derives the grid shapes from <paramref name="slotCapacityRules"/>.</summary>
        /// <exception cref="ArgumentException">A capacity is not a whole number of rows.</exception>
        public InventoryGridShapes(SlotCapacityRules slotCapacityRules)
        {
            if (slotCapacityRules == null)
            {
                throw new ArgumentNullException(nameof(slotCapacityRules));
            }

            Ground = FixedGridShape.ForSlots(slotCapacityRules.GroundSlots, GroundColumns, nameof(Ground));
            ZoneFeatures = FixedGridShape.ForSlots(slotCapacityRules.ZoneFeatureSlots, ZoneFeatureColumns, nameof(ZoneFeatures));
            Inventory = FixedGridShape.ForSlots(slotCapacityRules.SurvivorInventorySlots, SurvivorInventoryColumns, nameof(Inventory));
            Hands = FixedGridShape.ForSlots(slotCapacityRules.HandSlots, HandColumns, nameof(Hands));
        }

        /// <summary>The zone's ground pool.</summary>
        public FixedGridShape Ground { get; }

        /// <summary>The zone's features.</summary>
        public FixedGridShape ZoneFeatures { get; }

        /// <summary>What the survivor carries.</summary>
        public FixedGridShape Inventory { get; }

        /// <summary>The tool hand and offhand.</summary>
        public FixedGridShape Hands { get; }
    }
}
