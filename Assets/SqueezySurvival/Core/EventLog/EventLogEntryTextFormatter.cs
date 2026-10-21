using System;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.EventLog
{
    /// <summary>The line the event log shows for an entry: "Day 1, 6:04:20 AM: You picked up 5 stones."</summary>
    public static class EventLogEntryTextFormatter
    {
        /// <summary>The entry's day, time of day and text.</summary>
        public static string Format(EventLogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return $"Day {entry.Time.Day}, {GameClockFormatter.FormatTimeOfDay(entry.Time)}: {entry.Text}";
        }
    }
}
