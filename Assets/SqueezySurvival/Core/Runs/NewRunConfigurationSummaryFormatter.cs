using System;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Describes a run configuration in one line, for example in the wireframe-stage notice.</summary>
    public static class NewRunConfigurationSummaryFormatter
    {
        /// <summary>For example "Right-handed, Hard mode. Day 1, 6:00 AM, Beach."</summary>
        public static string Format(NewRunConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string hand = configuration.DominantHand == DominantHand.Left ? "Left-handed" : "Right-handed";
            return $"{hand}, {DescribeMode(configuration.RunOptions)}. " +
                   $"Day {NewRunConfiguration.StartingDay}, {FormatClock(NewRunConfiguration.StartingMinuteOfDay)}, " +
                   $"{NewRunConfiguration.StartingZoneName}.";
        }

        // 12-hour clock with AM/PM (Docs/Design/Systems/Time.md, Display).
        private static string FormatClock(int minuteOfDay)
        {
            int hour24 = minuteOfDay / 60;
            int minute = minuteOfDay % 60;
            int hour12 = hour24 % 12 == 0 ? 12 : hour24 % 12;
            return $"{hour12}:{minute:00} {(hour24 < 12 ? "AM" : "PM")}";
        }

        private static string DescribeMode(RunOptions options)
        {
            if (options.IsCasual && options.IsEasy)
            {
                return "Casual and Easy mode";
            }

            if (options.IsCasual)
            {
                return "Casual mode";
            }

            if (options.IsEasy)
            {
                return "Easy mode";
            }

            return options.IsHard ? "Hard mode" : "default mode";
        }
    }
}
