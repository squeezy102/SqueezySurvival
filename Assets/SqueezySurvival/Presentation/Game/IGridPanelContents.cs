using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// What a <see cref="GridPanelView"/> shows and acts on: a slotted collection in Core, seen without its item type
    /// so one view works for items and zone features alike.
    /// </summary>
    public interface IGridPanelContents
    {
        /// <summary>Raised after the contents or their slots change.</summary>
        event Action Changed;

        /// <summary>One more than the highest occupied slot; 0 when empty.</summary>
        int SlotCount { get; }

        /// <summary>Whether anything is in a slot.</summary>
        bool IsOccupied(int slot);

        /// <summary>The icon id of the thing in a slot.</summary>
        string DefinitionIdAt(int slot);

        /// <summary>The count badge of a slot: empty for a single thing.</summary>
        string CountBadgeAt(int slot);

        /// <summary>The hover tooltip of a slot.</summary>
        ThingTooltipContent TooltipAt(int slot);

        /// <summary>What releasing the thing in <paramref name="sourceSlot"/> over <paramref name="target"/> would do.</summary>
        GridDropOutcome ResolveDrop(int sourceSlot, GridDropTarget target, GridSlotNeighborhood neighborhood);

        /// <summary>Carries out an outcome returned by <see cref="ResolveDrop"/> for the same source slot.</summary>
        void ApplyDrop(int sourceSlot, GridDropOutcome outcome);
    }
}
