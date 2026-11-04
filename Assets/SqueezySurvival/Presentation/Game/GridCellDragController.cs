using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Drags cells of one inventory grid. While dragging, nothing in the grid moves: a copy of the icon follows the
    /// pointer, a marker shows where it would go, and a tooltip says what releasing would do, as decided by Core.
    /// Releasing over the grid carries that out; releasing anywhere else changes nothing (Docs/Design/UI/DragAndDrop.md).
    /// It also owns the click: every press of a cell ends as a drag or as a click, never both, as Core's
    /// <see cref="PointerPressGesture"/> decides, so the grid does not listen for Unity's click event at all.
    /// </summary>
    public sealed class GridCellDragController
    {
        // How often a held drag is re-checked, so the combine hold completes even while the pointer is still.
        private const long HoldCheckIntervalMilliseconds = 50;

        /// <summary>How long a brief message by a slot stays up, such as a combine that can't happen (Docs/Design/UI/DragAndDrop.md).</summary>
        public const long BriefMessageMilliseconds = 2000;

        private readonly InventoryGridView _view;
        private readonly IInventoryGridContents _contents;
        private readonly GridTooltipView _tooltip;
        private readonly VisualElement _dragLayer;

        private VisualElement _pressedCell;
        private int _pressedSlot = -1;
        private bool _pressCanDrag;
        private Action _pressClickAction;
        private int _pointerId = -1;
        private readonly PointerPressGesture _pressGesture = new PointerPressGesture();
        private VisualElement _draggedIcon;
        private float _draggedIconSize;
        private readonly PixelArtTextureCache _textures;
        private GridDropOutcome _shownOutcome = GridDropOutcome.ReturnToPlace;
        private readonly CombineHoldTimer _combineHoldTimer = new CombineHoldTimer();
        private readonly DropOutcomeMemo _outcomeMemo = new DropOutcomeMemo();
        private Vector2 _pointerPosition;
        private IVisualElementScheduledItem _holdCheck;
        private ItemPanelMoveCoordinator _itemMoves;
        private ItemPanel _panel;
        private InventoryGridView _shownTargetView;
        private int _shownHoveredSlot = -1;
        private ItemTransferQuantity _shownQuantity = ItemTransferQuantity.All;

        /// <summary>Creates the controller for one grid.</summary>
        public GridCellDragController(
            InventoryGridView view,
            IInventoryGridContents contents,
            GridTooltipView tooltip,
            VisualElement dragLayer,
            PixelArtTextureCache textures)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _dragLayer = dragLayer ?? throw new ArgumentNullException(nameof(dragLayer));
            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
        }

        /// <summary>Lets drags carry this grid's things to the other panel (Docs/Design/UI/DragAndDrop.md, Between panels).</summary>
        public void EnableMovesBetweenPanels(ItemPanel panel, ItemPanelMoveCoordinator itemMoves)
        {
            _itemMoves = itemMoves ?? throw new ArgumentNullException(nameof(itemMoves));
            _panel = panel;
        }

        /// <summary>The slot pressed or being dragged, or -1.</summary>
        public int PressedSlot => _pressedSlot;

        /// <summary>Whether a drag is in progress.</summary>
        public bool IsDragging => _draggedIcon != null;

        /// <summary>
        /// Makes the cell in a slot respond to presses whenever <paramref name="canPress"/> says so. A press that moves
        /// far enough is a drag, when <paramref name="canDrag"/> says the cell may be dragged; one that does not, and is
        /// let go over the cell, calls <paramref name="click"/>. It is never both.
        /// </summary>
        public void RegisterCell(VisualElement cell, int slot, Func<bool> canPress, Func<bool> canDrag, Action click)
        {
            if (canPress == null)
            {
                throw new ArgumentNullException(nameof(canPress));
            }

            if (canDrag == null)
            {
                throw new ArgumentNullException(nameof(canDrag));
            }

            if (click == null)
            {
                throw new ArgumentNullException(nameof(click));
            }

            cell.RegisterCallback<PointerDownEvent>(down =>
            {
                if (canPress())
                {
                    OnPointerDown(cell, slot, down, canDrag(), click);
                }
            });
            cell.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            cell.RegisterCallback<PointerUpEvent>(OnPointerUp);
            cell.RegisterCallback<PointerCaptureOutEvent>(_ => Cancel());
        }

        /// <summary>Ends any press or drag without changing anything.</summary>
        public void Cancel()
        {
            EndDrag();
        }

        private void OnPointerDown(VisualElement cell, int slot, PointerDownEvent down, bool canDrag, Action click)
        {
            if (down.button != 0 || _pressedCell != null)
            {
                return;
            }

            _pressedCell = cell;
            _pressedSlot = slot;
            _pressCanDrag = canDrag;
            _pressClickAction = click;
            _pointerId = down.pointerId;
            _pressGesture.Press(down.position.x, down.position.y);

            // Capturing the pointer keeps move and release events coming to this cell wherever the pointer goes.
            cell.CapturePointer(down.pointerId);
            down.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent move)
        {
            if (_pressedCell == null || move.pointerId != _pointerId)
            {
                return;
            }

            Vector2 position = move.position;
            if (!_pressGesture.Move(position.x, position.y) || !_pressCanDrag)
            {
                return;
            }

            if (!IsDragging)
            {
                StartDrag();
            }

            _pointerPosition = position;
            MoveDraggedIcon(position);
            RefreshOutcome();
            move.StopPropagation();
        }

        private void RefreshOutcome()
        {
            int hoveredSlot = -1;
            InventoryGridView otherView = _itemMoves == null ? null : _itemMoves.OtherPanelUnder(_panel, _pointerPosition, out hoveredSlot);
            _shownHoveredSlot = hoveredSlot;

            // Grids that don't take part in moves between panels (zone features, hands) always move whole stacks.
            _shownQuantity = _itemMoves == null ? ItemTransferQuantity.All : _itemMoves.QuantityForDrag();
            GridDropOutcome outcome;
            if (otherView != null)
            {
                _combineHoldTimer.Reset();
                DropOutcomeKey key = DropOutcomeKey.ForOtherPanel(
                    _pressedSlot, _shownHoveredSlot, _shownQuantity, _itemMoves.CanTransfer, _itemMoves.PanelContentsRevision);
                if (!_outcomeMemo.TryGet(key, out outcome))
                {
                    outcome = _itemMoves.Resolve(_panel, _pressedSlot, _shownHoveredSlot, _shownQuantity);
                    _outcomeMemo.Store(key, outcome);
                }
            }
            else
            {
                GridDropTarget target = TargetAt(_pointerPosition);
                DropOutcomeKey key = DropOutcomeKey.ForOwnGrid(_pressedSlot, target, _shownQuantity, _contents.Revision);
                if (!_outcomeMemo.TryGet(key, out outcome))
                {
                    outcome = _contents.ResolveDrop(_pressedSlot, target, _shownQuantity);
                    _outcomeMemo.Store(key, outcome);
                }
            }

            InventoryGridView targetView = otherView ?? _view;
            if (targetView != _shownTargetView || !SameOutcome(outcome, _shownOutcome))
            {
                _shownTargetView?.HideDropTarget();
                _shownTargetView = targetView;
                Show(outcome);
            }
        }

        private void OnPointerUp(PointerUpEvent up)
        {
            if (_pressedCell == null || up.pointerId != _pointerId)
            {
                return;
            }

            // Releasing carries out exactly what was being shown.
            GridDropOutcome outcome = IsDragging ? _shownOutcome : GridDropOutcome.ReturnToPlace;
            InventoryGridView targetView = _shownTargetView ?? _view;
            int sourceSlot = _pressedSlot;
            int hoveredSlot = _shownHoveredSlot;
            ItemTransferQuantity quantity = _shownQuantity;
            Action click = _pressClickAction;
            PointerPressEnding ending = _pressGesture.Release(_pressedCell.worldBound.Contains((Vector2)up.position));
            EndDrag();
            if (ending == PointerPressEnding.Click)
            {
                click();
            }
            else if (outcome.Kind == GridDropOutcomeKind.Transfer)
            {
                _itemMoves.Apply(_panel, sourceSlot, hoveredSlot, quantity);
            }
            else if (outcome.ChangesSomething)
            {
                _contents.ApplyDrop(sourceSlot, outcome);
            }
            else if ((outcome.Kind == GridDropOutcomeKind.CannotCombine || outcome.Kind == GridDropOutcomeKind.NoRoom) && outcome.AnchorSlot >= 0)
            {
                // The thing goes back, and the player is told why by the slot it was held over.
                targetView.ShowBriefMessage(outcome, outcome.AnchorSlot);
            }

            up.StopPropagation();
        }

        private void StartDrag()
        {
            // Moving a thing ends any selection in this grid (Docs/Design/UI/InventoryGrids.md, Selecting).
            _view.ClearSelection();
            _tooltip.Hide();

            // The same whole-pixel size as the icon in its cell, so the art pixels don't change size when picked up.
            _draggedIconSize = PixelIconElement.WholePixelSize(_view.CurrentLayout.CellSize);
            _draggedIcon = new PixelIconElement(_textures.Icon(_contents.DefinitionIdAt(_pressedSlot)), fitsParent: false);
            _draggedIcon.AddToClassList("grid-dragged-icon");
            _draggedIcon.style.position = Position.Absolute;
            _draggedIcon.style.width = _draggedIconSize;
            _draggedIcon.style.height = _draggedIconSize;
            _dragLayer.Add(_draggedIcon);
            _holdCheck = _dragLayer.schedule.Execute(RefreshOutcome).Every(HoldCheckIntervalMilliseconds);
        }

        private void MoveDraggedIcon(Vector2 pointerWorldPosition)
        {
            Vector2 local = _dragLayer.WorldToLocal(pointerWorldPosition);
            _draggedIcon.style.left = local.x - (_draggedIconSize / 2f);
            _draggedIcon.style.top = local.y - (_draggedIconSize / 2f);
        }

        // Only the dragged cell's own grid accepts drops for now.
        private GridDropTarget TargetAt(Vector2 pointerWorldPosition)
        {
            int slot = _view.SlotAtWorldPosition(pointerWorldPosition, out _, out _);
            bool overAThing = slot >= 0 && _contents.IsOccupied(slot);
            bool attemptsCombine = _combineHoldTimer.Update(overAThing ? slot : -1, Time.unscaledTimeAsDouble);
            if (slot < 0)
            {
                return GridDropTarget.Nowhere;
            }

            return _contents.IsOccupied(slot)
                ? GridDropTarget.OccupiedSlot(slot, attemptsCombine)
                : GridDropTarget.EmptySlot(slot);
        }

        private void Show(GridDropOutcome outcome)
        {
            _shownOutcome = outcome;
            InventoryGridView targetView = _shownTargetView ?? _view;
            if (outcome.ChangesSomething)
            {
                targetView.ShowDropTarget(outcome.AnchorSlot);
            }
            else
            {
                targetView.HideDropTarget();
            }

            if (outcome.Kind == GridDropOutcomeKind.ReturnToPlace)
            {
                _tooltip.Hide();
                return;
            }

            Rect anchor = outcome.AnchorSlot >= 0 ? targetView.SlotWorldBound(outcome.AnchorSlot) : _draggedIcon.worldBound;
            _tooltip.Show(new ThingTooltipContent(outcome.Title, string.Empty, outcome.Description, Array.Empty<string>()), anchor);
        }

        private static bool SameOutcome(GridDropOutcome first, GridDropOutcome second) =>
            first.Kind == second.Kind
            && first.TargetSlot == second.TargetSlot
            && first.AnchorSlot == second.AnchorSlot
            && first.Description == second.Description;

        private void EndDrag()
        {
            _holdCheck?.Pause();
            _holdCheck = null;
            _combineHoldTimer.Reset();
            _outcomeMemo.Clear();
            _view.HideDropTarget();
            _shownTargetView?.HideDropTarget();
            _shownTargetView = null;
            _shownHoveredSlot = -1;
            _shownQuantity = ItemTransferQuantity.All;

            _tooltip.Hide();
            _draggedIcon?.RemoveFromHierarchy();
            _draggedIcon = null;
            _shownOutcome = GridDropOutcome.ReturnToPlace;

            if (_pressedCell != null && _pressedCell.HasPointerCapture(_pointerId))
            {
                _pressedCell.ReleasePointer(_pointerId);
            }

            _pressedCell = null;
            _pressedSlot = -1;
            _pressCanDrag = false;
            _pressClickAction = null;
            _pointerId = -1;
        }
    }
}
