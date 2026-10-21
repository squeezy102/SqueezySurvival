using System;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>One line of the event log: when it happened and what happened (Docs/Design/UI/HUD.md, Event log).</summary>
    public sealed class EventLogEntry
    {
        /// <summary>Creates an entry.</summary>
        public EventLogEntry(GameClock time, string text, EventLogSeverity severity = EventLogSeverity.Normal)
        {
            Time = time ?? throw new ArgumentNullException(nameof(time));
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("An entry needs text.", nameof(text));
            }

            Text = text;
            Severity = severity;
        }

        /// <summary>Game time when the event happened.</summary>
        public GameClock Time { get; }

        /// <summary>What happened, in plain language.</summary>
        public string Text { get; }

        /// <summary>How the entry is marked.</summary>
        public EventLogSeverity Severity { get; }
    }
}
