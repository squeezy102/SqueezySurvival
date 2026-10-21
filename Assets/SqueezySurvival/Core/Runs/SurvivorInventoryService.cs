using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// What the survivor carries: the inventory, both hands, and the load they make together
    /// (Docs/Design/Systems/Inventory.md, Load; Docs/Design/Systems/Equipment.md).
    /// </summary>
    public sealed class SurvivorInventoryService
    {
        /// <summary>Starts with an empty inventory and empty hands.</summary>
        public SurvivorInventoryService()
        {
            Inventory = new CountedCollection<ItemDefinition>(InventoryGridShapes.Inventory.SlotCount);
            Hands = new CountedCollection<ItemDefinition>(InventoryGridShapes.Hands.SlotCount);
        }

        /// <summary>The survivor's inventory slots (Docs/Design/Systems/Inventory.md, Slots).</summary>
        public CountedCollection<ItemDefinition> Inventory { get; }

        /// <summary>The tool hand and offhand (Docs/Design/Systems/Equipment.md).</summary>
        public CountedCollection<ItemDefinition> Hands { get; }

        /// <summary>The inventory and both hands against the weight limit. There is no cap; carrying too much slows and then stops the survivor.</summary>
        public SurvivorLoad Load => new SurvivorLoad(WeightOf(Inventory) + WeightOf(Hands), SurvivorLoad.PlaceholderLimitPounds);

        /// <summary>Whether the survivor can move: not at or past the weight limit (Docs/Design/Systems/Inventory.md, Load).</summary>
        public bool CanMove => Load.State != SurvivorLoadState.Immobile;

        private static decimal WeightOf(CountedCollection<ItemDefinition> collection)
        {
            decimal pounds = 0m;
            foreach (CountedEntry<ItemDefinition> entry in collection.Entries)
            {
                pounds += entry.Definition.WeightPounds * entry.Count;
            }

            return pounds;
        }
    }
}
