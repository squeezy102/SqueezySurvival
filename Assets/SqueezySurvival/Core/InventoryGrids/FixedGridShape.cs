using System;

namespace SqueezySurvival.Core.InventoryGrids
{
    /// <summary>A grid's columns and rows; slots fill left to right, then down. Immutable.</summary>
    public sealed class FixedGridShape
    {
        /// <summary>Creates a shape.</summary>
        /// <param name="columns">Slots per row. Must be at least 1.</param>
        /// <param name="rows">Rows. Must be at least 1.</param>
        public FixedGridShape(int columns, int rows)
        {
            if (columns < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), columns, "Must be at least 1.");
            }

            if (rows < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), rows, "Must be at least 1.");
            }

            Columns = columns;
            Rows = rows;
        }

        /// <summary>Slots per row.</summary>
        public int Columns { get; }

        /// <summary>Rows.</summary>
        public int Rows { get; }

        /// <summary>How many slots the grid has.</summary>
        public int SlotCount => Columns * Rows;

        /// <summary>The shape that holds exactly <paramref name="slotCount"/> slots in rows of <paramref name="columns"/>.</summary>
        /// <exception cref="ArgumentException">The slots do not fill whole rows.</exception>
        public static FixedGridShape ForSlots(int slotCount, int columns, string gridName)
        {
            if (columns < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), columns, "Must be at least 1.");
            }

            if (slotCount < 1 || slotCount % columns != 0)
            {
                throw new ArgumentException(
                    $"The {gridName} grid has {columns} columns, so its {slotCount} slots do not fill whole rows.",
                    nameof(slotCount));
            }

            return new FixedGridShape(columns, slotCount / columns);
        }
    }
}
