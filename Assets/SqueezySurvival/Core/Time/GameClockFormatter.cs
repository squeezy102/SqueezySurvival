using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Time
{
    /// <summary>Formats game time for display (Docs/Design/Systems/Time.md, Display).</summary>
    public static class GameClockFormatter
    {
        /// <summary>12-hour time with AM/PM and seconds, for example <c>7:45:20 AM</c> (Docs/Design/Systems/Time.md, Display).</summary>
        public static string FormatTimeOfDay(GameClock clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            int secondOfDay = clock.SecondOfDay;
            int hour24 = secondOfDay / 3600;
            int minute = (secondOfDay / GameClock.SecondsPerMinute) % 60;
            int second = secondOfDay % GameClock.SecondsPerMinute;
            int hour12 = hour24 % 12 == 0 ? 12 : hour24 % 12;
            return $"{hour12}:{minute:00}:{second:00} {(hour24 < 12 ? "AM" : "PM")}";
        }

        /// <summary>
        /// The HUD clock as three rows, for example <c>Day 3</c>, <c>7:45:20 AM</c> and <c>Morning</c>
        /// (Docs/Design/Systems/Time.md, Display).
        /// </summary>
        public static IReadOnlyList<string> FormatHudClockRows(GameClock clock, DayPhaseSchedule schedule)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            return new[] { $"Day {clock.Day}", FormatTimeOfDay(clock), schedule.PhaseAt(clock.MinuteOfDay).ToString() };
        }

        /// <summary>An action's duration as hours and minutes, for example <c>1 hr 30 min</c>.</summary>
        public static string FormatDuration(int minutes)
        {
            if (minutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "Durations are at least 1 minute.");
            }

            int hours = minutes / 60;
            int remainder = minutes % 60;
            if (hours == 0)
            {
                return $"{remainder} min";
            }

            return remainder == 0 ? $"{hours} hr" : $"{hours} hr {remainder} min";
        }

        /// <summary>A tiny action's duration in seconds, for example <c>10 sec</c> or <c>1 min 5 sec</c>.</summary>
        public static string FormatDurationSeconds(int seconds)
        {
            if (seconds < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Durations are at least 1 second.");
            }

            int minutes = seconds / GameClock.SecondsPerMinute;
            int remainder = seconds % GameClock.SecondsPerMinute;
            if (minutes == 0)
            {
                return $"{remainder} sec";
            }

            return remainder == 0 ? $"{minutes} min" : $"{minutes} min {remainder} sec";
        }
    }
}
