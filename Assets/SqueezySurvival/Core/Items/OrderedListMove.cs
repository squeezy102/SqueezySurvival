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
    }
}
