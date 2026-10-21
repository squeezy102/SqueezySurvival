using System;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The cell for one slot of an inventory grid. Created once with its grid and kept for the grid's life: changes to
    /// the slot update it in place, so hover, selection and drags survive changes elsewhere, and no elements are made
    /// or discarded as things move (#114; Unity Manual, best practices for managing elements).
    /// </summary>
    public sealed class InventoryGridCell
    {
        private readonly PixelArtTextureCache _textures;
        private readonly PixelIconElement _icon;
        private readonly Label _count;

        /// <summary>Creates the cell for <paramref name="slot"/>, empty and hidden.</summary>
        public InventoryGridCell(int slot, PixelArtTextureCache textures)
        {
            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
            Slot = slot;
            Element = new VisualElement();
            Element.AddToClassList("grid-cell");
            Element.style.position = Position.Absolute;
            Element.style.display = DisplayStyle.None;

            _icon = new PixelIconElement(textures.EmptyIcon);
            Element.Add(_icon);
            _count = new Label { pickingMode = PickingMode.Ignore };
            _count.AddToClassList("grid-cell-count");
            _count.style.display = DisplayStyle.None;
            Element.Add(_count);
        }

        /// <summary>The slot this cell shows.</summary>
        public int Slot { get; }

        /// <summary>The cell's element, placed in the grid area.</summary>
        public VisualElement Element { get; }

        /// <summary>The id of the thing shown, or null when the slot is empty.</summary>
        public string DefinitionId { get; private set; }

        /// <summary>The count badge shown; empty for one thing or none.</summary>
        public string CountBadge { get; private set; } = string.Empty;

        /// <summary>Whether the slot holds something.</summary>
        public bool IsOccupied => DefinitionId != null;

        /// <summary>
        /// Shows <paramref name="definitionId"/> with <paramref name="countBadge"/>, or nothing for a null id. Returns
        /// whether anything changed.
        /// </summary>
        public bool Show(string definitionId, string countBadge)
        {
            countBadge = definitionId == null ? string.Empty : countBadge ?? string.Empty;
            if (definitionId == DefinitionId && countBadge == CountBadge)
            {
                return false;
            }

            if (definitionId != DefinitionId && definitionId != null)
            {
                _icon.SetIcon(_textures.Icon(definitionId));
            }

            DefinitionId = definitionId;
            CountBadge = countBadge;
            _count.text = countBadge;
            _count.style.display = countBadge.Length > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            Element.style.display = definitionId == null ? DisplayStyle.None : DisplayStyle.Flex;
            return true;
        }
    }
}
