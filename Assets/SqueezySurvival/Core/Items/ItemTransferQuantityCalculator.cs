using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>How many things a move takes from a stack (Docs/Design/UI/DragAndDrop.md, Between panels).</summary>
    public static class ItemTransferQuantityCalculator
    {
        /// <summary>All of the stack, half of it rounded up (half of 7 is 4), or one.</summary>
        public static int CountToMove(int stackCount, ItemTransferQuantity quantity)
        {
            if (stackCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(stackCount), stackCount, "A stack holds at least one thing.");
            }

            switch (quantity)
            {
                case ItemTransferQuantity.All:
                    return stackCount;
                case ItemTransferQuantity.Half:
                    return (stackCount + 1) / 2;
                case ItemTransferQuantity.One:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Unknown quantity.");
            }
        }
    }
}
