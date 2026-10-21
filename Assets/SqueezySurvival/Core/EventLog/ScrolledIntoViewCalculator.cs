namespace SqueezySurvival.Core.EventLog
{
    /// <summary>
    /// Whether a scrolled element is fully in view, which is when a warning counts as read
    /// (Docs/Design/UI/HUD.md, Event log). Positions are in the same vertical space, growing downward.
    /// </summary>
    public static class ScrolledIntoViewCalculator
    {
        /// <summary>How far, in UI pixels, an edge may sit outside the view and still count as in view.</summary>
        public const float EdgeTolerance = 0.5f;

        /// <summary>Whether the span from <paramref name="top"/> to <paramref name="bottom"/> lies within the view.</summary>
        public static bool IsFullyInView(float top, float bottom, float viewTop, float viewBottom) =>
            top >= viewTop - EdgeTolerance && bottom <= viewBottom + EdgeTolerance;

        /// <summary>Whether a scroll position is at, or within tolerance of, its end.</summary>
        public static bool IsScrolledToEnd(float scrollPosition, float scrollEnd) => scrollPosition >= scrollEnd - EdgeTolerance;
    }
}
