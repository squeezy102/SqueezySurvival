using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The run's full event history, oldest first (Docs/Design/UI/HUD.md, Event log).</summary>
    public interface IRunEventLog
    {
        /// <summary>Raised after an entry is added.</summary>
        event Action<EventLogEntry> EntryAdded;

        /// <summary>Every entry, oldest first.</summary>
        IReadOnlyList<EventLogEntry> Entries { get; }

        /// <summary>Adds an entry at the given time.</summary>
        void Add(GameClock time, string text, EventLogSeverity severity = EventLogSeverity.Normal);
    }
}
