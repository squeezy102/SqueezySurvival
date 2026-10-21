using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>One advance of game time: the clock before, the clock after, and the action's duration. Immutable.</summary>
    public sealed class GameTimeAdvance
    {
        /// <summary>Describes an advance by <paramref name="duration"/> starting at <paramref name="previous"/>.</summary>
        public GameTimeAdvance(GameClock previous, ActionDuration duration)
        {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            Duration = duration ?? throw new ArgumentNullException(nameof(duration));
            Current = previous.Advance(duration);
        }

        /// <summary>The clock before the action.</summary>
        public GameClock Previous { get; }

        /// <summary>The clock after the action.</summary>
        public GameClock Current { get; }

        /// <summary>How long the action took. Time-dependent systems use this, never the difference of two clocks.</summary>
        public ActionDuration Duration { get; }
    }
}
