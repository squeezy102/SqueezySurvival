using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
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
        private readonly PixelArtInterfaceSkin _skin;
        private readonly GridCellDragController _dragController;
        private readonly List<VisualElement> _slotBackgrounds = new List<VisualElement>();
        private readonly Dictionary<int, VisualElement> _slotGhosts = new Dictionary<int, VisualElement>();
        private readonly Func<int, Texture2D> _emptySlotGhost;
        private readonly List<InventoryGridCell> _cells = new List<InventoryGridCell>();
        private int _tooltipSlot = -1;
        private readonly VisualElement _dropTargetOutline;
        private int _dropTargetSlot = -1;
        private SlotChoice _slotChoice;
        private int _selectedSlot = -1;
        private ItemPanelMoveCoordinator _itemMoves;
        private ItemPanel _panel;

        /// <summary>Binds the grid area to its contents.</summary>
        /// <param name="shape">The grid's columns and rows.</param>
        /// <param name="horizontalAlignment">Where the grid sits across its area.</param>
        /// <param name="contents">What the grid shows and acts on.</param>
        /// <param name="gridArea">The element the cells are drawn in.</param>
        /// <param name="tooltip">The shared hover and drop-preview tooltip.</param>
        /// <param name="dragLayer">The screen-wide element a dragged icon is drawn on.</param>
        /// <param name="skin">Draws the slots and icons.</param>
        /// <param name="emptySlotGhost">A faint picture for each empty slot, such as a hand, or null for none.</param>
        public InventoryGridView(
            FixedGridShape shape,
            FixedGridHorizontalAlignment horizontalAlignment,
            IInventoryGridContents contents,
            VisualElement gridArea,
            GridTooltipView tooltip,
            VisualElement dragLayer,
            PixelArtInterfaceSkin skin,
            Func<int, Texture2D> emptySlotGhost = null)
        {
            _shape = shape ?? throw new ArgumentNullException(nameof(shape));
            _horizontalAlignment = horizontalAlignment;
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            GridArea = gridArea ?? throw new ArgumentNullException(nameof(gridArea));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _skin = skin ?? throw new ArgumentNullException(nameof(skin));
            _emptySlotGhost = emptySlotGhost;
            if (_contents.SlotCapacity != _shape.SlotCount)
            {
                throw new ArgumentException("The grid's shape must have exactly one cell per slot.", nameof(contents));
            }

            _dragController = new GridCellDragController(this, _contents, _tooltip, dragLayer, _skin.Textures);
            _dropTargetOutline = new VisualElement { pickingMode = PickingMode.Ignore };
            _dropTargetOutline.AddToClassList("grid-drop-target");
            _dropTargetOutline.style.position = Position.Absolute;
            CreateSlotBackgrounds();

            // The layout is calculated from the grid area's own size, which never changes with its contents.
            GridArea.RegisterCallback<GeometryChangedEvent>(_ => Relayout());
            // Any press outside the selected cell ends the selection, wherever on the screen it lands. Listening as
            // the event trickles down sees it before the element pressed handles it.
            dragLayer.RegisterCallback<PointerDownEvent>(OnAnyPointerDown, TrickleDown.TrickleDown);
            CreateCells();
            _contents.Changed += Refresh;
            Refresh();
        }

        /// <summary>The element the cells are drawn in.</summary>
        public VisualElement GridArea { get; }

        /// <summary>The geometry the cells were last placed with.</summary>
        public FixedGridLayout CurrentLayout { get; private set; }

        /// <summary>Whether a cell from this grid is being dragged.</summary>
        public bool IsDragging => _dragController.IsDragging;

        /// <summary>Whether the player is being asked to choose a slot.</summary>
        public bool IsChoosingSlot => _slotChoice != null;

        /// <summary>Whether a thing in this grid is selected.</summary>
        public bool HasSelection => _selectedSlot >= 0;

        /// <summary>Ends the selection, if any, and closes its tooltip (Docs/Design/UI/InventoryGrids.md, Selecting).</summary>
        public void ClearSelection()
        {
            if (_selectedSlot < 0)
            {
                return;
            }

            _cells[_selectedSlot].Element.RemoveFromClassList("grid-cell--selected");

            _selectedSlot = -1;
            if (_tooltip.IsPinned)
            {
                _tooltip.Hide();
            }
        }

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

        /// <summary>
        /// Lets this grid's things be dragged or modifier-clicked to the other panel (Docs/Design/UI/DragAndDrop.md,
        /// Between panels). Called once, by <see cref="ItemPanelMoveCoordinator.Register"/>.
        /// </summary>
        public void EnableMovesBetweenPanels(ItemPanel panel, ItemPanelMoveCoordinator itemMoves)
        {
            _itemMoves = itemMoves ?? throw new ArgumentNullException(nameof(itemMoves));
            _panel = panel;
            _dragController.EnableMovesBetweenPanels(panel, itemMoves);
        }

        /// <summary>Shows an outcome's message by a slot for a moment, as when a move has no room.</summary>
        public void ShowBriefMessage(GridDropOutcome outcome, int slot)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            _tooltip.ShowBriefly(
                new ThingTooltipContent(outcome.Title, string.Empty, outcome.Description, Array.Empty<string>()),
                SlotWorldBound(slot),
                GridCellDragController.BriefMessageMilliseconds);
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
            _contents.Changed -= Refresh;
        }

        // Brings each cell up to date with its slot. Only cells whose contents changed are touched: a selection or drag
        // on a changed slot ends, since what it was about has moved, and its tooltip follows. Nothing else is disturbed (#114).
        private void Refresh()
        {
            foreach (InventoryGridCell cell in _cells)
            {
                int slot = cell.Slot;
                bool occupied = _contents.IsOccupied(slot);
                bool changed = cell.Show(occupied ? _contents.DefinitionIdAt(slot) : null, occupied ? _contents.CountBadgeAt(slot) : string.Empty);
                if (_slotGhosts.TryGetValue(slot, out VisualElement ghost))
                {
                    ghost.style.display = occupied ? DisplayStyle.None : DisplayStyle.Flex;
                }

                if (!changed)
                {
                    continue;
                }

                if (slot == _selectedSlot)
                {
                    ClearSelection();
                }

                if (slot == _dragController.PressedSlot)
                {
                    _dragController.Cancel();
                }

                // A hovered slot's tooltip follows its contents, so a count shrinking under the pointer stays true.
                if (slot == _tooltipSlot && !_tooltip.IsShowingBriefMessage && !_tooltip.IsPinned)
                {
                    if (occupied && _slotChoice == null && !_dragController.IsDragging)
                    {
                        _tooltip.Show(_contents.TooltipAt(slot), cell.Element.worldBound);
                    }
                    else
                    {
                        _tooltip.Hide();
                        _tooltipSlot = -1;
                    }
                }
            }
        }

        // One cell per slot, made once; the grid's size never changes (Docs/Design/UI/InventoryGrids.md).
        private void CreateCells()
        {
            for (int slot = 0; slot < _shape.SlotCount; slot++)
            {
                var cell = new InventoryGridCell(slot, _skin.Textures);
                RegisterCellEvents(cell);
                _cells.Add(cell);
                GridArea.Add(cell.Element);
            }
        }

        private void RegisterCellEvents(InventoryGridCell gridCell)
        {
            VisualElement cell = gridCell.Element;
            int slot = gridCell.Slot;
            cell.RegisterCallback<PointerEnterEvent>(_ =>
            {
                if (!gridCell.IsOccupied || _tooltip.IsShowingBriefMessage || _tooltip.IsPinned)
                {
                    return;
                }

                if (_slotChoice != null)
                {
                    _tooltip.Show(_slotChoice.Preview(slot), cell.worldBound);
                    _tooltipSlot = slot;
                }
                else if (!_dragController.IsDragging)
                {
                    _tooltip.Show(_contents.TooltipAt(slot), cell.worldBound);
                    _tooltipSlot = slot;
                }
            });
            cell.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                if (!_dragController.IsDragging && !_tooltip.IsShowingBriefMessage && !_tooltip.IsPinned)
                {
                    _tooltip.Hide();
                }

                if (_tooltipSlot == slot)
                {
                    _tooltipSlot = -1;
                }
            });

            // While choosing a slot, a click chooses; the drag controller ignores presses then. Otherwise a click selects.
            cell.RegisterCallback<ClickEvent>(_ =>
            {
                if (!gridCell.IsOccupied)
                {
                    return;
                }

                if (_slotChoice != null)
                {
                    _slotChoice.Chosen(slot);
                }
                else if (_itemMoves != null && _itemMoves.TrySendToOtherPanel(_panel, slot))
                {
                    // A modified click sent things to the other panel instead of selecting.
                }
                else
                {
                    Select(slot, cell);
                }
            });
            _dragController.RegisterCell(cell, slot, () => _slotChoice == null && gridCell.IsOccupied);
        }

        private void Select(int slot, VisualElement cell)
        {
            ClearSelection();
            _selectedSlot = slot;
            cell.AddToClassList("grid-cell--selected");
            _tooltip.ShowPinned(_contents.TooltipAt(slot), cell.worldBound);
        }

        private void OnAnyPointerDown(PointerDownEvent down)
        {
            if (_selectedSlot < 0)
            {
                return;
            }

            VisualElement cell = _cells[_selectedSlot].Element;
            bool onSelectedCell = down.target is VisualElement pressed
                && (pressed == cell || cell.Contains(pressed));
            if (!onSelectedCell)
            {
                ClearSelection();
            }
        }

        // Faint squares mark every slot, so gaps are visible and can be dropped on. They sit behind the items.
        private void CreateSlotBackgrounds()
        {
            for (int slot = 0; slot < _shape.SlotCount; slot++)
            {
                var background = new VisualElement { pickingMode = PickingMode.Ignore };
                background.AddToClassList("grid-slot");
                background.style.position = Position.Absolute;
                _skin.ApplyFrame(background, InterfaceFrameStyles.Slot);
                Texture2D ghost = _emptySlotGhost?.Invoke(slot);
                if (ghost != null)
                {
                    var ghostIcon = new PixelIconElement(ghost);
                    ghostIcon.AddToClassList("grid-slot-ghost");
                    background.Add(ghostIcon);
                    _slotGhosts.Add(slot, ghostIcon);
                }

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

            foreach (InventoryGridCell cell in _cells)
            {
                Place(cell.Element, cell.Slot);
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
