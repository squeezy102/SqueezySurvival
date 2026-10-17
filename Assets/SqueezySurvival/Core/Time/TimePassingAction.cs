using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>Actions whose only purpose is to pass time (Docs/Design/Systems/Time.md, Passing time deliberately).</summary>
    public enum TimePassingAction
    {
        Wait,
        Rest,
    }

    /// <summary>Durations and log text of the time-passing actions.</summary>
    public static class TimePassingActions
    {
        /// <summary>Wait: 15 minutes.</summary>
        public const int WaitMinutes = 15;

        /// <summary>Rest: 1 hour.</summary>
        public const int RestMinutes = 60;

        /// <summary>How long the action takes, in minutes.</summary>
        public static int DurationMinutes(TimePassingAction action)
        {
            switch (action)
            {
                case TimePassingAction.Wait:
                    return WaitMinutes;
                case TimePassingAction.Rest:
                    return RestMinutes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, "Unknown time-passing action.");
            }
        }

        /// <summary>The event log entry for the action (Docs/Design/UI/HUD.md, Voice).</summary>
        public static string LogText(TimePassingAction action)
        {
            switch (action)
            {
                case TimePassingAction.Wait:
                    return "You waited 15 minutes.";
                case TimePassingAction.Rest:
                    return "You rested for an hour.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, "Unknown time-passing action.");
            }
        }
    }
}
