using System;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Icons;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// A grid panel's Sort button and the arrow beside it (Docs/Design/UI/InventoryGrids.md, Sorting). Sort packs the
    /// grid in the panel's current order, by type until another is chosen; the arrow opens the other orders, and
    /// choosing one sorts at once and makes it the panel's order.
    /// </summary>
    public sealed class InventoryGridSortController
    {
        private readonly IInventoryGridContents _contents;
        private readonly Button _sortButton;
        private readonly Button _sortMenuButton;
        private ThingSortOrder _order = ThingSortOrder.ByType;

        /// <summary>Binds the two buttons to the grid's contents.</summary>
        public InventoryGridSortController(IInventoryGridContents contents, Button sortButton, Button sortMenuButton)
        {
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            _sortButton = sortButton ?? throw new ArgumentNullException(nameof(sortButton));
            _sortMenuButton = sortMenuButton ?? throw new ArgumentNullException(nameof(sortMenuButton));

            var arrow = new DropdownArrowIconElement();
            arrow.AddToClassList("sort-menu-arrow");
            _sortMenuButton.Add(arrow);

            _sortButton.clicked += () => _contents.Sort(_order);
            _sortMenuButton.clicked += OpenMenu;
            RefreshLabel();
        }

        private void OpenMenu()
        {
            // GenericDropdownMenu is UI Toolkit's menu for runtime and Editor UI
            // (https://docs.unity3d.com/ScriptReference/UIElements.GenericDropdownMenu.html).
            var menu = new GenericDropdownMenu();
            foreach (ThingSortOrder order in (ThingSortOrder[])Enum.GetValues(typeof(ThingSortOrder)))
            {
                ThingSortOrder chosen = order;
                menu.AddItem(ThingSortOrderTextFormatter.Format(order), order == _order, () => Choose(chosen));
            }

            menu.DropDown(_sortMenuButton.worldBound, _sortMenuButton, DropdownMenuSizeMode.Content);
        }

        private void Choose(ThingSortOrder order)
        {
            _order = order;
            RefreshLabel();
            _contents.Sort(order);
        }

        // The button names its order, so the player can see what one click will do.
        private void RefreshLabel()
        {
            _sortButton.text = $"Sort: {ThingSortOrderTextFormatter.Format(_order)}";
        }
    }
}
