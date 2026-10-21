using System;
using System.Collections;
using System.Collections.Generic;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows a Core read-only list to a <c>ListView</c>, whose <c>itemsSource</c> is a non-generic <see cref="IList"/>,
    /// without copying it or letting the view change it. Reads go straight to the Core list, so items Core adds appear on
    /// the next <c>RefreshItems</c>.
    /// </summary>
    public sealed class ReadOnlyListViewItemsSource<T> : IList
        where T : class
    {
        private readonly IReadOnlyList<T> _items;

        /// <summary>Wraps <paramref name="items"/>.</summary>
        public ReadOnlyListViewItemsSource(IReadOnlyList<T> items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        /// <inheritdoc />
        public int Count => _items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <inheritdoc />
        public bool IsFixedSize => false;

        /// <inheritdoc />
        public bool IsSynchronized => false;

        /// <inheritdoc />
        public object SyncRoot => this;

        /// <inheritdoc />
        public object this[int index]
        {
            get => _items[index];
            set => throw ReadOnly();
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            if (value is T item)
            {
                for (int index = 0; index < _items.Count; index++)
                {
                    if (ReferenceEquals(_items[index], item))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }

        /// <inheritdoc />
        public bool Contains(object value) => IndexOf(value) >= 0;

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int offset = 0; offset < _items.Count; offset++)
            {
                array.SetValue(_items[offset], index + offset);
            }
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator() => _items.GetEnumerator();

        /// <inheritdoc />
        public int Add(object value) => throw ReadOnly();

        /// <inheritdoc />
        public void Clear() => throw ReadOnly();

        /// <inheritdoc />
        public void Insert(int index, object value) => throw ReadOnly();

        /// <inheritdoc />
        public void Remove(object value) => throw ReadOnly();

        /// <inheritdoc />
        public void RemoveAt(int index) => throw ReadOnly();

        private static NotSupportedException ReadOnly() => new NotSupportedException("Core owns this list; the view only reads it.");
    }
}
