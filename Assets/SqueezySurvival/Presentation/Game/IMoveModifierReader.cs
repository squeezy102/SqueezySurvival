using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Reports which move modifier the player is holding as a move role (Docs/Design/UI/Controls.md). An interface so
    /// tests can hold a modifier without a keyboard.
    /// </summary>
    public interface IMoveModifierReader
    {
        /// <summary>The role of the modifier held right now, or null when none is held.</summary>
        ItemTransferQuantity? HeldQuantity();
    }
}
