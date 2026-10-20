using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Time
{
    /// <summary>Formats game time for display (Docs/Design/Systems/Time.md, Display).</summary>
    public static class GameClockFormatter
    {
        /// <summary>12-hour time with AM/PM, for example <c>7:45 AM</c>.</summary>
        public static string FormatTimeOfDay(int minuteOfDay)
        {
            if (minuteOfDay < 0 || minuteOfDay >= GameClock.MinutesPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(minuteOfDay), minuteOfDay, "Must be within the day.");
            }

            int hour24 = minuteOfDay / 60;
            int minute = minuteOfDay % 60;
            int hour12 = hour24 % 12 == 0 ? 12 : hour24 % 12;
            return $"{hour12}:{minute:00} {(hour24 < 12 ? "AM" : "PM")}";
        }

        /// <summary>12-hour time with seconds and AM/PM, for example <c>7:45:12 AM</c>.</summary>
        public static string FormatTimeOfDayWithSeconds(int secondOfDay)
        {
            if (secondOfDay < 0 || secondOfDay >= GameClock.SecondsPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(secondOfDay), secondOfDay, "Must be within the day.");
            }

            string hoursAndMinutes = FormatTimeOfDay(secondOfDay / GameClock.SecondsPerMinute);
            int second = secondOfDay % GameClock.SecondsPerMinute;
            int amOrPm = hoursAndMinutes.LastIndexOf(' ');
            return $"{hoursAndMinutes.Substring(0, amOrPm)}:{second:00}{hoursAndMinutes.Substring(amOrPm)}";
        }

        /// <summary>
        /// The HUD clock as three rows, for example <c>Day 3</c>, <c>7:45:12 AM</c> and <c>Morning</c>
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

            return new[] { $"Day {clock.Day}", FormatTimeOfDayWithSeconds(clock.SecondOfDay), schedule.PhaseAt(clock.MinuteOfDay).ToString() };
        }

        /// <summary>A duration in whole minutes, as <see cref="FormatDurationSeconds"/> writes it: <c>1 hr 30 min</c>.</summary>
        public static string FormatDuration(int minutes)
        {
            if (minutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "A duration in minutes is at least 1 minute.");
            }

            return FormatDurationSeconds(minutes * GameClock.SecondsPerMinute);
        }

        /// <summary>
        /// A duration in the largest units that apply, leaving out any that are zero: <c>10 sec</c>, <c>1 min 20 sec</c>,
        /// <c>1 hr 30 min</c>, <c>1 hr 5 sec</c> (Docs/Design/Systems/Time.md, Display).
        /// </summary>
        public static string FormatDurationSeconds(int seconds)
        {
            if (seconds < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Every action takes at least 1 second.");
            }

            int hours = seconds / 3600;
            int minutes = seconds % 3600 / GameClock.SecondsPerMinute;
            int remainder = seconds % GameClock.SecondsPerMinute;
            var parts = new List<string>(3);
            if (hours > 0)
            {
                parts.Add($"{hours} hr");
            }

            if (minutes > 0)
            {
                parts.Add($"{minutes} min");
            }

            if (remainder > 0)
            {
                parts.Add($"{remainder} sec");
            }

            return string.Join(" ", parts);
        }
    }
}
