using System;

namespace SqueezySurvival.Core.Randomness
{
    /// <summary>A <see cref="IRandomNumberSource"/> backed by <see cref="Random"/>.</summary>
    public sealed class SystemRandomNumberSource : IRandomNumberSource
    {
        private readonly Random _random;

        /// <summary>Creates a source whose sequence is fixed by the seed: the same seed gives the same numbers.</summary>
        public SystemRandomNumberSource(int seed)
        {
            _random = new Random(seed);
        }

        /// <inheritdoc />
        public int NextInt(int minimumInclusive, int maximumExclusive)
        {
            if (maximumExclusive <= minimumInclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumExclusive), maximumExclusive, "Must be greater than the minimum.");
            }

            return _random.Next(minimumInclusive, maximumExclusive);
        }
    }
}
