using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The run's full event history, kept in memory.</summary>
    public sealed class RunEventLog : IRunEventLog
    {
        private readonly List<EventLogEntry> _entries = new List<EventLogEntry>();

        /// <inheritdoc />
        public event Action<EventLogEntry> EntryAdded;

        /// <inheritdoc />
        public IReadOnlyList<EventLogEntry> Entries => _entries;

        /// <inheritdoc />
        public void Add(GameClock time, string text, EventLogSeverity severity = EventLogSeverity.Normal)
        {
            var entry = new EventLogEntry(time, text, severity);
            _entries.Add(entry);
            EntryAdded?.Invoke(entry);
        }
    }
}
