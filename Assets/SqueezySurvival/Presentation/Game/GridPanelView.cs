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
        private readonly GridPanel _panel;
        private readonly PlayerOptions _options;
        private readonly VisualElement _panelElement;
        private readonly ScrollView _grid;
        private readonly Label _emptyLabel;
        private readonly List<GridCell> _cells = new List<GridCell>();

        /// <summary>Binds the grid in <paramref name="panelElement"/> to a panel's settings.</summary>
        public GridPanelView(GridPanel panel, PlayerOptions options, VisualElement panelElement, ScrollView grid, Label emptyLabel)
        {
            _panel = panel;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _panelElement = panelElement ?? throw new ArgumentNullException(nameof(panelElement));
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _emptyLabel = emptyLabel ?? throw new ArgumentNullException(nameof(emptyLabel));

            // The panel handles the wheel before the grid does (trickle-down reaches ancestors first), so the
            // wheel follows the panel's scroll direction and inversion instead of the scroll view's default.
            _panelElement.RegisterCallback<WheelEvent>(OnWheel, TrickleDown.TrickleDown);
            _grid.contentViewport.RegisterCallback<GeometryChangedEvent>(_ => Relayout());
            _options.GridPanelLayoutChanged += OnGridPanelLayoutChanged;
            ApplyScrollDirection();
        }

        /// <summary>Replaces the grid's cells with one cell per entry, in order.</summary>
        public void ShowEntries<TDefinition>(IReadOnlyList<CountedEntry<TDefinition>> entries)
            where TDefinition : class, ICountableThingDefinition
        {
            _grid.Clear();
            _cells.Clear();
            foreach (CountedEntry<TDefinition> entry in entries)
            {
                var cell = new GridCell(entry.Definition.Id, CountedThingTextFormatter.FormatCountBadge(entry.Count));
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

        // Only the scroll direction's scrollbar is shown, and always, so the viewport's size never changes as
        // cells are added; that keeps the cell size steady.
        private void ApplyScrollDirection()
        {
            bool vertical = LayoutOptions.ScrollDirection == GridPanelScrollDirection.Vertical;
            _grid.mode = vertical ? ScrollViewMode.Vertical : ScrollViewMode.Horizontal;
            _grid.verticalScrollerVisibility = vertical ? ScrollerVisibility.AlwaysVisible : ScrollerVisibility.Hidden;
            _grid.horizontalScrollerVisibility = vertical ? ScrollerVisibility.Hidden : ScrollerVisibility.AlwaysVisible;
        }

        private void Relayout()
        {
            Rect viewport = _grid.contentViewport.layout;
            GridPanelLayout layout = GridPanelLayoutCalculator.Calculate(LayoutOptions, viewport.width, viewport.height, _cells.Count);

            _grid.contentContainer.style.width = layout.ContentWidth;
            _grid.contentContainer.style.height = layout.ContentHeight;
            for (int index = 0; index < _cells.Count; index++)
            {
                _cells[index].Place(layout.PositionOf(index), layout.CellSize);
            }
        }

        private void OnWheel(WheelEvent wheel)
        {
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
            wheel.StopPropagation();
        }

        /// <summary>One square of the grid: an icon, and a count in the bottom-right corner.</summary>
        private sealed class GridCell
        {
            public GridCell(string definitionId, string countBadge)
            {
                Element = new VisualElement();
                Element.AddToClassList("grid-cell");
                Element.style.position = Position.Absolute;

                var icon = new PlaceholderIconElement(definitionId);
                icon.AddToClassList("grid-cell-icon");
                Element.Add(icon);

                if (countBadge.Length > 0)
                {
                    var count = new Label(countBadge);
                    count.AddToClassList("grid-cell-count");
                    Element.Add(count);
                }
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
