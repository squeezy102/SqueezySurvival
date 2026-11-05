using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Moving things between panels (the ground, the inventory and the open container): previewing a move and carrying
    /// it out (Docs/Design/UI/DragAndDrop.md, Between panels and Destinations). How the player asked for the move is
    /// not its concern.
    /// </summary>
    public interface IInventoryTransferService
    {
        /// <summary>Whether anything can be moved now: not while a find waits for room on the ground.</summary>
        bool CanTransfer { get; }

        /// <summary>The slots of a panel things move between. A container's panel must be the open container's.</summary>
        CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel);

        /// <summary>
        /// What moving <paramref name="quantity"/> of the stack in <paramref name="fromSlot"/> of <paramref name="from"/>
        /// to <paramref name="to"/> would do, letting go over <paramref name="targetSlot"/> (-1 for no particular slot).
        /// Changes nothing.
        /// </summary>
        ItemMovePreview PreviewTransfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity);

        /// <summary>
        /// What sending the stack in <paramref name="fromSlot"/> of <paramref name="from"/> where the destination policy
        /// says would do, as a modified click does: the first place with room, or the first place when none has room.
        /// Changes nothing.
        /// </summary>
        ItemMovePreview PreviewSend(ItemPanel from, int fromSlot, ItemTransferQuantity quantity);

        /// <summary>
        /// Moves things as <see cref="PreviewTransfer"/> describes: takes its seconds and logs it. There is no weight
        /// cap. Not available while a find waits for room on the ground, or when there is no room.
        /// </summary>
        void Transfer(ItemPanel from, ItemPanel to, int fromSlot, int targetSlot, ItemTransferQuantity quantity);

        /// <summary>
        /// Moves things as <see cref="PreviewSend"/> describes: takes its seconds and logs it. Not available while a
        /// find waits for room on the ground, or when there is no room.
        /// </summary>
        void Send(ItemPanel from, int fromSlot, ItemTransferQuantity quantity);
    }
}
