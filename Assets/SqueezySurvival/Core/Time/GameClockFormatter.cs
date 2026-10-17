using System;

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

        /// <summary>The HUD clock, for example <c>Day 3 - 7:45 AM - Morning</c>.</summary>
        public static string FormatHudClock(GameClock clock, DayPhaseSchedule schedule)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            return $"Day {clock.Day} - {FormatTimeOfDay(clock.MinuteOfDay)} - {schedule.PhaseAt(clock.MinuteOfDay)}";
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
    }
}
