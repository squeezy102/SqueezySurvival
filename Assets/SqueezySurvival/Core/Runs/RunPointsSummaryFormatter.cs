using System;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Describes what a run earns in plain words, for the New Game screen
    /// (Docs/Design/UI/StartupFlow.md, New Game screen).
    /// </summary>
    public static class RunPointsSummaryFormatter
    {
        /// <summary>Shown when the run earns neither currency.</summary>
        public const string NoPoints = "No points";

        /// <summary>For example "1 survival point per day, 1 challenge point per 30 days".</summary>
        public static string Format(RunOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            PointEarningRate survival = RunPointEarningRules.SurvivalPointRate(options);
            PointEarningRate challenge = RunPointEarningRules.ChallengePointRate(options);
            if (survival.EarnsNothing && challenge.EarnsNothing)
            {
                return NoPoints;
            }

            return FormatRate(survival, "survival") + ", " + FormatRate(challenge, "challenge");
        }

        private static string FormatRate(PointEarningRate rate, string currency)
        {
            if (rate.EarnsNothing)
            {
                return $"no {currency} points";
            }

            string points = rate.Points == 1 ? $"1 {currency} point" : $"{rate.Points} {currency} points";
            string period = rate.Days == 1 ? "per day" : $"per {rate.Days} days";
            return points + " " + period;
        }
    }
}
