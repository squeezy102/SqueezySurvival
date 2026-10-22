using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// What an <see cref="InventoryGridView"/> shows and acts on: a slotted collection in Core, seen without its item
    /// type so one view works for items and zone features alike.
    /// </summary>
    public interface IInventoryGridContents
    {
        /// <summary>Raised after the contents change, naming the slots that changed.</summary>
        event EventHandler<CountedCollectionChange> Changed;

        /// <summary>How many slots there are.</summary>
        int SlotCapacity { get; }

        /// <summary>Whether anything is in a slot.</summary>
        bool IsOccupied(int slot);

        /// <summary>The icon id of the thing in a slot.</summary>
        string DefinitionIdAt(int slot);

        /// <summary>The count badge of a slot: empty for a single thing.</summary>
        string CountBadgeAt(int slot);

        /// <summary>The hover tooltip of a slot.</summary>
        ThingTooltipContent TooltipAt(int slot);

        /// <summary>
        /// What releasing <paramref name="quantity"/> of the stack in <paramref name="sourceSlot"/> over
        /// <paramref name="target"/> would do.
        /// </summary>
        GridDropOutcome ResolveDrop(int sourceSlot, GridDropTarget target, ItemTransferQuantity quantity);

        /// <summary>Carries out an outcome returned by <see cref="ResolveDrop"/> for the same source slot.</summary>
        void ApplyDrop(int sourceSlot, GridDropOutcome outcome);

        /// <summary>Packs the contents into the first slots in the given order (Docs/Design/UI/InventoryGrids.md, Sorting).</summary>
        void Sort(ThingSortOrder order);
    }
}
