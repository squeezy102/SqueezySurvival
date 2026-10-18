using System;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// How much of a zone has been explored. Stored as explores completed; the percentage is derived. Each explore
    /// adds 100% divided by the zone's explores to map, up to 100% (Docs/Design/Systems/Exploration.md, Pace).
    /// Immutable.
    /// </summary>
    public sealed class ExplorationProgress
    {
        /// <summary>Creates progress for a zone that takes <paramref name="exploresToMap"/> explores to map fully.</summary>
        public ExplorationProgress(int exploresCompleted, int exploresToMap)
        {
            if (exploresCompleted < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exploresCompleted), exploresCompleted, "Cannot be negative.");
            }

            if (exploresToMap < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(exploresToMap), exploresToMap, "Must be at least 1.");
            }

            ExploresCompleted = exploresCompleted;
            ExploresToMap = exploresToMap;
        }

        /// <summary>Every explore made in the zone, including those after it was fully explored.</summary>
        public int ExploresCompleted { get; }

        /// <summary>Explores needed to map the zone fully.</summary>
        public int ExploresToMap { get; }

        /// <summary>Whether the zone is fully explored.</summary>
        public bool IsFullyExplored => ExploresCompleted >= ExploresToMap;

        /// <summary>Percentage explored, rounded down so 100% appears only when the zone is fully explored.</summary>
        public int WholePercent => IsFullyExplored ? 100 : ExploresCompleted * 100 / ExploresToMap;

        /// <summary>The progress after one more explore.</summary>
        public ExplorationProgress AfterOneExplore() => new ExplorationProgress(ExploresCompleted + 1, ExploresToMap);
    }
}
