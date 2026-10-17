namespace SqueezySurvival.Core.Options
{
    /// <summary>Which way a grid panel scrolls (Docs/Design/UI/GridPanels.md, Scroll direction).</summary>
    public enum GridPanelScrollDirection
    {
        /// <summary>A fixed number of columns; scrolls up and down.</summary>
        Vertical,

        /// <summary>A fixed number of rows; scrolls left and right.</summary>
        Horizontal,
    }
}
