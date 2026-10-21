using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Controls
{
    /// <summary>
    /// Which modifier key does each move role: Move all, Move half and Move one. Each key does exactly one role, so the
    /// scheme stays consistent when the player remaps it (Docs/Design/UI/Controls.md, Accessibility). Immutable.
    /// </summary>
    public sealed class MoveModifierBindings
    {
        /// <summary>The defaults: Shift moves all, Ctrl moves half, Alt moves one (resolves OQ-085).</summary>
        public static readonly MoveModifierBindings Default =
            new MoveModifierBindings(MoveModifierKey.Shift, MoveModifierKey.Control, MoveModifierKey.Alt);

        /// <summary>Binds one key to each role. The three keys must differ.</summary>
        public MoveModifierBindings(MoveModifierKey moveAllKey, MoveModifierKey moveHalfKey, MoveModifierKey moveOneKey)
        {
            if (moveAllKey == moveHalfKey || moveAllKey == moveOneKey || moveHalfKey == moveOneKey)
            {
                throw new ArgumentException("Each modifier key does exactly one role.");
            }

            MoveAllKey = moveAllKey;
            MoveHalfKey = moveHalfKey;
            MoveOneKey = moveOneKey;
        }

        /// <summary>The key that moves the whole stack.</summary>
        public MoveModifierKey MoveAllKey { get; }

        /// <summary>The key that moves half the stack.</summary>
        public MoveModifierKey MoveHalfKey { get; }

        /// <summary>The key that moves one.</summary>
        public MoveModifierKey MoveOneKey { get; }

        /// <summary>
        /// The role of the modifiers being held, or null when none is. When several are held, the smallest move wins,
        /// so an accidental extra key never moves more than meant.
        /// </summary>
        public ItemTransferQuantity? QuantityForHeld(bool shiftHeld, bool controlHeld, bool altHeld)
        {
            bool IsHeld(MoveModifierKey key)
            {
                switch (key)
                {
                    case MoveModifierKey.Shift:
                        return shiftHeld;
                    case MoveModifierKey.Control:
                        return controlHeld;
                    case MoveModifierKey.Alt:
                        return altHeld;
                    default:
                        return false;
                }
            }

            if (IsHeld(MoveOneKey))
            {
                return ItemTransferQuantity.One;
            }

            if (IsHeld(MoveHalfKey))
            {
                return ItemTransferQuantity.Half;
            }

            if (IsHeld(MoveAllKey))
            {
                return ItemTransferQuantity.All;
            }

            return null;
        }
    }
}
