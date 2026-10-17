using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Things grouped by kind, each kind with its count and a fixed grid slot. Used for a zone's ground pool and its
    /// features. A kind keeps its slot until the player moves it; slots may be left empty on purpose, and a new kind
    /// takes the first empty slot (Docs/Design/UI/DragAndDrop.md, Slots).
    /// </summary>
    public sealed class CountedCollection<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        private readonly List<CountedEntry<TDefinition>> _entries = new List<CountedEntry<TDefinition>>();

        /// <summary>Raised after the contents or their slots change.</summary>
        public event Action Changed;

        /// <summary>One entry per kind, in slot order.</summary>
        public IReadOnlyList<CountedEntry<TDefinition>> Entries => _entries;

        /// <summary>One more than the highest occupied slot: the slots that hold or lie between things. 0 when empty.</summary>
        public int SlotCount => _entries.Count == 0 ? 0 : _entries[_entries.Count - 1].Slot + 1;

        /// <summary>How many of the given kind there are.</summary>
        public int CountOf(TDefinition definition)
        {
            int index = IndexOfKind(definition);
            return index < 0 ? 0 : _entries[index].Count;
        }

        /// <summary>The entry in a slot, or null when the slot is empty.</summary>
        public CountedEntry<TDefinition> EntryAtSlot(int slot)
        {
            int index = IndexOfSlot(slot);
            return index < 0 ? null : _entries[index];
        }

        /// <summary>
        /// Adds things one at a time: a kind already present adds to its count in place; a new kind takes the first
        /// empty slot. Raises <see cref="Changed"/> once.
        /// </summary>
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

        /// <summary>
        /// Moves the thing in <paramref name="fromSlot"/> to <paramref name="toSlot"/>. If something is already there,
        /// it and the things after it shift one slot later, up to the first empty slot, which absorbs the shift; nothing
        /// moves earlier, and nothing past that gap moves (Docs/Design/UI/DragAndDrop.md, Slots).
        /// </summary>
        public void MoveToSlot(int fromSlot, int toSlot)
        {
            int fromIndex = IndexOfSlot(fromSlot);
            if (fromIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fromSlot), fromSlot, "Nothing is in that slot.");
            }

            if (toSlot < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toSlot), toSlot, "Cannot be negative.");
            }

            if (toSlot == fromSlot)
            {
                return;
            }

            CountedEntry<TDefinition> moving = _entries[fromIndex];
            _entries.RemoveAt(fromIndex);

            // The occupied run starting at the target slot moves one slot later; the run ends at the first gap,
            // which may be the slot the moving thing just left.
            int runEnd = toSlot;
            while (IndexOfSlot(runEnd) >= 0)
            {
                runEnd++;
            }

            for (int index = 0; index < _entries.Count; index++)
            {
                CountedEntry<TDefinition> entry = _entries[index];
                if (entry.Slot >= toSlot && entry.Slot < runEnd)
                {
                    _entries[index] = new CountedEntry<TDefinition>(entry.Definition, entry.Count, entry.Slot + 1);
                }
            }

            Insert(new CountedEntry<TDefinition>(moving.Definition, moving.Count, toSlot));
            Changed?.Invoke();
        }

        private void AddOne(TDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentException("Cannot add a missing definition.", nameof(definition));
            }

            int index = IndexOfKind(definition);
            if (index < 0)
            {
                Insert(new CountedEntry<TDefinition>(definition, 1, FirstEmptySlot()));
            }
            else
            {
                CountedEntry<TDefinition> entry = _entries[index];
                _entries[index] = new CountedEntry<TDefinition>(definition, entry.Count + 1, entry.Slot);
            }
        }

        // Entries are kept sorted by slot, so the first slot that doesn't match its position is the first gap.
        private int FirstEmptySlot()
        {
            for (int index = 0; index < _entries.Count; index++)
            {
                if (_entries[index].Slot != index)
                {
                    return index;
                }
            }

            return _entries.Count;
        }

        private void Insert(CountedEntry<TDefinition> entry)
        {
            int index = 0;
            while (index < _entries.Count && _entries[index].Slot < entry.Slot)
            {
                index++;
            }

            _entries.Insert(index, entry);
        }

        private int IndexOfKind(TDefinition definition)
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

        private int IndexOfSlot(int slot)
        {
            for (int index = 0; index < _entries.Count; index++)
            {
                if (_entries[index].Slot == slot)
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
