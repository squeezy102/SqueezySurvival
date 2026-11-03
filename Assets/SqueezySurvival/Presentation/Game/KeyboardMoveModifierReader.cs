using System;
using SqueezySurvival.Core.Controls;
using SqueezySurvival.Core.Items;
using UnityEngine.InputSystem;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Reads which move modifier is held on the keyboard and turns it into a move role through the player's bindings
    /// (Docs/Design/UI/Controls.md). Which key does which role is decided in Core.
    /// </summary>
    public sealed class KeyboardMoveModifierReader : IMoveModifierReader
    {
        private readonly MoveModifierBindings _bindings;

        /// <summary>Reads modifiers through <paramref name="bindings"/>.</summary>
        public KeyboardMoveModifierReader(MoveModifierBindings bindings)
        {
            _bindings = bindings ?? throw new ArgumentNullException(nameof(bindings));
        }

        /// <inheritdoc />
        public ItemTransferQuantity? HeldQuantity()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return null;
            }

            return _bindings.QuantityForHeld(keyboard.shiftKey.isPressed, keyboard.ctrlKey.isPressed, keyboard.altKey.isPressed);
        }
    }
}
