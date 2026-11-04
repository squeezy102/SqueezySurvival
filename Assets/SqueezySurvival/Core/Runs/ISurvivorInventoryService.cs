using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// What the survivor carries: the inventory, both hands, and the load they make together
    /// (Docs/Design/Systems/Inventory.md, Load; Docs/Design/Systems/Equipment.md).
    /// </summary>
    public interface ISurvivorInventoryService
    {
        /// <summary>The survivor's inventory slots (Docs/Design/Systems/Inventory.md, Slots).</summary>
        CountedCollection<ItemDefinition> Inventory { get; }

        /// <summary>The tool hand and offhand (Docs/Design/Systems/Equipment.md).</summary>
        CountedCollection<ItemDefinition> Hands { get; }

        /// <summary>The inventory and both hands against the weight limit. There is no cap; carrying too much slows and then stops the survivor.</summary>
        SurvivorLoad Load { get; }

        /// <summary>Whether the survivor can move: not at or past the weight limit (Docs/Design/Systems/Inventory.md, Load).</summary>
        bool CanMove { get; }
    }
}
