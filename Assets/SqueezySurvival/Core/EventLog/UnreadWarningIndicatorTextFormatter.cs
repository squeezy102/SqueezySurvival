namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The words on the event log's unread warning marker (Docs/Design/UI/HUD.md, Event log).</summary>
    public static class UnreadWarningIndicatorTextFormatter
    {
        /// <summary>"!" for one unread warning, "! 3" for three, and nothing when none are unread.</summary>
        public static string Format(int unreadCount)
        {
            if (unreadCount <= 0)
            {
                return string.Empty;
            }

            return unreadCount == 1 ? "!" : $"! {unreadCount}";
        }
    }
}
