using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>
    /// Keeps the event log's unread warnings, oldest first, so the log can alert the player and take them to each one
    /// in turn (Docs/Design/UI/HUD.md, Event log).
    /// </summary>
    public sealed class UnreadWarningTracker
    {
        private readonly List<EventLogEntry> _unread = new List<EventLogEntry>();

        /// <summary>Raised whenever the unread warnings change.</summary>
        public event Action Changed;

        /// <summary>How many warnings are unread.</summary>
        public int UnreadCount => _unread.Count;

        /// <summary>Whether any warning is unread.</summary>
        public bool HasUnread => _unread.Count > 0;

        /// <summary>The oldest unread warning, or null when every warning has been read.</summary>
        public EventLogEntry OldestUnread => _unread.Count > 0 ? _unread[0] : null;

        /// <summary>The unread warnings, oldest first.</summary>
        public IReadOnlyList<EventLogEntry> Unread => _unread;

        /// <summary>Notes a newly logged entry. Warnings start unread; other entries are ignored.</summary>
        public void Record(EventLogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry.Severity != EventLogSeverity.Warning || _unread.Contains(entry))
            {
                return;
            }

            _unread.Add(entry);
            Changed?.Invoke();
        }

        /// <summary>Marks a warning read. Entries that are not unread warnings are ignored.</summary>
        public void MarkRead(EventLogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (_unread.Remove(entry))
            {
                Changed?.Invoke();
            }
        }
    }
}
