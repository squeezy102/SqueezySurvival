using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Times how long a dragged thing has been held over one thing, another or its own stack, so that a quick drop swaps and a held one
    /// attempts a combine (Docs/Design/UI/DragAndDrop.md, Moving, swapping and combining). The hold starts when the
    /// pointer enters an occupied slot and resets when it leaves that slot.
    /// </summary>
    public sealed class CombineHoldTimer
    {
        /// <summary>How long to hold over another thing before a drop attempts a combine, in seconds.</summary>
        public const double CombineHoldSeconds = 1.0;

        private int _heldSlot = -1;
        private double _heldSinceSeconds;

        /// <summary>
        /// Reports what the pointer is over at <paramref name="nowSeconds"/> and returns whether a drop there would
        /// attempt a combine.
        /// </summary>
        /// <param name="occupiedSlotOrNone">The occupied slot under the pointer, its own included, or -1 when there is none.</param>
        /// <param name="nowSeconds">The current time from any clock that only moves forward, in seconds.</param>
        public bool Update(int occupiedSlotOrNone, double nowSeconds)
        {
            if (occupiedSlotOrNone < 0)
            {
                Reset();
                return false;
            }

            if (occupiedSlotOrNone != _heldSlot)
            {
                _heldSlot = occupiedSlotOrNone;
                _heldSinceSeconds = nowSeconds;
            }

            return nowSeconds - _heldSinceSeconds >= CombineHoldSeconds;
        }

        /// <summary>Forgets any hold, as when a drag ends.</summary>
        public void Reset()
        {
            _heldSlot = -1;
            _heldSinceSeconds = 0;
        }
    }
}
