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
    /// Shows a counted collection as a scrollable grid inside a fixed-size panel, laid out by Core's
    /// <see cref="GridPanelLayoutCalculator"/> from the panel's settings in the shared <see cref="PlayerOptions"/>
    /// (Docs/Design/UI/GridPanels.md). The panel never resizes; only the cells and the scroll position change.
    /// </summary>
    public sealed class GridPanelView : IDisposable
    {
        // One fixed thickness for the grid's scrollbar, so Core can reserve exactly its space.
        private const float ScrollbarThickness = 12f;

        private readonly GridPanel _panel;
        private readonly PlayerOptions _options;
        private readonly VisualElement _panelElement;
        private readonly ScrollView _grid;
        private readonly Label _emptyLabel;
        private readonly GridTooltipView _tooltip;
        private readonly List<GridCell> _cells = new List<GridCell>();
        private bool _needsScrolling;

        /// <summary>Binds the grid in <paramref name="panelElement"/> to a panel's settings.</summary>
        public GridPanelView(
            GridPanel panel,
            PlayerOptions options,
            VisualElement panelElement,
            ScrollView grid,
            Label emptyLabel,
            GridTooltipView tooltip)
        {
            _panel = panel;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _panelElement = panelElement ?? throw new ArgumentNullException(nameof(panelElement));
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _emptyLabel = emptyLabel ?? throw new ArgumentNullException(nameof(emptyLabel));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));

            _grid.verticalScroller.style.width = ScrollbarThickness;
            _grid.horizontalScroller.style.height = ScrollbarThickness;

            // The panel handles the wheel before the grid does (trickle-down reaches ancestors first), so the
            // wheel follows the panel's scroll direction and inversion instead of the scroll view's default.
            _panelElement.RegisterCallback<WheelEvent>(OnWheel, TrickleDown.TrickleDown);

            // The layout is calculated from the grid area's own size, which never changes with its contents.
            _grid.RegisterCallback<GeometryChangedEvent>(_ => Relayout());
            _options.GridPanelLayoutChanged += OnGridPanelLayoutChanged;
            ApplyScrollDirection();
        }

        /// <summary>Replaces the grid's cells with one cell per entry, in order.</summary>
        public void ShowEntries<TDefinition>(IReadOnlyList<CountedEntry<TDefinition>> entries)
            where TDefinition : class, ICountableThingDefinition
        {
            _tooltip.Hide();
            _grid.Clear();
            _cells.Clear();
            foreach (CountedEntry<TDefinition> entry in entries)
            {
                var cell = new GridCell(
                    entry.Definition.Id,
                    CountedThingTextFormatter.FormatCountBadge(entry.Count),
                    ThingTooltipContentFactory.Create(entry),
                    _tooltip);
                _cells.Add(cell);
                _grid.Add(cell.Element);
            }

            _emptyLabel.style.display = entries.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            _grid.style.display = entries.Count == 0 ? DisplayStyle.None : DisplayStyle.Flex;
            Relayout();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _options.GridPanelLayoutChanged -= OnGridPanelLayoutChanged;
        }

        private GridPanelLayoutOptions LayoutOptions => _options.GridPanelLayout(_panel);

        private void OnGridPanelLayoutChanged(GridPanel panel)
        {
            if (panel != _panel)
            {
                return;
            }

            ApplyScrollDirection();
            _grid.scrollOffset = Vector2.zero;
            Relayout();
        }

        private void ApplyScrollDirection()
        {
            _grid.mode = LayoutOptions.ScrollDirection == GridPanelScrollDirection.Vertical
                ? ScrollViewMode.Vertical
                : ScrollViewMode.Horizontal;
        }

        private void Relayout()
        {
            GridPanelLayoutOptions options = LayoutOptions;
            Rect area = _grid.layout;
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(options, area.width, area.height, _cells.Count, ScrollbarThickness);

            // The scrollbar along the scroll direction is always there, so cells never resize; it is enabled only
            // when there is something to scroll, and dimmed otherwise.
            _needsScrolling = layout.NeedsScrolling;
            bool vertical = options.ScrollDirection == GridPanelScrollDirection.Vertical;
            _grid.verticalScrollerVisibility = vertical ? ScrollerVisibility.AlwaysVisible : ScrollerVisibility.Hidden;
            _grid.horizontalScrollerVisibility = vertical ? ScrollerVisibility.Hidden : ScrollerVisibility.AlwaysVisible;
            _grid.verticalScroller.SetEnabled(vertical && _needsScrolling);
            _grid.horizontalScroller.SetEnabled(!vertical && _needsScrolling);
            if (!_needsScrolling)
            {
                _grid.scrollOffset = Vector2.zero;
            }

            _grid.contentContainer.style.width = layout.ContentWidth;
            _grid.contentContainer.style.height = layout.ContentHeight;
            for (int index = 0; index < _cells.Count; index++)
            {
                _cells[index].Place(layout.PositionOf(index), layout.CellSize);
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

            _tooltip.Hide();
            GridPanelLayoutOptions layoutOptions = LayoutOptions;
            float change = GridPanelScrollWheelMapper.ScrollOffsetChange(layoutOptions, wheel.delta.x, wheel.delta.y, _grid.mouseWheelScrollSize);
            Vector2 offset = _grid.scrollOffset;
            if (layoutOptions.ScrollDirection == GridPanelScrollDirection.Vertical)
            {
                offset.y = Mathf.Clamp(offset.y + change, 0f, _grid.verticalScroller.highValue);
            }
            else
            {
                offset.x = Mathf.Clamp(offset.x + change, 0f, _grid.horizontalScroller.highValue);
            }

            _grid.scrollOffset = offset;
        }

        /// <summary>One square of the grid: an icon, a count in the bottom-right corner, and a tooltip on hover.</summary>
        private sealed class GridCell
        {
            public GridCell(string definitionId, string countBadge, ThingTooltipContent tooltipContent, GridTooltipView tooltip)
            {
                Element = new VisualElement();
                Element.AddToClassList("grid-cell");
                Element.style.position = Position.Absolute;

                var icon = new PlaceholderIconElement(definitionId);
                icon.AddToClassList("grid-cell-icon");
                Element.Add(icon);

                if (countBadge.Length > 0)
                {
                    var count = new Label(countBadge) { pickingMode = PickingMode.Ignore };
                    count.AddToClassList("grid-cell-count");
                    Element.Add(count);
                }

                Element.RegisterCallback<PointerEnterEvent>(_ => tooltip.Show(tooltipContent, Element.worldBound));
                Element.RegisterCallback<PointerLeaveEvent>(_ => tooltip.Hide());
            }

            public VisualElement Element { get; }

            public void Place(GridCellPosition position, float size)
            {
                Element.style.left = position.Left;
                Element.style.top = position.Top;
                Element.style.width = size;
                Element.style.height = size;
            }
        }
    }
}
