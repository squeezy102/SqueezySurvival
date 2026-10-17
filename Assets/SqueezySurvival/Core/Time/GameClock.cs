using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// A run's game time, stored as whole minutes (TU) elapsed since the run started; day and clock are derived,
    /// never stored (Docs/Design/Systems/Time.md, Implementation notes). Immutable.
    /// </summary>
    public sealed class GameClock
    {
        /// <summary>Minutes in a day: 1,440 TU.</summary>
        public const int MinutesPerDay = 24 * 60;

        /// <summary>A run starts at 6:00 AM on Day 1 (Docs/Design/Systems/Time.md, Start of a run).</summary>
        public const int RunStartMinuteOfDay = 6 * 60;

        /// <summary>The clock at the moment a run starts.</summary>
        public static readonly GameClock RunStart = new GameClock(0);

        /// <summary>Creates a clock that many minutes after the run started.</summary>
        public GameClock(long elapsedMinutes)
        {
            if (elapsedMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsedMinutes), elapsedMinutes, "Cannot be negative.");
            }

            ElapsedMinutes = elapsedMinutes;
        }

        /// <summary>Whole minutes elapsed since the run started.</summary>
        public long ElapsedMinutes { get; }

        /// <summary>The day number, starting at Day 1.</summary>
        public int Day => (int)(MinutesSinceDayOneMidnight / MinutesPerDay) + 1;

        /// <summary>Minutes since midnight of the current day.</summary>
        public int MinuteOfDay => (int)(MinutesSinceDayOneMidnight % MinutesPerDay);

        private long MinutesSinceDayOneMidnight => RunStartMinuteOfDay + ElapsedMinutes;

        /// <summary>The clock after an action of the given duration. Every action takes at least 1 minute.</summary>
        public GameClock Advance(int minutes)
        {
            if (minutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "Every action takes at least 1 minute.");
            }

            return new GameClock(ElapsedMinutes + minutes);
        }
    }
}
