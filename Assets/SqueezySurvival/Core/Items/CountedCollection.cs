using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// A fixed number of slots, each holding one kind of thing and its count, like Diablo's inventory
    /// (Docs/Design/UI/InventoryGrids.md). A kind keeps its slot until the player moves it; slots may be left empty on
    /// purpose, and a new kind takes the first empty slot. When every slot is taken, a new kind does not fit; more of a
    /// kind already present always does.
    /// </summary>
    public sealed class CountedCollection<TDefinition> where TDefinition : class, ICountableThingDefinition
    {
        private readonly List<CountedEntry<TDefinition>> _entries = new List<CountedEntry<TDefinition>>();

        /// <summary>Creates an empty collection with the given number of slots.</summary>
        /// <param name="slotCapacity">How many slots, and so how many kinds, it holds. Must be at least 1.</param>
        public CountedCollection(int slotCapacity)
        {
            if (slotCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(slotCapacity), slotCapacity, "Must be at least 1.");
            }

            SlotCapacity = slotCapacity;
        }

        /// <summary>Raised after the contents or their slots change.</summary>
        public event Action Changed;

        /// <summary>How many slots there are.</summary>
        public int SlotCapacity { get; }

        /// <summary>One entry per kind, in slot order.</summary>
        public IReadOnlyList<CountedEntry<TDefinition>> Entries => _entries;

        /// <summary>Whether every slot is taken.</summary>
        public bool IsFull => _entries.Count == SlotCapacity;

        /// <summary>How many of the given kind there are.</summary>
        public int CountOf(TDefinition definition)
        {
            int index = IndexOfKind(definition);
            return index < 0 ? 0 : _entries[index].Count;
        }

        /// <summary>Whether the given kind fits: it is already present, or there is an empty slot for it.</summary>
        public bool HasRoomFor(TDefinition definition) => IndexOfKind(definition) >= 0 || !IsFull;

        /// <summary>The entry in a slot, or null when the slot is empty.</summary>
        public CountedEntry<TDefinition> EntryAtSlot(int slot)
        {
            RequireSlot(slot, nameof(slot));
            int index = IndexOfSlot(slot);
            return index < 0 ? null : _entries[index];
        }

        /// <summary>
        /// Adds things one at a time: a kind already present adds to its count in place; a new kind takes the first
        /// empty slot. Things of a new kind that do not fit are returned, grouped by kind in the order they arrived,
        /// and are not added. Raises <see cref="Changed"/> once if anything was added.
        /// </summary>
        public IReadOnlyList<CountedThing<TDefinition>> AddRange(IEnumerable<TDefinition> definitions)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            bool changed = false;
            var overflowKinds = new List<TDefinition>();
            var overflowCounts = new Dictionary<string, int>();
            foreach (TDefinition definition in definitions)
            {
                if (definition == null)
                {
                    throw new ArgumentException("Cannot add a missing definition.", nameof(definitions));
                }

                if (TryAddOne(definition))
                {
                    changed = true;
                }
                else if (overflowCounts.TryGetValue(definition.Id, out int count))
                {
                    overflowCounts[definition.Id] = count + 1;
                }
                else
                {
                    overflowKinds.Add(definition);
                    overflowCounts[definition.Id] = 1;
                }
            }

            if (changed)
            {
                Changed?.Invoke();
            }

            var overflow = new List<CountedThing<TDefinition>>(overflowKinds.Count);
            foreach (TDefinition kind in overflowKinds)
            {
                overflow.Add(new CountedThing<TDefinition>(kind, overflowCounts[kind.Id]));
            }

            return overflow;
        }

        /// <summary>
        /// Moves the thing in <paramref name="fromSlot"/> to <paramref name="toSlot"/>. If something is already there,
        /// the two swap slots; nothing else moves (Docs/Design/UI/DragAndDrop.md, Drop zones).
        /// </summary>
        public void MoveToSlot(int fromSlot, int toSlot)
        {
            RequireSlot(fromSlot, nameof(fromSlot));
            RequireSlot(toSlot, nameof(toSlot));
            int fromIndex = IndexOfSlot(fromSlot);
            if (fromIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fromSlot), fromSlot, "Nothing is in that slot.");
            }

            if (toSlot == fromSlot)
            {
                return;
            }

            CountedEntry<TDefinition> moving = _entries[fromIndex];
            int toIndex = IndexOfSlot(toSlot);
            CountedEntry<TDefinition> displaced = toIndex < 0 ? null : _entries[toIndex];
            _entries.Remove(moving);
            if (displaced != null)
            {
                _entries.Remove(displaced);
                Insert(new CountedEntry<TDefinition>(displaced.Definition, displaced.Count, fromSlot));
            }

            Insert(new CountedEntry<TDefinition>(moving.Definition, moving.Count, toSlot));
            Changed?.Invoke();
        }

        /// <summary>
        /// Throws away what is in <paramref name="slot"/> and puts <paramref name="incoming"/>, a kind not already
        /// present, in its place (Docs/Design/UI/InventoryGrids.md, When the ground is full). Returns what was thrown away.
        /// </summary>
        public CountedEntry<TDefinition> ReplaceSlot(int slot, CountedThing<TDefinition> incoming)
        {
            if (incoming == null)
            {
                throw new ArgumentNullException(nameof(incoming));
            }

            RequireSlot(slot, nameof(slot));
            int index = IndexOfSlot(slot);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, "Nothing is in that slot.");
            }

            if (IndexOfKind(incoming.Definition) >= 0)
            {
                throw new InvalidOperationException("That kind is already present; add to it instead.");
            }

            CountedEntry<TDefinition> discarded = _entries[index];
            _entries[index] = new CountedEntry<TDefinition>(incoming.Definition, incoming.Count, slot);
            Changed?.Invoke();
            return discarded;
        }

        /// <summary>
        /// Packs everything into the first slots, closing gaps, in the order <paramref name="order"/> gives
        /// (Docs/Design/UI/InventoryGrids.md, Sorting). Raises <see cref="Changed"/>.
        /// </summary>
        public void Sort(IComparer<CountedEntry<TDefinition>> order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            var sorted = new List<CountedEntry<TDefinition>>(_entries);

            // List.Sort is unstable; slot order breaks ties so equal things keep their relative order.
            sorted.Sort((first, second) =>
            {
                int comparison = order.Compare(first, second);
                return comparison != 0 ? comparison : first.Slot.CompareTo(second.Slot);
            });

            _entries.Clear();
            for (int slot = 0; slot < sorted.Count; slot++)
            {
                _entries.Add(new CountedEntry<TDefinition>(sorted[slot].Definition, sorted[slot].Count, slot));
            }

            Changed?.Invoke();
        }

        private bool TryAddOne(TDefinition definition)
        {
            int index = IndexOfKind(definition);
            if (index >= 0)
            {
                CountedEntry<TDefinition> entry = _entries[index];
                _entries[index] = new CountedEntry<TDefinition>(definition, entry.Count + 1, entry.Slot);
                return true;
            }

            if (IsFull)
            {
                return false;
            }

            Insert(new CountedEntry<TDefinition>(definition, 1, FirstEmptySlot()));
            return true;
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
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

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

        private void RequireSlot(int slot, string parameterName)
        {
            if (slot < 0 || slot >= SlotCapacity)
            {
                throw new ArgumentOutOfRangeException(parameterName, slot, $"Must be from 0 to {SlotCapacity - 1}.");
            }
        }
    }
}
