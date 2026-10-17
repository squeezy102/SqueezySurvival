using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>A kind of thing, how many there are, and the grid slot it occupies. Immutable.</summary>
    public sealed class CountedEntry<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        /// <summary>Creates an entry. The count must be at least 1 and the slot cannot be negative.</summary>
        public CountedEntry(TDefinition definition, int count, int slot)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be at least 1.");
            }

            if (slot < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, "Cannot be negative.");
            }

            Count = count;
            Slot = slot;
        }

        /// <summary>What it is.</summary>
        public TDefinition Definition { get; }

        /// <summary>How many there are.</summary>
        public int Count { get; }

        /// <summary>The grid slot it sits in, counted in fill order from 0 (Docs/Design/UI/DragAndDrop.md, Slots).</summary>
        public int Slot { get; }
    }
}
