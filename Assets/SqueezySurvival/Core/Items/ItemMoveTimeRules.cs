using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// How long moving things between panels takes: seconds by weight, so it is never free and never a penalty
    /// (Docs/Design/Systems/Inventory.md, Moving things).
    /// </summary>
    public static class ItemMoveTimeRules
    {
        /// <summary>Seconds per pound moved: 2, a placeholder until the weight balancing pass (OQ-087).</summary>
        public const decimal PlaceholderSecondsPerPound = 2m;

        /// <summary>The seconds a move of <paramref name="pounds"/> takes: rounded up to a whole second, at least 1.</summary>
        public static int SecondsToMove(decimal pounds)
        {
            if (pounds < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(pounds), pounds, "Cannot be negative.");
            }

            return Math.Max(1, (int)decimal.Ceiling(pounds * PlaceholderSecondsPerPound));
        }
    }
}
