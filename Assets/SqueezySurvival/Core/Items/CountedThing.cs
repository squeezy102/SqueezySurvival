using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>Some number of one kind of thing, not in any slot: for example, finds with no room on the ground. Immutable.</summary>
    public sealed class CountedThing<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        /// <summary>Creates a counted thing.</summary>
        /// <param name="definition">The kind of thing.</param>
        /// <param name="count">How many. Must be at least 1.</param>
        public CountedThing(TDefinition definition, int count)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be at least 1.");
            }

            Count = count;
        }

        /// <summary>The kind of thing.</summary>
        public TDefinition Definition { get; }

        /// <summary>How many.</summary>
        public int Count { get; }
    }
}
