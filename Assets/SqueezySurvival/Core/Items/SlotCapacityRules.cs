using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// How many slots each place that holds things has: the survivor's inventory and hands, a zone's ground and its
    /// features (Docs/Design/Systems/Inventory.md, Slots; Docs/Design/Systems/Zones.md, Capacity). Immutable; tuning
    /// changes <see cref="Wireframe"/> and the design documents together. Screen layout is derived from these, never the
    /// other way round.
    /// </summary>
    public sealed class SlotCapacityRules
    {
        /// <summary>The wireframe stage's capacities.</summary>
        public static readonly SlotCapacityRules Wireframe = new SlotCapacityRules(
            survivorInventorySlots: 8,
            handSlots: 2,
            groundSlots: 36,
            zoneFeatureSlots: 36);

        /// <summary>Creates a set of capacities.</summary>
        public SlotCapacityRules(int survivorInventorySlots, int handSlots, int groundSlots, int zoneFeatureSlots)
        {
            SurvivorInventorySlots = RequireAtLeastOne(survivorInventorySlots, nameof(survivorInventorySlots));
            HandSlots = RequireAtLeastOne(handSlots, nameof(handSlots));
            GroundSlots = RequireAtLeastOne(groundSlots, nameof(groundSlots));
            ZoneFeatureSlots = RequireAtLeastOne(zoneFeatureSlots, nameof(zoneFeatureSlots));
        }

        /// <summary>Slots in the survivor's inventory, not counting the hands.</summary>
        public int SurvivorInventorySlots { get; }

        /// <summary>Slots for the tool hand and offhand.</summary>
        public int HandSlots { get; }

        /// <summary>Slots in a zone's ground pool.</summary>
        public int GroundSlots { get; }

        /// <summary>Slots for a zone's features.</summary>
        public int ZoneFeatureSlots { get; }

        private static int RequireAtLeastOne(int slots, string parameterName)
        {
            if (slots < 1)
            {
                throw new ArgumentOutOfRangeException(parameterName, slots, "Must be at least 1.");
            }

            return slots;
        }
    }
}
