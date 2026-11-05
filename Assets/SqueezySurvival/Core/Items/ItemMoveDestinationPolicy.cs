using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Decides where a modified click sends a stack (Docs/Design/UI/DragAndDrop.md, Destinations). It only names the
    /// panels the stack may go to, best first; whether one has room is for the mover to find out.
    /// </summary>
    public sealed class ItemMoveDestinationPolicy
    {
        /// <summary>
        /// The panels a stack clicked in <paramref name="from"/> may go to, best first. <paramref name="openContainer"/>
        /// is the open container's panel, or null when none is open.
        /// </summary>
        public IReadOnlyList<ItemPanel> DestinationsFor(ItemPanel from, ItemPanel? openContainer)
        {
            if (openContainer.HasValue && openContainer.Value.Kind != ItemPanelKind.Container)
            {
                throw new ArgumentException("The open container must be a container panel.", nameof(openContainer));
            }

            switch (from.Kind)
            {
                case ItemPanelKind.Container:
                    return new[] { ItemPanel.Inventory, ItemPanel.Ground };
                case ItemPanelKind.Ground:
                case ItemPanelKind.Inventory:
                    if (openContainer.HasValue)
                    {
                        return new[] { openContainer.Value };
                    }

                    return new[] { from.Kind == ItemPanelKind.Ground ? ItemPanel.Inventory : ItemPanel.Ground };
                default:
                    throw new ArgumentOutOfRangeException(nameof(from), from.Kind, "Unknown kind of panel.");
            }
        }
    }
}
