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
    /// </summary>
    public sealed class GridCellDragController
    {
        // Movement before a press becomes a drag, so a plain click stays a click (Docs/Design/UI/DragAndDrop.md).
        private const float DragStartDistance = 6f;

        // How often a held drag is re-checked, so the combine hold completes even while the pointer is still.
        private const long HoldCheckIntervalMilliseconds = 50;

        // How long the message for a combine that can't happen stays up (Docs/Design/UI/DragAndDrop.md).
        private const long CannotCombineMessageMilliseconds = 2000;

        private readonly InventoryGridView _view;
        private readonly IInventoryGridContents _contents;
        private readonly GridTooltipView _tooltip;
        private readonly VisualElement _dragLayer;

        private VisualElement _pressedCell;
        private int _pressedSlot = -1;
        private int _pointerId = -1;
        private Vector2 _pressPosition;
        private VisualElement _draggedIcon;
        private float _draggedIconSize;
        private readonly PixelArtTextureCache _textures;
        private GridDropOutcome _shownOutcome = GridDropOutcome.ReturnToPlace;
        private readonly CombineHoldTimer _combineHoldTimer = new CombineHoldTimer();
        private Vector2 _pointerPosition;
        private IVisualElementScheduledItem _holdCheck;

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

        /// <summary>Whether a drag is in progress.</summary>
        public bool IsDragging => _draggedIcon != null;

        /// <summary>Makes the item cell in a slot draggable whenever <paramref name="canDrag"/> says so.</summary>
        public void RegisterCell(VisualElement cell, int slot, Func<bool> canDrag)
        {
            if (canDrag == null)
            {
                throw new ArgumentNullException(nameof(canDrag));
            }

            cell.RegisterCallback<PointerDownEvent>(down =>
            {
                if (canDrag())
                {
                    OnPointerDown(cell, slot, down);
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

        private void OnPointerDown(VisualElement cell, int slot, PointerDownEvent down)
        {
            if (down.button != 0 || _pressedCell != null)
            {
                return;
            }

            _pressedCell = cell;
            _pressedSlot = slot;
            _pointerId = down.pointerId;
            _pressPosition = down.position;

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
            if (!IsDragging)
            {
                if (Vector2.Distance(position, _pressPosition) < DragStartDistance)
                {
                    return;
                }

                StartDrag();
            }

            _pointerPosition = position;
            MoveDraggedIcon(position);
            RefreshOutcome();
            move.StopPropagation();
        }

        private void RefreshOutcome()
        {
            GridDropOutcome outcome = _contents.ResolveDrop(_pressedSlot, TargetAt(_pointerPosition));
            if (!SameOutcome(outcome, _shownOutcome))
            {
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
            int sourceSlot = _pressedSlot;
            EndDrag();
            if (outcome.ChangesSomething)
            {
                _contents.ApplyDrop(sourceSlot, outcome);
            }
            else if (outcome.Kind == GridDropOutcomeKind.CannotCombine)
            {
                // The thing goes back, and the player is told why by the slot it was held over.
                _tooltip.ShowBriefly(
                    new ThingTooltipContent(outcome.Title, string.Empty, outcome.Description, Array.Empty<string>()),
                    _view.SlotWorldBound(outcome.AnchorSlot),
                    CannotCombineMessageMilliseconds);
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
            if (outcome.ChangesSomething)
            {
                _view.ShowDropTarget(outcome.AnchorSlot);
            }
            else
            {
                _view.HideDropTarget();
            }

            if (outcome.Kind == GridDropOutcomeKind.ReturnToPlace)
            {
                _tooltip.Hide();
                return;
            }

            Rect anchor = outcome.AnchorSlot >= 0 ? _view.SlotWorldBound(outcome.AnchorSlot) : _draggedIcon.worldBound;
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
            _view.HideDropTarget();

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
            _pointerId = -1;
        }
    }
}
