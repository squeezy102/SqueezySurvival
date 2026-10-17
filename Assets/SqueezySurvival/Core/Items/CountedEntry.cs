using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>A kind of thing and how many there are. Immutable.</summary>
    public sealed class CountedEntry<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        /// <summary>Creates an entry. The count must be at least 1.</summary>
        public CountedEntry(TDefinition definition, int count)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be at least 1.");
            }

            Count = count;
        }

        /// <summary>What it is.</summary>
        public TDefinition Definition { get; }

        /// <summary>How many there are.</summary>
        public int Count { get; }
    }
}
