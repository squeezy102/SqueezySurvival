using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The ground in the zone the survivor is in, and the finds waiting for room on it
    /// (Docs/Design/UI/InventoryGrids.md, When the ground is full).
    /// </summary>
    public interface IGroundInventoryService
    {
        /// <summary>Raised after finds are queued in, or removed from, <see cref="PendingOverflow"/>.</summary>
        event Action PendingOverflowChanged;

        /// <summary>What is on the ground in the current zone.</summary>
        CountedCollection<ItemDefinition> Ground { get; }

        /// <summary>
        /// Finds with no room on the ground, waiting for the player to choose what to throw away, first one first.
        /// While any wait, no time-taking action can be taken.
        /// </summary>
        IReadOnlyList<CountedThing<ItemDefinition>> PendingOverflow { get; }

        /// <summary>Whether a find is waiting for the player to choose what to throw away.</summary>
        bool HasPendingOverflow { get; }

        /// <summary>Queues finds that had no room on the ground. Does nothing for none.</summary>
        void QueueOverflow(IReadOnlyList<CountedThing<ItemDefinition>> finds);

        /// <summary>Leaves the first waiting find behind, gone for good, and logs it. Takes no time.</summary>
        void LeavePendingOverflowBehind();

        /// <summary>
        /// Throws away what is in <paramref name="groundSlot"/>, gone for good, puts the first waiting find in its
        /// place, and logs it. Takes no time.
        /// </summary>
        void DiscardToMakeRoomForPendingOverflow(int groundSlot);
    }
}
