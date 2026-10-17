using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The run's full event history, oldest first (Docs/Design/UI/HUD.md, Event log).</summary>
    public sealed class RunEventLog
    {
        private readonly List<EventLogEntry> _entries = new List<EventLogEntry>();

        /// <summary>Raised after an entry is added.</summary>
        public event Action<EventLogEntry> EntryAdded;

        /// <summary>Every entry, oldest first.</summary>
        public IReadOnlyList<EventLogEntry> Entries => _entries;

        /// <summary>Adds an entry at the given time.</summary>
        public void Add(GameClock time, string text)
        {
            var entry = new EventLogEntry(time, text);
            _entries.Add(entry);
            EntryAdded?.Invoke(entry);
        }
    }
}
