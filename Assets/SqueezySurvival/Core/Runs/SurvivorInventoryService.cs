using System;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>What the survivor carries, kept in slot collections sized by the slot capacity rules.</summary>
    public sealed class SurvivorInventoryService : ISurvivorInventoryService
    {
        /// <summary>Starts with an empty inventory and empty hands, sized by <paramref name="slotCapacityRules"/>.</summary>
        public SurvivorInventoryService(SlotCapacityRules slotCapacityRules)
        {
            if (slotCapacityRules == null)
            {
                throw new ArgumentNullException(nameof(slotCapacityRules));
            }

            Inventory = new CountedCollection<ItemDefinition>(slotCapacityRules.SurvivorInventorySlots);
            Hands = new CountedCollection<ItemDefinition>(slotCapacityRules.HandSlots);
        }

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> Inventory { get; }

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> Hands { get; }

        /// <inheritdoc />
        public SurvivorLoad Load => new SurvivorLoad(WeightOf(Inventory) + WeightOf(Hands), SurvivorLoad.PlaceholderLimitPounds);

        /// <inheritdoc />
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
