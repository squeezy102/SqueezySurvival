using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// What a move between panels would do, worked out before anything changes so it can be shown first
    /// (Docs/Design/UI/DragAndDrop.md, Between panels). Immutable.
    /// </summary>
    public sealed class ItemMovePreview
    {
        /// <summary>Creates a preview.</summary>
        /// <param name="definition">The kind being moved.</param>
        /// <param name="count">How many would move.</param>
        /// <param name="from">The panel they leave.</param>
        /// <param name="to">The panel they go to.</param>
        /// <param name="destinationSlot">The slot they would land in, or -1 when there is no room.</param>
        /// <param name="alreadyThereCount">How many of the kind are already in the destination; 0 for a new stack.</param>
        /// <param name="seconds">How long the move takes.</param>
        public ItemMovePreview(ItemDefinition definition, int count, ItemPanel from, ItemPanel to, int destinationSlot, int alreadyThereCount, int seconds)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "A move takes at least one thing.");
            }

            if (from == to)
            {
                throw new ArgumentException("A move goes between two different panels.", nameof(to));
            }

            Count = count;
            From = from;
            To = to;
            DestinationSlot = destinationSlot;
            AlreadyThereCount = alreadyThereCount;
            Seconds = seconds;
        }

        /// <summary>The kind being moved.</summary>
        public ItemDefinition Definition { get; }

        /// <summary>How many would move.</summary>
        public int Count { get; }

        /// <summary>The panel they leave.</summary>
        public ItemPanel From { get; }

        /// <summary>The panel they go to.</summary>
        public ItemPanel To { get; }

        /// <summary>The slot they would land in, or -1 when there is no room.</summary>
        public int DestinationSlot { get; }

        /// <summary>How many of the kind are already in the destination; 0 when they would start a new stack.</summary>
        public int AlreadyThereCount { get; }

        /// <summary>How long the move takes, in seconds.</summary>
        public int Seconds { get; }

        /// <summary>Whether the destination has room.</summary>
        public bool CanMove => DestinationSlot >= 0;

        /// <summary>Whether the things would join a stack already in the destination.</summary>
        public bool JoinsStack => AlreadyThereCount > 0;
    }
}
