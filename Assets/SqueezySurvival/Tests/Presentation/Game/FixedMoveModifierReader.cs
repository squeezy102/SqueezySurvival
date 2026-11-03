using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Game;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>A modifier reader that reports one fixed move role, as if that modifier were held, or none.</summary>
    public sealed class FixedMoveModifierReader : IMoveModifierReader
    {
        private readonly ItemTransferQuantity? _heldQuantity;

        /// <summary>Reports <paramref name="heldQuantity"/> as held, or no modifier when null.</summary>
        public FixedMoveModifierReader(ItemTransferQuantity? heldQuantity)
        {
            _heldQuantity = heldQuantity;
        }

        /// <inheritdoc />
        public ItemTransferQuantity? HeldQuantity() => _heldQuantity;
    }
}
