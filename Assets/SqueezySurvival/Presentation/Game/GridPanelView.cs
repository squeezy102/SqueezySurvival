using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Options;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows slotted Core contents as a scrollable grid inside a fixed-size panel, laid out by Core's
    /// <see cref="GridPanelLayoutCalculator"/> from the panel's settings in the shared <see cref="PlayerOptions"/>
    /// (Docs/Design/UI/GridPanels.md). Every slot is drawn, empty or not, so empty slots can be dropped on. The panel
    /// never resizes. Dragging cells is handled by a <see cref="GridCellDragController"/>.
    /// </summary>
    public sealed class GridPanelView : IDisposable
    {
        // One fixed thickness for the grid's scrollbar, so Core can reserve exactly its space.
        private const float ScrollbarThickness = 12f;

        private readonly GridPanel _panel;
        private readonly PlayerOptions _options;
        private readonly IGridPanelContents _contents;
        private readonly VisualElement _panelElement;
        private readonly Label _emptyLabel;
        private readonly GridTooltipView _tooltip;
        private readonly GridCellDragController _dragController;
        private readonly List<VisualElement> _slotBackgrounds = new List<VisualElement>();
        private readonly Dictionary<int, VisualElement> _itemCells = new Dictionary<int, VisualElement>();
        private bool _needsScrolling;
        private int _previewSourceSlot = -1;
        private int _previewTargetSlot = -1;

        /// <summary>Binds the grid in <paramref name="panelElement"/> to its contents and the panel's settings.</summary>
        /// <param name="panel">Which panel this is, for its settings.</param>
        /// <param name="options">The shared player options.</param>
        /// <param name="contents">What the grid shows and acts on.</param>
        /// <param name="panelElement">The whole panel, which handles the mouse wheel.</param>
        /// <param name="grid">The scroll view holding the cells.</param>
        /// <param name="emptyLabel">Shown instead of the grid while it is empty.</param>
        /// <param name="tooltip">The shared hover and drop-preview tooltip.</param>
        /// <param name="dragLayer">The screen-wide element a dragged icon is drawn on.</param>
        public GridPanelView(
            GridPanel panel,
            PlayerOptions options,
            IGridPanelContents contents,
            VisualElement panelElement,
            ScrollView grid,
            Label emptyLabel,
            GridTooltipView tooltip,
            VisualElement dragLayer)
        {
            _panel = panel;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            _panelElement = panelElement ?? throw new ArgumentNullException(nameof(panelElement));
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _emptyLabel = emptyLabel ?? throw new ArgumentNullException(nameof(emptyLabel));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _dragController = new GridCellDragController(this, _contents, _tooltip, dragLayer);

            Grid.verticalScroller.style.width = ScrollbarThickness;
            Grid.horizontalScroller.style.height = ScrollbarThickness;

            // The panel handles the wheel before the grid does (trickle-down reaches ancestors first), so the
            // wheel follows the panel's scroll direction and inversion instead of the scroll view's default.
            _panelElement.RegisterCallback<WheelEvent>(OnWheel, TrickleDown.TrickleDown);

            // The layout is calculated from the grid area's own size, which never changes with its contents.
            Grid.RegisterCallback<GeometryChangedEvent>(_ => Relayout());
            _options.GridPanelLayoutChanged += OnGridPanelLayoutChanged;
            _contents.Changed += Rebuild;
            ApplyScrollDirection();
            Rebuild();
        }

        /// <summary>The scroll view holding the cells.</summary>
        public ScrollView Grid { get; }

        /// <summary>The geometry the cells were last placed with.</summary>
        public GridPanelLayout CurrentLayout { get; private set; }

        /// <summary>How many slots are drawn, empty ones included.</summary>
        public int DisplayedSlotCount { get; private set; }

        /// <summary>Which slots lie beside which, for the current settings.</summary>
        public GridSlotNeighborhood Neighborhood => new GridSlotNeighborhood(LayoutOptions.ScrollDirection, LayoutOptions.FixedLineCount);

        /// <summary>Whether a cell from this grid is being dragged.</summary>
        public bool IsDragging => _dragController.IsDragging;

        /// <summary>Cancels a drag in progress; the dragged thing returns to its place.</summary>
        public void CancelDrag() => _dragController.Cancel();

        /// <summary>The item cell in a slot, or null when the slot is empty.</summary>
        public VisualElement ItemCellAt(int slot) => _itemCells.TryGetValue(slot, out VisualElement cell) ? cell : null;

        /// <summary>
        /// Shows the result of moving the thing in <paramref name="sourceSlot"/> to <paramref name="targetSlot"/>: it
        /// appears there, and anything already there appears in its place. No other cell moves.
        /// </summary>
        public void ShowMovePreview(int sourceSlot, int targetSlot)
        {
            _previewSourceSlot = sourceSlot;
            _previewTargetSlot = targetSlot;
            PlaceItemCells();
        }

        /// <summary>Puts every cell back in its real slot.</summary>
        public void ClearMovePreview()
        {
            _previewSourceSlot = -1;
            _previewTargetSlot = -1;
            PlaceItemCells();
        }

        /// <summary>A slot's rectangle in world (panel) coordinates.</summary>
        public Rect SlotWorldBound(int slot)
        {
            GridCellPosition position = CurrentLayout.PositionOf(slot);
            Vector2 topLeft = Grid.contentContainer.LocalToWorld(new Vector2(position.Left, position.Top));
            return new Rect(topLeft, new Vector2(CurrentLayout.CellSize, CurrentLayout.CellSize));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _options.GridPanelLayoutChanged -= OnGridPanelLayoutChanged;
            _contents.Changed -= Rebuild;
        }

        private GridPanelLayoutOptions LayoutOptions => _options.GridPanelLayout(_panel);

        private void Rebuild()
        {
            _dragController.Cancel();
            _tooltip.Hide();
            foreach (VisualElement cell in _itemCells.Values)
            {
                cell.RemoveFromHierarchy();
            }

            _itemCells.Clear();
            _previewSourceSlot = -1;
            _previewTargetSlot = -1;
            for (int slot = 0; slot < _contents.SlotCount; slot++)
            {
                if (_contents.IsOccupied(slot))
                {
                    VisualElement cell = CreateItemCell(slot);
                    _itemCells.Add(slot, cell);
                    Grid.Add(cell);
                }
            }

            bool empty = _itemCells.Count == 0;
            _emptyLabel.style.display = empty ? DisplayStyle.Flex : DisplayStyle.None;
            Grid.style.display = empty ? DisplayStyle.None : DisplayStyle.Flex;
            Relayout();
        }

        private VisualElement CreateItemCell(int slot)
        {
            var cell = new VisualElement();
            cell.AddToClassList("grid-cell");
            cell.style.position = Position.Absolute;

            var icon = new PlaceholderIconElement(_contents.DefinitionIdAt(slot));
            icon.AddToClassList("grid-cell-icon");
            cell.Add(icon);

            string countBadge = _contents.CountBadgeAt(slot);
            if (countBadge.Length > 0)
            {
                var count = new Label(countBadge) { pickingMode = PickingMode.Ignore };
                count.AddToClassList("grid-cell-count");
                cell.Add(count);
            }

            cell.RegisterCallback<PointerEnterEvent>(_ =>
            {
                if (!_dragController.IsDragging)
                {
                    _tooltip.Show(_contents.TooltipAt(slot), cell.worldBound);
                }
            });
            cell.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                if (!_dragController.IsDragging)
                {
                    _tooltip.Hide();
                }
            });
            _dragController.RegisterCell(cell, slot);
            return cell;
        }

        private void OnGridPanelLayoutChanged(GridPanel panel)
        {
            if (panel != _panel)
            {
                return;
            }

            ApplyScrollDirection();
            Grid.scrollOffset = Vector2.zero;
            Relayout();
        }

        private void ApplyScrollDirection()
        {
            Grid.mode = LayoutOptions.ScrollDirection == GridPanelScrollDirection.Vertical
                ? ScrollViewMode.Vertical
                : ScrollViewMode.Horizontal;
        }

        private void Relayout()
        {
            GridPanelLayoutOptions options = LayoutOptions;
            Rect area = Grid.layout;
            DisplayedSlotCount = GridPanelLayoutCalculator.DisplayedSlotCount(options, area.width, area.height, ScrollbarThickness, _contents.SlotCount);
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(options, area.width, area.height, DisplayedSlotCount, ScrollbarThickness);
            CurrentLayout = layout;

            // The scrollbar along the scroll direction is always there, so cells never resize; it is enabled only
            // when there is something to scroll, and dimmed otherwise.
            _needsScrolling = layout.NeedsScrolling;
            bool vertical = options.ScrollDirection == GridPanelScrollDirection.Vertical;
            Grid.verticalScrollerVisibility = vertical ? ScrollerVisibility.AlwaysVisible : ScrollerVisibility.Hidden;
            Grid.horizontalScrollerVisibility = vertical ? ScrollerVisibility.Hidden : ScrollerVisibility.AlwaysVisible;
            Grid.verticalScroller.SetEnabled(vertical && _needsScrolling);
            Grid.horizontalScroller.SetEnabled(!vertical && _needsScrolling);
            if (!_needsScrolling)
            {
                Grid.scrollOffset = Vector2.zero;
            }

            Grid.contentContainer.style.width = layout.ContentWidth;
            Grid.contentContainer.style.height = layout.ContentHeight;
            PlaceSlotBackgrounds();
            PlaceItemCells();
        }

        // Empty squares mark every slot, so gaps are visible and can be dropped on. They sit behind the items.
        private void PlaceSlotBackgrounds()
        {
            while (_slotBackgrounds.Count < DisplayedSlotCount)
            {
                var background = new VisualElement { pickingMode = PickingMode.Ignore };
                background.AddToClassList("grid-slot");
                background.style.position = Position.Absolute;
                _slotBackgrounds.Add(background);
                Grid.Insert(0, background);
            }

            for (int slot = 0; slot < _slotBackgrounds.Count; slot++)
            {
                VisualElement background = _slotBackgrounds[slot];
                bool shown = slot < DisplayedSlotCount;
                background.style.display = shown ? DisplayStyle.Flex : DisplayStyle.None;
                if (shown)
                {
                    Place(background, slot);
                }
            }
        }

        private void PlaceItemCells()
        {
            if (CurrentLayout == null)
            {
                return;
            }

            foreach (KeyValuePair<int, VisualElement> item in _itemCells)
            {
                Place(item.Value, ShownSlotOf(item.Key));
            }
        }

        // During a move preview the dragged thing shows in the target slot and whatever was there shows in its slot.
        private int ShownSlotOf(int slot)
        {
            if (_previewSourceSlot < 0)
            {
                return slot;
            }

            if (slot == _previewSourceSlot)
            {
                return _previewTargetSlot;
            }

            return slot == _previewTargetSlot ? _previewSourceSlot : slot;
        }

        private void Place(VisualElement element, int slot)
        {
            GridCellPosition position = CurrentLayout.PositionOf(slot);
            element.style.left = position.Left;
            element.style.top = position.Top;
            element.style.width = CurrentLayout.CellSize;
            element.style.height = CurrentLayout.CellSize;
        }

        private void OnWheel(WheelEvent wheel)
        {
            // Nothing to scroll: the wheel does nothing here.
            wheel.StopPropagation();
            if (!_needsScrolling)
            {
                return;
            }

            if (!_dragController.IsDragging)
            {
                _tooltip.Hide();
            }

            GridPanelLayoutOptions layoutOptions = LayoutOptions;
            float change = GridPanelScrollWheelMapper.ScrollOffsetChange(layoutOptions, wheel.delta.x, wheel.delta.y, Grid.mouseWheelScrollSize);
            Vector2 offset = Grid.scrollOffset;
            if (layoutOptions.ScrollDirection == GridPanelScrollDirection.Vertical)
            {
                offset.y = Mathf.Clamp(offset.y + change, 0f, Grid.verticalScroller.highValue);
            }
            else
            {
                offset.x = Mathf.Clamp(offset.x + change, 0f, Grid.horizontalScroller.highValue);
            }

            Grid.scrollOffset = offset;
        }
    }
}
