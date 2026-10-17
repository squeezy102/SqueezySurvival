namespace SqueezySurvival.Core.Items
{
    /// <summary>The arithmetic of moving one entry of an ordered list to just before another position.</summary>
    public static class OrderedListMove
    {
        /// <summary>
        /// Where an entry at <paramref name="fromIndex"/> ends up when inserted just before the entry now at
        /// <paramref name="insertionIndex"/> (or at the end, when that equals the count). Removing it first shifts
        /// later entries back by one.
        /// </summary>
        public static int ResultingIndex(int fromIndex, int insertionIndex) =>
            insertionIndex > fromIndex ? insertionIndex - 1 : insertionIndex;

        /// <summary>
        /// Where the entry now at <paramref name="index"/> would sit if the entry at <paramref name="fromIndex"/> moved
        /// to <paramref name="toIndex"/>: the dragged entry takes <paramref name="toIndex"/>, and the entries between
        /// shift by one to make room. Used to preview a move before it happens.
        /// </summary>
        public static int SlotAfterMove(int index, int fromIndex, int toIndex)
        {
            if (index == fromIndex)
            {
                return toIndex;
            }

            if (fromIndex < toIndex && index > fromIndex && index <= toIndex)
            {
                return index - 1;
            }

            if (fromIndex > toIndex && index >= toIndex && index < fromIndex)
            {
                return index + 1;
            }

            return index;
        }

        /// <summary>The entry shown in <paramref name="slot"/> when the entry at <paramref name="fromIndex"/> is previewed at <paramref name="toIndex"/>.</summary>
        public static int IndexAtSlotAfterMove(int slot, int fromIndex, int toIndex)
        {
            if (slot == toIndex)
            {
                return fromIndex;
            }

            if (fromIndex < toIndex && slot >= fromIndex && slot < toIndex)
            {
                return slot + 1;
            }

            if (fromIndex > toIndex && slot > toIndex && slot <= fromIndex)
            {
                return slot - 1;
            }

            return slot;
        }
    }
}
