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
    }
}
