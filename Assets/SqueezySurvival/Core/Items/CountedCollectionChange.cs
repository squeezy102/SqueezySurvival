using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// The slots of a <see cref="CountedCollection{TDefinition}"/> whose contents changed in one command. Says which
    /// slots, not how they changed (Docs/Engineering/Architecture.md, Commands, state and change events). Immutable.
    /// </summary>
    public sealed class CountedCollectionChange
    {
        private readonly int[] _changedSlots;

        /// <summary>Describes a change to <paramref name="changedSlots"/>, given once each in slot order.</summary>
        public CountedCollectionChange(IEnumerable<int> changedSlots)
        {
            if (changedSlots == null)
            {
                throw new ArgumentNullException(nameof(changedSlots));
            }

            _changedSlots = new List<int>(changedSlots).ToArray();
        }

        /// <summary>Each slot whose kind or count changed, once each, in slot order.</summary>
        public IReadOnlyList<int> ChangedSlots => _changedSlots;
    }
}
