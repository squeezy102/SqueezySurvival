using System;
using System.Collections.Generic;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows slotted Core contents as a fixed grid, like Diablo's inventory, laid out by Core's
    /// <see cref="FixedGridLayoutCalculator"/> (Docs/Design/UI/InventoryGrids.md). Every slot is drawn, empty or not,
    /// so empty slots can be dropped on. The grid never scrolls or resizes, and cells move only when a drop happens.
    /// Dragging is handled by a <see cref="GridCellDragController"/>.
    /// </summary>
    public sealed class InventoryGridView : IDisposable
    {
        private readonly FixedGridShape _shape;
        private readonly FixedGridHorizontalAlignment _horizontalAlignment;
        private readonly IInventoryGridContents _contents;
        private readonly GridTooltipView _tooltip;
        private readonly GridCellDragController _dragController;
        private readonly List<VisualElement> _slotBackgrounds = new List<VisualElement>();
        private readonly Dictionary<int, VisualElement> _itemCells = new Dictionary<int, VisualElement>();
        private readonly VisualElement _dropTargetOutline;
        private int _dropTargetSlot = -1;
        private SlotChoice _slotChoice;

        /// <summary>Binds the grid area to its contents.</summary>
        /// <param name="shape">The grid's columns and rows.</param>
        /// <param name="horizontalAlignment">Where the grid sits across its area.</param>
        /// <param name="contents">What the grid shows and acts on.</param>
        /// <param name="gridArea">The element the cells are drawn in.</param>
        /// <param name="tooltip">The shared hover and drop-preview tooltip.</param>
        /// <param name="dragLayer">The screen-wide element a dragged icon is drawn on.</param>
        public InventoryGridView(
            FixedGridShape shape,
            FixedGridHorizontalAlignment horizontalAlignment,
            IInventoryGridContents contents,
            VisualElement gridArea,
            GridTooltipView tooltip,
            VisualElement dragLayer)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            _horizontalAlignment = horizontalAlignment;
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            GridArea = gridArea ?? throw new ArgumentNullException(nameof(gridArea));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            if (_contents.SlotCapacity != _shape.SlotCount)
            {
                throw new ArgumentException("The grid's shape must have exactly one cell per slot.", nameof(contents));
            }

            _dragController = new GridCellDragController(this, _contents, _tooltip, dragLayer);
            _dropTargetOutline = new VisualElement { pickingMode = PickingMode.Ignore };
            _dropTargetOutline.AddToClassList("grid-drop-target");
            _dropTargetOutline.style.position = Position.Absolute;
            CreateSlotBackgrounds();

            // The layout is calculated from the grid area's own size, which never changes with its contents.
            GridArea.RegisterCallback<GeometryChangedEvent>(_ => Relayout());
            _contents.Changed += Rebuild;
            Rebuild();
        }

        /// <summary>The element the cells are drawn in.</summary>
        public VisualElement GridArea { get; }

        /// <summary>The geometry the cells were last placed with.</summary>
        public FixedGridLayout CurrentLayout { get; private set; }

        /// <summary>Whether a cell from this grid is being dragged.</summary>
        public bool IsDragging => _dragController.IsDragging;

        /// <summary>Whether the player is being asked to choose a slot.</summary>
        public bool IsChoosingSlot => _slotChoice != null;

        /// <summary>Cancels a drag in progress; the dragged thing returns to its place.</summary>
        public void CancelDrag() => _dragController.Cancel();

        /// <summary>
        /// Asks the player to choose an occupied slot: hovering one shows <paramref name="preview"/>'s tooltip, and
        /// clicking it calls <paramref name="chosen"/>. Dragging is off until <see cref="EndSlotChoice"/>.
        /// </summary>
        public void BeginSlotChoice(Func<int, ThingTooltipContent> preview, Action<int> chosen)
        {
            _dragController.Cancel();
            _slotChoice = new SlotChoice(preview ?? throw new ArgumentNullException(nameof(preview)), chosen ?? throw new ArgumentNullException(nameof(chosen)));
            GridArea.AddToClassList("inventory-grid--choosing");
            _tooltip.Hide();
        }

        /// <summary>Ends a slot choice without choosing.</summary>
        public void EndSlotChoice()
        {
            _slotChoice = null;
            GridArea.RemoveFromClassList("inventory-grid--choosing");
            _tooltip.Hide();
        }

        /// <summary>Outlines the slot a drop would go to. No cell moves until the drop happens (Docs/Design/UI/DragAndDrop.md).</summary>
        public void ShowDropTarget(int slot)
        {
            _dropTargetSlot = slot;
            if (_dropTargetOutline.parent == null)
            {
                GridArea.Add(_dropTargetOutline);
            }

            _dropTargetOutline.BringToFront();
            Place(_dropTargetOutline, slot);
        }

        /// <summary>Removes the drop target outline.</summary>
        public void HideDropTarget()
        {
            _dropTargetSlot = -1;
            _dropTargetOutline.RemoveFromHierarchy();
        }

        /// <summary>A slot's rectangle in world (panel) coordinates.</summary>
        public Rect SlotWorldBound(int slot)
        {
            GridCellPosition position = CurrentLayout.PositionOf(slot);
            Vector2 topLeft = GridArea.LocalToWorld(new Vector2(position.Left, position.Top));
            return new Rect(topLeft, new Vector2(CurrentLayout.CellSize, CurrentLayout.CellSize));
        }

        /// <summary>The slot under a point in world (panel) coordinates, or -1; with where in the cell the point is.</summary>
        public int SlotAtWorldPosition(Vector2 worldPosition, out float xFraction, out float yFraction)
        {
            Vector2 local = GridArea.WorldToLocal(worldPosition);
            return CurrentLayout.SlotAt(local.x, local.y, out xFraction, out yFraction);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _contents.Changed -= Rebuild;
        }

        private void Rebuild()
        {
            _dragController.Cancel();
            _tooltip.Hide();
            foreach (VisualElement cell in _itemCells.Values)
            {
                cell.RemoveFromHierarchy();
            }

            _itemCells.Clear();
            HideDropTarget();
            for (int slot = 0; slot < _contents.SlotCapacity; slot++)
            {
                if (_contents.IsOccupied(slot))
                {
                    VisualElement cell = CreateItemCell(slot);
                    _itemCells.Add(slot, cell);
                    GridArea.Add(cell);
                }
            }

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
                if (_slotChoice != null)
                {
                    _tooltip.Show(_slotChoice.Preview(slot), cell.worldBound);
                }
                else if (!_dragController.IsDragging)
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

            // While choosing a slot, a click chooses; the drag controller ignores presses then.
            cell.RegisterCallback<ClickEvent>(_ => _slotChoice?.Chosen(slot));
            _dragController.RegisterCell(cell, slot, () => _slotChoice == null);
            return cell;
        }

        // Faint squares mark every slot, so gaps are visible and can be dropped on. They sit behind the items.
        private void CreateSlotBackgrounds()
        {
            for (int slot = 0; slot < _shape.SlotCount; slot++)
            {
                var background = new VisualElement { pickingMode = PickingMode.Ignore };
                background.AddToClassList("grid-slot");
                background.style.position = Position.Absolute;
                _slotBackgrounds.Add(background);
                GridArea.Add(background);
            }
        }

        private void Relayout()
        {
            Rect area = GridArea.contentRect;
            CurrentLayout = FixedGridLayoutCalculator.Calculate(_shape, area.width, area.height, _horizontalAlignment);
            for (int slot = 0; slot < _slotBackgrounds.Count; slot++)
            {
                Place(_slotBackgrounds[slot], slot);
            }

            foreach (KeyValuePair<int, VisualElement> item in _itemCells)
            {
                Place(item.Value, item.Key);
            }

            if (_dropTargetSlot >= 0)
            {
                Place(_dropTargetOutline, _dropTargetSlot);
            }
        }

        private void Place(VisualElement element, int slot)
        {
            if (CurrentLayout == null)
            {
                return;
            }

            GridCellPosition position = CurrentLayout.PositionOf(slot);
            element.style.left = position.Left;
            element.style.top = position.Top;
            element.style.width = CurrentLayout.CellSize;
            element.style.height = CurrentLayout.CellSize;
        }

        private sealed class SlotChoice
        {
            public SlotChoice(Func<int, ThingTooltipContent> preview, Action<int> chosen)
            {
                Preview = preview;
                Chosen = chosen;
            }

            public Func<int, ThingTooltipContent> Preview { get; }

            public Action<int> Chosen { get; }
        }
    }
}
