using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Moving things between the survivor's inventory and the ground: previewing a move and carrying it out
    /// (Docs/Design/UI/DragAndDrop.md, Between panels). How the player asked for the move is not its concern.
    /// </summary>
    public interface IInventoryTransferService
    {
        /// <summary>Whether anything can be moved now: not while a find waits for room on the ground.</summary>
        bool CanTransfer { get; }

        /// <summary>The slots of a panel things move between.</summary>
        CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel);

        /// <summary>
        /// What moving <paramref name="quantity"/> of the stack in <paramref name="fromSlot"/> to the other panel would
        /// do, letting go over <paramref name="targetSlot"/> (-1 for no particular slot). Changes nothing.
        /// </summary>
        ItemMovePreview PreviewTransfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity);

        /// <summary>
        /// Moves things to the other panel as <see cref="PreviewTransfer"/> describes: takes its seconds and logs it.
        /// There is no weight cap. Not available while a find waits for room on the ground, or when there is no room.
        /// </summary>
        void Transfer(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity);
    }
}
