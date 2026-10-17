using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// A collection of things grouped by kind, with how many of each there are, in the order each kind first arrived.
    /// Used for a zone's ground pool and its features.
    /// </summary>
    public sealed class CountedCollection<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        private readonly List<CountedEntry<TDefinition>> _entries = new List<CountedEntry<TDefinition>>();

        /// <summary>Raised after the contents change.</summary>
        public event Action Changed;

        /// <summary>One entry per kind, in the order each kind first arrived.</summary>
        public IReadOnlyList<CountedEntry<TDefinition>> Entries => _entries;

        /// <summary>How many of the given kind there are.</summary>
        public int CountOf(TDefinition definition)
        {
            int index = IndexOf(definition);
            return index < 0 ? 0 : _entries[index].Count;
        }

        /// <summary>Adds things, one at a time or several of the same kind, and raises <see cref="Changed"/> once.</summary>
        public void AddRange(IEnumerable<TDefinition> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            bool changed = false;
            foreach (TDefinition definition in definitions)
            {
                AddOne(definition);
                changed = true;
            }

            if (changed)
            {
                Changed?.Invoke();
            }
        }

        private void AddOne(TDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentException("Cannot add a missing definition.", nameof(definition));
            }

            int index = IndexOf(definition);
            if (index < 0)
            {
                _entries.Add(new CountedEntry<TDefinition>(definition, 1));
            }
            else
            {
                _entries[index] = new CountedEntry<TDefinition>(definition, _entries[index].Count + 1);
            }
        }

        private int IndexOf(TDefinition definition)
        {
            for (int index = 0; index < _entries.Count; index++)
            {
                if (_entries[index].Definition.Id == definition.Id)
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
