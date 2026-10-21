using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// How long an action takes, in whole seconds. Most actions take whole minutes; tiny actions take seconds
    /// (Docs/Design/Systems/Time.md, Rules). Every action takes at least 1 second. Immutable.
    /// </summary>
    public sealed class ActionDuration : IEquatable<ActionDuration>
    {
        private ActionDuration(int seconds)
        {
            Seconds = seconds;
        }

        /// <summary>Whole seconds the action takes.</summary>
        public int Seconds { get; }

        /// <summary>An action lasting whole minutes. Every such action takes at least 1 minute.</summary>
        public static ActionDuration FromMinutes(int minutes)
        {
            if (minutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "An action in minutes takes at least 1 minute.");
            }

            return new ActionDuration(checked(minutes * GameClock.SecondsPerMinute));
        }

        /// <summary>A tiny action lasting whole seconds. Every such action takes at least 1 second.</summary>
        public static ActionDuration FromSeconds(int seconds)
        {
            if (seconds < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "An action in seconds takes at least 1 second.");
            }

            return new ActionDuration(seconds);
        }

        /// <inheritdoc />
        public bool Equals(ActionDuration other) => other != null && Seconds == other.Seconds;

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ActionDuration);

        /// <inheritdoc />
        public override int GetHashCode() => Seconds;

        /// <inheritdoc />
        public override string ToString() => $"{Seconds} s";
    }
}
