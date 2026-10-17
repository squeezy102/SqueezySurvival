using System;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// How many survival and challenge points a run earns, by run options
    /// (Docs/Design/Systems/Runs.md, Points earned and Run options).
    /// </summary>
    public static class RunPointEarningRules
    {
        /// <summary>Days in a month for challenge points.</summary>
        public const int DaysPerMonth = 30;

        /// <summary>Survival points: 1 per day; Easy 1 per 5 days; Hard 2 per day; Casual none.</summary>
        public static PointEarningRate SurvivalPointRate(RunOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.IsCasual)
            {
                return PointEarningRate.None;
            }

            if (options.IsEasy)
            {
                return new PointEarningRate(1, 5);
            }

            return options.IsHard ? new PointEarningRate(2, 1) : new PointEarningRate(1, 1);
        }

        /// <summary>Challenge points: 1 per 30 days; Hard 2 per 30 days; Easy and Casual none.</summary>
        public static PointEarningRate ChallengePointRate(RunOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.IsCasual || options.IsEasy)
            {
                return PointEarningRate.None;
            }

            return new PointEarningRate(options.IsHard ? 2 : 1, DaysPerMonth);
        }
    }
}
