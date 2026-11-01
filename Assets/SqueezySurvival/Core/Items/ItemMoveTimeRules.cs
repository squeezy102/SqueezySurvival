using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// How long moving things between panels takes: seconds by weight, so it is never free and never a penalty
    /// (Docs/Design/Systems/Inventory.md, Moving things). Immutable; tuning changes <see cref="Wireframe"/> and the
    /// design document together.
    /// </summary>
    public sealed class ItemMoveTimeRules
    {
        /// <summary>The wireframe stage's rate: 2 seconds a pound, a placeholder until the weight balancing pass (OQ-087).</summary>
        public static readonly ItemMoveTimeRules Wireframe = new ItemMoveTimeRules(secondsPerPound: 2m);

        /// <summary>Creates rules that take <paramref name="secondsPerPound"/> for each pound moved.</summary>
        public ItemMoveTimeRules(decimal secondsPerPound)
        {
            if (secondsPerPound <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(secondsPerPound), secondsPerPound, "Must be more than zero.");
            }

            SecondsPerPound = secondsPerPound;
        }

        /// <summary>Seconds each pound moved takes.</summary>
        public decimal SecondsPerPound { get; }

        /// <summary>The seconds a move of <paramref name="pounds"/> takes: rounded up to a whole second, at least 1.</summary>
        public int SecondsToMove(decimal pounds)
        {
            if (pounds < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(pounds), pounds, "Cannot be negative.");
            }

            return Math.Max(1, (int)decimal.Ceiling(pounds * SecondsPerPound));
        }
    }
}
