using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The tooltip shown while the pointer is over a grid cell (Docs/Design/UI/InventoryGrids.md, Tooltips). UI Toolkit's
    /// built-in tooltip property only works in the Unity Editor, so the game draws its own. Its elements are made once and
    /// updated in place; a detail line is added only when a tooltip has more lines than any before it
    /// (Docs/Engineering/Architecture.md, Element lifetime).
    /// </summary>
    public sealed class GridTooltipView
    {
        private const float DistanceFromCell = 8f;

        private readonly VisualElement _tooltip;
        private readonly PixelArtTextureCache _textures;
        private readonly VisualElement _iconHolder;
        private readonly PixelIconElement _icon;
        private readonly List<Label> _detailLines = new List<Label>();
        private readonly Label _title;
        private readonly Label _subtitle;
        private readonly Label _description;
        private readonly VisualElement _details;
        private Rect _anchorCellWorldBound;
        private IVisualElementScheduledItem _scheduledHide;
        private bool _pinned;

        /// <summary>Binds the tooltip element. It starts hidden.</summary>
        public GridTooltipView(VisualElement tooltip, PixelArtTextureCache textures)
        {
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
            _iconHolder = tooltip.Q<VisualElement>("grid-tooltip-icon");
            _title = tooltip.Q<Label>("grid-tooltip-title");
            _subtitle = tooltip.Q<Label>("grid-tooltip-subtitle");
            _description = tooltip.Q<Label>("grid-tooltip-description");
            _details = tooltip.Q<VisualElement>("grid-tooltip-details");
            _icon = new PixelIconElement(_textures.EmptyIcon, fitsParent: false);
            _icon.style.position = Position.Relative;
            _iconHolder.Add(_icon);
            _tooltip.pickingMode = PickingMode.Ignore;
            _tooltip.style.position = Position.Absolute;

            // The tooltip's size is known only after layout, so it is placed once its geometry is computed.
            _tooltip.RegisterCallback<GeometryChangedEvent>(_ => PlaceBesideAnchor());
            Hide();
        }

        /// <summary>Shows the content next to a cell, given in world (panel) coordinates.</summary>
        public void Show(ThingTooltipContent content, Rect cellWorldBound)
        {
            CancelScheduledHide();
            _pinned = false;
            ShowIcon(content.IconId, cellWorldBound.height);
            _title.text = content.Title;
            _subtitle.text = content.Subtitle;
            _subtitle.style.display = content.Subtitle.Length > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            _description.text = content.Description;
            ShowDetailLines(content.DetailLines);

            _anchorCellWorldBound = cellWorldBound;
            _tooltip.style.display = DisplayStyle.Flex;
            PlaceBesideAnchor();
        }

        /// <summary>Whether a brief message is up; hovering does not replace it until it goes.</summary>
        public bool IsShowingBriefMessage => _scheduledHide != null;

        /// <summary>Whether the tooltip belongs to a selected thing; hovering does not replace it until it is hidden.</summary>
        public bool IsPinned => _pinned;

        /// <summary>Shows the content for a selected thing and keeps it until hidden (Docs/Design/UI/InventoryGrids.md, Selecting).</summary>
        public void ShowPinned(ThingTooltipContent content, Rect cellWorldBound)
        {
            Show(content, cellWorldBound);
            _pinned = true;
        }

        /// <summary>Shows the content next to a cell, then hides it after the given time unless replaced first.</summary>
        public void ShowBriefly(ThingTooltipContent content, Rect cellWorldBound, long milliseconds)
        {
            Show(content, cellWorldBound);
            _scheduledHide = _tooltip.schedule.Execute(Hide).StartingIn(milliseconds);
        }

        /// <summary>Hides the tooltip.</summary>
        public void Hide()
        {
            _pinned = false;
            CancelScheduledHide();
            _tooltip.style.display = DisplayStyle.None;
        }

        // The icon is one whole-pixel step larger than in its slot, so its art pixels stay square (#55).
        private void ShowIcon(string iconId, float cellSize)
        {
            if (iconId.Length == 0)
            {
                _iconHolder.style.display = DisplayStyle.None;
                return;
            }

            float size = PixelIconElement.WholePixelSize(cellSize) + Core.PixelArt.PlaceholderIconCatalog.IconSize;
            _icon.SetIcon(_textures.Icon(iconId));
            _icon.style.width = size;
            _icon.style.height = size;
            _iconHolder.style.display = DisplayStyle.Flex;
        }

        private void ShowDetailLines(IReadOnlyList<string> lines)
        {
            while (_detailLines.Count < lines.Count)
            {
                var detail = new Label();
                detail.AddToClassList("grid-tooltip-detail");
                _detailLines.Add(detail);
                _details.Add(detail);
            }

            for (int index = 0; index < _detailLines.Count; index++)
            {
                bool used = index < lines.Count;
                _detailLines[index].text = used ? lines[index] : string.Empty;
                _detailLines[index].style.display = used ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void CancelScheduledHide()
        {
            _scheduledHide?.Pause();
            _scheduledHide = null;
        }

        // Right of the cell when it fits, otherwise left of it; always kept inside the screen.
        private void PlaceBesideAnchor()
        {
            VisualElement container = _tooltip.parent;
            if (container == null || _tooltip.style.display == DisplayStyle.None)
            {
                return;
            }

            Rect cell = container.WorldToLocal(_anchorCellWorldBound);
            float width = _tooltip.resolvedStyle.width;
            float height = _tooltip.resolvedStyle.height;
            Rect bounds = container.contentRect;
            if (float.IsNaN(width) || float.IsNaN(height))
            {
                return;
            }

            float left = cell.xMax + DistanceFromCell;
            if (left + width > bounds.xMax)
            {
                left = cell.xMin - DistanceFromCell - width;
            }

            float top = Mathf.Clamp(cell.yMin, bounds.yMin, Mathf.Max(bounds.yMin, bounds.yMax - height));
            _tooltip.style.left = Mathf.Max(bounds.xMin, left);
            _tooltip.style.top = top;
        }
    }
}
