using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Options;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows Core contents as a scrollable grid inside a fixed-size panel, laid out by Core's
    /// <see cref="GridPanelLayoutCalculator"/> from the panel's settings in the shared <see cref="PlayerOptions"/>
    /// (Docs/Design/UI/GridPanels.md). The panel never resizes; only the cells and the scroll position change.
    /// Dragging cells is handled by a <see cref="GridCellDragController"/>.
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
        private readonly List<VisualElement> _cells = new List<VisualElement>();
        private bool _needsScrolling;
        private int _previewFromIndex = -1;
        private int _previewToIndex = -1;

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

        /// <summary>The panel's scroll direction.</summary>
        public GridPanelScrollDirection ScrollDirection => LayoutOptions.ScrollDirection;

        /// <summary>Whether a cell from this grid is being dragged.</summary>
        public bool IsDragging => _dragController.IsDragging;

        /// <summary>The element of the cell at <paramref name="index"/>.</summary>
        public VisualElement CellAt(int index) => _cells[index];

        /// <summary>The index of a cell element, or -1.</summary>
        public int IndexOfCell(VisualElement cell) => _cells.IndexOf(cell);

        /// <summary>Cancels a drag in progress; the dragged thing returns to its place.</summary>
        public void CancelDrag() => _dragController.Cancel();

        /// <summary>
        /// Shows the cells as they would be if the cell at <paramref name="fromIndex"/> moved to
        /// <paramref name="toIndex"/>: it takes that slot and the cells between slide over (Docs/Design/UI/DragAndDrop.md, Preview).
        /// </summary>
        public void ShowMovePreview(int fromIndex, int toIndex)
        {
            _previewFromIndex = fromIndex;
            _previewToIndex = toIndex;
            PlaceCells();
        }

        /// <summary>Puts every cell back in its real place.</summary>
        public void ClearMovePreview()
        {
            _previewFromIndex = -1;
            _previewToIndex = -1;
            PlaceCells();
        }

        /// <summary>The slot a cell is shown in: its own, or its previewed one during a drag.</summary>
        public int SlotOf(int index) =>
            _previewFromIndex < 0 ? index : OrderedListMove.SlotAfterMove(index, _previewFromIndex, _previewToIndex);

        /// <summary>The cell shown in a slot.</summary>
        public int IndexAtSlot(int slot) =>
            _previewFromIndex < 0 ? slot : OrderedListMove.IndexAtSlotAfterMove(slot, _previewFromIndex, _previewToIndex);

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
            _previewFromIndex = -1;
            _previewToIndex = -1;
            _tooltip.Hide();
            Grid.Clear();
            _cells.Clear();
            for (int index = 0; index < _contents.Count; index++)
            {
                VisualElement cell = CreateCell(index);
                _cells.Add(cell);
                Grid.Add(cell);
            }

            _emptyLabel.style.display = _contents.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            Grid.style.display = _contents.Count == 0 ? DisplayStyle.None : DisplayStyle.Flex;
            Relayout();
        }

        private VisualElement CreateCell(int index)
        {
            var cell = new VisualElement();
            cell.AddToClassList("grid-cell");
            cell.style.position = Position.Absolute;

            var icon = new PlaceholderIconElement(_contents.DefinitionIdAt(index));
            icon.AddToClassList("grid-cell-icon");
            cell.Add(icon);

            string countBadge = _contents.CountBadgeAt(index);
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
                    _tooltip.Show(_contents.TooltipAt(index), cell.worldBound);
                }
            });
            cell.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                if (!_dragController.IsDragging)
                {
                    _tooltip.Hide();
                }
            });
            _dragController.RegisterCell(cell, index);
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
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(options, area.width, area.height, _cells.Count, ScrollbarThickness);
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
            PlaceCells();
        }

        private void PlaceCells()
        {
            if (CurrentLayout == null)
            {
                return;
            }

            for (int index = 0; index < _cells.Count; index++)
            {
                GridCellPosition position = CurrentLayout.PositionOf(SlotOf(index));
                VisualElement cell = _cells[index];
                cell.style.left = position.Left;
                cell.style.top = position.Top;
                cell.style.width = CurrentLayout.CellSize;
                cell.style.height = CurrentLayout.CellSize;
            }
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
