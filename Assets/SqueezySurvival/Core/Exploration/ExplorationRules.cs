using System;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>
    /// The numbers that govern an explore (Docs/Design/Systems/Exploration.md). Immutable; tuning changes
    /// <see cref="Wireframe"/> and the design document together.
    /// </summary>
    public sealed class ExplorationRules
    {
        /// <summary>The wireframe stage's rules (Docs/Design/Systems/Exploration.md, Wireframe stage).</summary>
        public static readonly ExplorationRules Wireframe = new ExplorationRules(
            exploreMinutes: 15,
            minimumFindsPerExplore: 1,
            maximumFindsPerExplore: 3,
            featureChancePercent: 25);

        /// <summary>Creates a set of rules.</summary>
        public ExplorationRules(int exploreMinutes, int minimumFindsPerExplore, int maximumFindsPerExplore, int featureChancePercent)
        {
            if (exploreMinutes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(exploreMinutes), exploreMinutes, "Every action takes at least 1 minute.");
            }

            if (minimumFindsPerExplore < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumFindsPerExplore), minimumFindsPerExplore, "Every explore finds something.");
            }

            if (maximumFindsPerExplore < minimumFindsPerExplore)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumFindsPerExplore), maximumFindsPerExplore, "Cannot be less than the minimum.");
            }

            if (featureChancePercent < 0 || featureChancePercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(featureChancePercent), featureChancePercent, "Must be from 0 to 100.");
            }

            ExploreMinutes = exploreMinutes;
            MinimumFindsPerExplore = minimumFindsPerExplore;
            MaximumFindsPerExplore = maximumFindsPerExplore;
            FeatureChancePercent = featureChancePercent;
        }

        /// <summary>How long one explore takes, in minutes (TU).</summary>
        public int ExploreMinutes { get; }

        /// <summary>Fewest loose items a random explore finds.</summary>
        public int MinimumFindsPerExplore { get; }

        /// <summary>Most loose items a random explore finds.</summary>
        public int MaximumFindsPerExplore { get; }

        /// <summary>Chance, in percent, that a random explore of a zone not yet fully explored also reveals a feature.</summary>
        public int FeatureChancePercent { get; }
    }
}
