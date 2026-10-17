using System;
using System.Collections.Generic;
using System.Linq;

namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// When each day phase starts. Phase boundaries are data so they can be tuned without code changes
    /// (Docs/Design/Systems/Time.md, Day phases).
    /// </summary>
    public sealed class DayPhaseSchedule
    {
        /// <summary>The schedule in Time.md: Dawn 5 AM, Morning 7 AM, Midday 11 AM, Afternoon 2 PM, Dusk 6 PM, Night 10 PM.</summary>
        public static readonly DayPhaseSchedule Default = new DayPhaseSchedule(new Dictionary<DayPhase, int>
        {
            { DayPhase.Dawn, 5 * 60 },
            { DayPhase.Morning, 7 * 60 },
            { DayPhase.Midday, 11 * 60 },
            { DayPhase.Afternoon, 14 * 60 },
            { DayPhase.Dusk, 18 * 60 },
            { DayPhase.Night, 22 * 60 },
        });

        private readonly KeyValuePair<DayPhase, int>[] _phaseStartsInOrder;

        /// <summary>Creates a schedule from the minute of the day each phase starts.</summary>
        /// <exception cref="ArgumentException">A phase is missing, or a start is outside the day.</exception>
        public DayPhaseSchedule(IReadOnlyDictionary<DayPhase, int> phaseStartMinutes)
        {
            if (phaseStartMinutes == null)
            {
                throw new ArgumentNullException(nameof(phaseStartMinutes));
            }

            foreach (DayPhase phase in Enum.GetValues(typeof(DayPhase)))
            {
                if (!phaseStartMinutes.TryGetValue(phase, out int start) || start < 0 || start >= GameClock.MinutesPerDay)
                {
                    throw new ArgumentException($"{phase} needs a start minute within the day.", nameof(phaseStartMinutes));
                }
            }

            _phaseStartsInOrder = phaseStartMinutes.OrderBy(entry => entry.Value).ToArray();
        }

        /// <summary>The phase in effect at a minute of the day.</summary>
        public DayPhase PhaseAt(int minuteOfDay)
        {
            if (minuteOfDay < 0 || minuteOfDay >= GameClock.MinutesPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(minuteOfDay), minuteOfDay, "Must be within the day.");
            }

            // Before the earliest start, the latest phase (Night) is still running from the previous day.
            DayPhase phase = _phaseStartsInOrder[_phaseStartsInOrder.Length - 1].Key;
            foreach (KeyValuePair<DayPhase, int> start in _phaseStartsInOrder)
            {
                if (minuteOfDay >= start.Value)
                {
                    phase = start.Key;
                }
            }

            return phase;
        }
    }
}
