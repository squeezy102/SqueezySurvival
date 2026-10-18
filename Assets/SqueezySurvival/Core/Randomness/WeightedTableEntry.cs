using System;

namespace SqueezySurvival.Core.Randomness
{
    /// <summary>One outcome of a <see cref="WeightedTable{T}"/> and its weight.</summary>
    public sealed class WeightedTableEntry<T>
    {
        /// <summary>Creates an entry. The weight must be at least 1.</summary>
        public WeightedTableEntry(T outcome, int weight)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            if (weight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(weight), weight, "A weight must be at least 1.");
            }

            Outcome = outcome;
            Weight = weight;
        }

        /// <summary>What is picked.</summary>
        public T Outcome { get; }

        /// <summary>How likely it is, relative to the other entries.</summary>
        public int Weight { get; }
    }
}
