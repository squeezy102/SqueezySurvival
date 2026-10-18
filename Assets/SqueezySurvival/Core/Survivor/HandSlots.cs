using System;

namespace SqueezySurvival.Core.Survivor
{
    /// <summary>
    /// Which hand each of the two hand slots holds, and which is the tool hand (Docs/Design/Systems/Equipment.md,
    /// Handedness). The left hand's slot is on the left, as the player sees the survivor's hands from behind.
    /// </summary>
    public static class HandSlots
    {
        /// <summary>The slot of the left hand.</summary>
        public const int LeftHandSlot = 0;

        /// <summary>The slot of the right hand.</summary>
        public const int RightHandSlot = 1;

        /// <summary>The hand a slot holds.</summary>
        public static Hand HandAt(int slot)
        {
            switch (slot)
            {
                case LeftHandSlot:
                    return Hand.Left;
                case RightHandSlot:
                    return Hand.Right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, "There are two hand slots, 0 and 1.");
            }
        }

        /// <summary>The tool hand: the dominant hand. The other is the offhand.</summary>
        public static Hand ToolHand(DominantHand dominantHand) => dominantHand == DominantHand.Left ? Hand.Left : Hand.Right;
    }
}
