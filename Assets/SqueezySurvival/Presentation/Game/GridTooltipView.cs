using System;
using SqueezySurvival.Core.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The tooltip shown while the pointer is over a grid cell (Docs/Design/UI/InventoryGrids.md, Tooltips). UI Toolkit's
    /// built-in tooltip property only works in the Unity Editor, so the game draws its own.
    /// </summary>
    public sealed class GridTooltipView
    {
        private const float DistanceFromCell = 8f;

        private readonly VisualElement _tooltip;
        private readonly Label _title;
        private readonly Label _subtitle;
        private readonly Label _description;
        private readonly VisualElement _details;
        private Rect _anchorCellWorldBound;
        private IVisualElementScheduledItem _scheduledHide;

        /// <summary>Binds the tooltip element. It starts hidden.</summary>
        public GridTooltipView(VisualElement tooltip)
        {
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _title = tooltip.Q<Label>("grid-tooltip-title");
            _subtitle = tooltip.Q<Label>("grid-tooltip-subtitle");
            _description = tooltip.Q<Label>("grid-tooltip-description");
            _details = tooltip.Q<VisualElement>("grid-tooltip-details");
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
            _title.text = content.Title;
            _subtitle.text = content.Subtitle;
            _subtitle.style.display = content.Subtitle.Length > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            _description.text = content.Description;
            _details.Clear();
            foreach (string line in content.DetailLines)
            {
                var detail = new Label(line);
                detail.AddToClassList("grid-tooltip-detail");
                _details.Add(detail);
            }

            _anchorCellWorldBound = cellWorldBound;
            _tooltip.style.display = DisplayStyle.Flex;
            PlaceBesideAnchor();
        }

        /// <summary>Whether a brief message is up; hovering does not replace it until it goes.</summary>
        public bool IsShowingBriefMessage => _scheduledHide != null;

        /// <summary>Shows the content next to a cell, then hides it after the given time unless replaced first.</summary>
        public void ShowBriefly(ThingTooltipContent content, Rect cellWorldBound, long milliseconds)
        {
            Show(content, cellWorldBound);
            _scheduledHide = _tooltip.schedule.Execute(Hide).StartingIn(milliseconds);
        }

        /// <summary>Hides the tooltip.</summary>
        public void Hide()
        {
            CancelScheduledHide();
            _tooltip.style.display = DisplayStyle.None;
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
