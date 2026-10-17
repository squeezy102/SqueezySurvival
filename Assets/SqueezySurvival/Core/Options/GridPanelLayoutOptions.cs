using System;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// One grid panel's settings: scroll direction, columns, rows and wheel inversion
    /// (Docs/Design/UI/GridPanels.md, Settings). Columns and rows are both kept, so switching direction back restores
    /// the earlier layout. Immutable; each change makes a new instance.
    /// </summary>
    public sealed class GridPanelLayoutOptions
    {
        /// <summary>Fewest columns a vertical panel can have.</summary>
        public const int MinimumColumnCount = 4;

        /// <summary>Most columns a vertical panel can have.</summary>
        public const int MaximumColumnCount = 8;

        /// <summary>Fewest rows a horizontal panel can have.</summary>
        public const int MinimumRowCount = 2;

        /// <summary>Most rows a horizontal panel can have.</summary>
        public const int MaximumRowCount = 4;

        /// <summary>A new player's settings: vertical, 6 columns (3 rows when horizontal), wheel not inverted.</summary>
        public static readonly GridPanelLayoutOptions Default =
            new GridPanelLayoutOptions(GridPanelScrollDirection.Vertical, columnCount: 6, rowCount: 3, invertScrollWheel: false);

        /// <summary>Creates a panel's settings. Counts must be inside their limits.</summary>
        public GridPanelLayoutOptions(GridPanelScrollDirection scrollDirection, int columnCount, int rowCount, bool invertScrollWheel)
        {
            if (!Enum.IsDefined(typeof(GridPanelScrollDirection), scrollDirection))
            {
                throw new ArgumentOutOfRangeException(nameof(scrollDirection), scrollDirection, "Unknown scroll direction.");
            }

            if (columnCount < MinimumColumnCount || columnCount > MaximumColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), columnCount, $"Must be from {MinimumColumnCount} to {MaximumColumnCount}.");
            }

            if (rowCount < MinimumRowCount || rowCount > MaximumRowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, $"Must be from {MinimumRowCount} to {MaximumRowCount}.");
            }

            ScrollDirection = scrollDirection;
            ColumnCount = columnCount;
            RowCount = rowCount;
            InvertScrollWheel = invertScrollWheel;
        }

        /// <summary>Which way the panel scrolls.</summary>
        public GridPanelScrollDirection ScrollDirection { get; }

        /// <summary>Columns when scrolling vertically.</summary>
        public int ColumnCount { get; }

        /// <summary>Rows when scrolling horizontally.</summary>
        public int RowCount { get; }

        /// <summary>Whether the mouse wheel scrolls the opposite way.</summary>
        public bool InvertScrollWheel { get; }

        /// <summary>The count that is fixed for the current direction: columns when vertical, rows when horizontal.</summary>
        public int FixedLineCount => ScrollDirection == GridPanelScrollDirection.Vertical ? ColumnCount : RowCount;

        /// <summary>Smallest allowed value of <see cref="FixedLineCount"/> for the current direction.</summary>
        public int MinimumFixedLineCount => ScrollDirection == GridPanelScrollDirection.Vertical ? MinimumColumnCount : MinimumRowCount;

        /// <summary>Largest allowed value of <see cref="FixedLineCount"/> for the current direction.</summary>
        public int MaximumFixedLineCount => ScrollDirection == GridPanelScrollDirection.Vertical ? MaximumColumnCount : MaximumRowCount;

        /// <summary>These settings with a different scroll direction.</summary>
        public GridPanelLayoutOptions WithScrollDirection(GridPanelScrollDirection scrollDirection) =>
            new GridPanelLayoutOptions(scrollDirection, ColumnCount, RowCount, InvertScrollWheel);

        /// <summary>These settings with the current direction's fixed count changed, kept inside its limits.</summary>
        public GridPanelLayoutOptions WithFixedLineCount(int fixedLineCount)
        {
            int clamped = Math.Max(MinimumFixedLineCount, Math.Min(MaximumFixedLineCount, fixedLineCount));
            return ScrollDirection == GridPanelScrollDirection.Vertical
                ? new GridPanelLayoutOptions(ScrollDirection, clamped, RowCount, InvertScrollWheel)
                : new GridPanelLayoutOptions(ScrollDirection, ColumnCount, clamped, InvertScrollWheel);
        }

        /// <summary>These settings with the wheel inversion changed.</summary>
        public GridPanelLayoutOptions WithInvertScrollWheel(bool invertScrollWheel) =>
            new GridPanelLayoutOptions(ScrollDirection, ColumnCount, RowCount, invertScrollWheel);

        /// <summary>Whether two settings are the same.</summary>
        public bool HasSameValuesAs(GridPanelLayoutOptions other) =>
            other != null
            && other.ScrollDirection == ScrollDirection
            && other.ColumnCount == ColumnCount
            && other.RowCount == RowCount
            && other.InvertScrollWheel == InvertScrollWheel;
    }
}
