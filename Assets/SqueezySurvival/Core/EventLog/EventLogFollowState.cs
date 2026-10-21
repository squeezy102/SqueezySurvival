namespace SqueezySurvival.Core.EventLog
{
    /// <summary>
    /// Whether the event log follows new entries as they arrive: it does while scrolled to the bottom, stops once the
    /// player scrolls up or goes to an older entry, and starts again at the bottom or on jump to newest
    /// (Docs/Design/UI/HUD.md, Event log).
    /// </summary>
    public sealed class EventLogFollowState
    {
        /// <summary>Whether new entries should be scrolled into view as they arrive. A new log follows.</summary>
        public bool FollowsNewest { get; private set; } = true;

        /// <summary>Notes where the player's scroll left the log: following resumes only at the bottom.</summary>
        public void PlayerScrolled(bool isScrolledToBottom)
        {
            FollowsNewest = isScrolledToBottom;
        }

        /// <summary>Notes that the player jumped to the newest entry.</summary>
        public void JumpedToNewest()
        {
            FollowsNewest = true;
        }

        /// <summary>Notes that the log was taken to an older entry, such as an unread warning.</summary>
        public void WentToOlderEntry()
        {
            FollowsNewest = false;
        }
    }
}
