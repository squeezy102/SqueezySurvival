using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Randomness
{
    /// <summary>
    /// A set of outcomes, each with a whole-number weight. An outcome's chance of being picked is its weight divided
    /// by the total weight. Immutable.
    /// </summary>
    public sealed class WeightedTable<T>
    {
        private readonly List<WeightedTableEntry<T>> _entries;

        /// <summary>Creates a table. Needs at least one entry; every weight must be at least 1.</summary>
        public WeightedTable(IEnumerable<WeightedTableEntry<T>> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            _entries = new List<WeightedTableEntry<T>>(entries);
            if (_entries.Count == 0)
            {
                throw new ArgumentException("A weighted table needs at least one entry.", nameof(entries));
            }

            foreach (WeightedTableEntry<T> entry in _entries)
            {
                TotalWeight += entry.Weight;
            }
        }

        /// <summary>The entries, in the order given.</summary>
        public IReadOnlyList<WeightedTableEntry<T>> Entries => _entries;

        /// <summary>The sum of every entry's weight.</summary>
        public int TotalWeight { get; }

        /// <summary>Picks one outcome at random, in proportion to the weights.</summary>
        public T Pick(IRandomNumberSource randomNumberSource)
        {
            if (randomNumberSource == null)
            {
                throw new ArgumentNullException(nameof(randomNumberSource));
            }

            int roll = randomNumberSource.NextInt(0, TotalWeight);
            foreach (WeightedTableEntry<T> entry in _entries)
            {
                if (roll < entry.Weight)
                {
                    return entry.Outcome;
                }

                roll -= entry.Weight;
            }

            throw new InvalidOperationException("The random number source returned a value outside the requested range.");
        }
    }
}
