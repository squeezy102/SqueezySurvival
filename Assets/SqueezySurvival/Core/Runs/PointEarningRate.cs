using System;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>A rate at which a run earns points: a number of points per a number of days survived.</summary>
    public sealed class PointEarningRate
    {
        /// <summary>A rate that earns nothing.</summary>
        public static readonly PointEarningRate None = new PointEarningRate(0, 1);

        /// <summary>Creates a rate of <paramref name="points"/> per <paramref name="days"/> days.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Points are negative or days are not positive.</exception>
        public PointEarningRate(int points, int days)
        {
            if (points < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points), points, "Points cannot be negative.");
            }

            if (days < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(days), days, "Days must be at least 1.");
            }

            Points = points;
            Days = days;
        }

        /// <summary>Points earned per period.</summary>
        public int Points { get; }

        /// <summary>Days survived per period.</summary>
        public int Days { get; }

        /// <summary>True when the rate earns nothing.</summary>
        public bool EarnsNothing => Points == 0;
    }
}
