using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Turns a move between panels into the drop preview the drag shows (Docs/Design/UI/DragAndDrop.md, Between panels).
    /// </summary>
    public static class ItemMoveDropOutcomeFactory
    {
        /// <summary>
        /// A transfer outlined at the slot the things would land in, or, when there is no room, a refusal shown against
        /// <paramref name="hoveredSlot"/> (-1 when the pointer is between slots).
        /// </summary>
        public static GridDropOutcome Create(ItemMovePreview preview, int hoveredSlot)
        {
            if (preview == null)
            {
                throw new ArgumentNullException(nameof(preview));
            }

            string title = ItemMoveTextFormatter.FormatTitle(preview);
            string description = ItemMoveTextFormatter.FormatDescription(preview);
            return preview.CanMove
                ? new GridDropOutcome(GridDropOutcomeKind.Transfer, preview.DestinationSlot, preview.DestinationSlot, description, title)
                : new GridDropOutcome(GridDropOutcomeKind.NoRoom, -1, hoveredSlot, description, title);
        }
    }
}
