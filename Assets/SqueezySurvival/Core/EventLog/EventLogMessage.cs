using System;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The text of an event log entry and how it is marked, before it is given a time. Immutable.</summary>
    public sealed class EventLogMessage
    {
        /// <summary>Creates a message.</summary>
        public EventLogMessage(string text, EventLogSeverity severity = EventLogSeverity.Normal)
        {
            Text = string.IsNullOrWhiteSpace(text) ? throw new ArgumentException("A message needs text.", nameof(text)) : text;
            Severity = severity;
        }

        /// <summary>What happened.</summary>
        public string Text { get; }

        /// <summary>How it is marked.</summary>
        public EventLogSeverity Severity { get; }
    }
}
