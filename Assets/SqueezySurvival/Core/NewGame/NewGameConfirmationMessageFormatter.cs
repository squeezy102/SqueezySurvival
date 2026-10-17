using System;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.NewGame
{
    /// <summary>
    /// The last-chance confirmation shown when the player chooses Start on the New Game screen
    /// (Docs/Design/UI/StartupFlow.md, Start confirmation).
    /// </summary>
    public static class NewGameConfirmationMessageFormatter
    {
        /// <summary>For example "You are starting a Hard mode game. You will earn ... Are you sure?"</summary>
        public static string Format(RunOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return $"You are starting {DescribeGame(options)}. {DescribeEarnings(options)} Are you sure?";
        }

        private static string DescribeGame(RunOptions options)
        {
            if (options.IsCasual && options.IsEasy)
            {
                return "a Casual and Easy mode game";
            }

            if (options.IsCasual)
            {
                return "a Casual mode game";
            }

            if (options.IsEasy)
            {
                return "an Easy mode game";
            }

            return options.IsHard ? "a Hard mode game" : "an unmodified default game";
        }

        private static string DescribeEarnings(RunOptions options)
        {
            PointEarningRate survival = RunPointEarningRules.SurvivalPointRate(options);
            PointEarningRate challenge = RunPointEarningRules.ChallengePointRate(options);
            if (survival.EarnsNothing && challenge.EarnsNothing)
            {
                return "You will be awarded no points for this adventure.";
            }

            return $"You will earn {DescribeRate(survival, "survival")}, and {DescribeRate(challenge, "challenge")}.";
        }

        private static string DescribeRate(PointEarningRate rate, string currency)
        {
            if (rate.EarnsNothing)
            {
                return $"no {currency} points";
            }

            string points = rate.Points == 1 ? $"1 {currency} point" : $"{rate.Points} {currency} points";
            string period = rate.Days == 1 ? "per day survived" : $"per {rate.Days} days survived";
            return points + " " + period;
        }
    }
}
