using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// A run's game time, stored as whole seconds elapsed since the run started; day and clock are derived, never
    /// stored (Docs/Design/Systems/Time.md, Implementation notes). Immutable.
    /// </summary>
    public sealed class GameClock
    {
        /// <summary>Seconds in a minute.</summary>
        public const int SecondsPerMinute = 60;

        /// <summary>Minutes in a day: 1,440 TU.</summary>
        public const int MinutesPerDay = 24 * 60;

        /// <summary>Seconds in a day.</summary>
        public const int SecondsPerDay = MinutesPerDay * SecondsPerMinute;

        /// <summary>A run starts at 6:00 AM on Day 1 (Docs/Design/Systems/Time.md, Start of a run).</summary>
        public const int RunStartMinuteOfDay = 6 * 60;

        /// <summary>The clock at the moment a run starts.</summary>
        public static readonly GameClock RunStart = new GameClock(0);

        /// <summary>Creates a clock that many seconds after the run started.</summary>
        public GameClock(long elapsedSeconds)
        {
            if (elapsedSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsedSeconds), elapsedSeconds, "Cannot be negative.");
            }

            ElapsedSeconds = elapsedSeconds;
        }

        /// <summary>Whole seconds since the run started.</summary>
        public long ElapsedSeconds { get; }

        /// <summary>Whole minutes (TU) since the run started; seconds past the last whole minute are not counted.</summary>
        public long ElapsedMinutes => ElapsedSeconds / SecondsPerMinute;

        /// <summary>The day of the run, starting at 1.</summary>
        public int Day => (int)(SecondsSinceDayOneMidnight / SecondsPerDay) + 1;

        /// <summary>Seconds since midnight, from 0 to 86,399.</summary>
        public int SecondOfDay => (int)(SecondsSinceDayOneMidnight % SecondsPerDay);

        /// <summary>Whole minutes since midnight, from 0 to 1,439.</summary>
        public int MinuteOfDay => SecondOfDay / SecondsPerMinute;

        private long SecondsSinceDayOneMidnight => (RunStartMinuteOfDay * (long)SecondsPerMinute) + ElapsedSeconds;

        /// <summary>The clock after an action lasting whole minutes. Every such action takes at least 1 minute.</summary>
        public GameClock AdvanceMinutes(int minutes)
        {
            if (minutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "An action in minutes takes at least 1 minute.");
            }

            return new GameClock(ElapsedSeconds + ((long)minutes * SecondsPerMinute));
        }

        /// <summary>The clock after a tiny action lasting whole seconds. Every such action takes at least 1 second.</summary>
        public GameClock AdvanceSeconds(int seconds)
        {
            if (seconds < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "An action in seconds takes at least 1 second.");
            }

            return new GameClock(ElapsedSeconds + seconds);
        }
    }
}
