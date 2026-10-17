using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Drags cells of one grid panel. The dragged icon follows the pointer anywhere. Over the same grid, Core says what
    /// releasing would do and the grid shows it: for a move or swap, only the dragged thing and the one it trades
    /// places with change slots. Releasing carries out what is shown; releasing anywhere else returns the thing to its
    /// place (Docs/Design/UI/DragAndDrop.md).
    /// </summary>
    public sealed class GridCellDragController
    {
        // Movement before a press becomes a drag, so a plain click stays a click (Docs/Design/UI/DragAndDrop.md).
        private const float DragStartDistance = 6f;
        private const float DraggedIconScale = 0.8f;
        private const string DraggedSourceClassName = "grid-cell--drag-source";
        private const string CombineTargetClassName = "grid-cell--combine-target";

        private readonly GridPanelView _view;
        private readonly IGridPanelContents _contents;
        private readonly GridTooltipView _tooltip;
        private readonly VisualElement _dragLayer;

        private VisualElement _pressedCell;
        private int _pressedSlot = -1;
        private int _pointerId = -1;
        private Vector2 _pressPosition;
        private VisualElement _draggedIcon;
        private float _draggedIconSize;
        private VisualElement _highlightedCell;
        private GridDropOutcome _shownOutcome = GridDropOutcome.ReturnToPlace;

        /// <summary>Creates the controller for one grid.</summary>
        public GridCellDragController(GridPanelView view, IGridPanelContents contents, GridTooltipView tooltip, VisualElement dragLayer)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _dragLayer = dragLayer ?? throw new ArgumentNullException(nameof(dragLayer));
        }

        /// <summary>Whether a drag is in progress.</summary>
        public bool IsDragging => _draggedIcon != null;

        /// <summary>Makes the item cell in a slot draggable.</summary>
        public void RegisterCell(VisualElement cell, int slot)
        {
            cell.RegisterCallback<PointerDownEvent>(down => OnPointerDown(cell, slot, down));
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

            MoveDraggedIcon(position);
            GridDropOutcome outcome = _contents.ResolveDrop(_pressedSlot, TargetAt(position), _view.Neighborhood);
            if (!SameOutcome(outcome, _shownOutcome))
            {
                Show(outcome);
            }

            move.StopPropagation();
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

            up.StopPropagation();
        }

        private void StartDrag()
        {
            _tooltip.Hide();
            _pressedCell.AddToClassList(DraggedSourceClassName);

            _draggedIconSize = Math.Max(16f, _view.CurrentLayout.CellSize * DraggedIconScale);
            _draggedIcon = new PlaceholderIconElement(_contents.DefinitionIdAt(_pressedSlot)) { pickingMode = PickingMode.Ignore };
            _draggedIcon.AddToClassList("grid-dragged-icon");
            _draggedIcon.style.position = Position.Absolute;
            _draggedIcon.style.width = _draggedIconSize;
            _draggedIcon.style.height = _draggedIconSize;
            _dragLayer.Add(_draggedIcon);
        }

        private void MoveDraggedIcon(Vector2 pointerWorldPosition)
        {
            Vector2 local = _dragLayer.WorldToLocal(pointerWorldPosition);
            _draggedIcon.style.left = local.x - (_draggedIconSize / 2f);
            _draggedIcon.style.top = local.y - (_draggedIconSize / 2f);
        }

        // Hit-testing uses the real slots, not the previewed ones, so the preview never moves the target out from
        // under the pointer. Only the dragged cell's own grid accepts drops for now.
        private GridDropTarget TargetAt(Vector2 pointerWorldPosition)
        {
            if (!_view.Grid.contentViewport.worldBound.Contains(pointerWorldPosition))
            {
                return GridDropTarget.Nowhere;
            }

            Vector2 contentPoint = _view.Grid.contentContainer.WorldToLocal(pointerWorldPosition);
            int slot = _view.CurrentLayout.CellIndexAt(contentPoint.x, contentPoint.y, _view.DisplayedSlotCount, out float x, out float y);
            if (slot < 0)
            {
                return GridDropTarget.Nowhere;
            }

            return _contents.IsOccupied(slot)
                ? GridDropTarget.OccupiedSlot(slot, CellDropZoneClassifier.Classify(x, y))
                : GridDropTarget.EmptySlot(slot);
        }

        private void Show(GridDropOutcome outcome)
        {
            _shownOutcome = outcome;
            _highlightedCell?.RemoveFromClassList(CombineTargetClassName);
            _highlightedCell = null;

            if (outcome.Kind == GridDropOutcomeKind.Move || outcome.Kind == GridDropOutcomeKind.Swap)
            {
                _view.ShowMovePreview(_pressedSlot, outcome.TargetSlot);
            }
            else
            {
                _view.ClearMovePreview();
            }

            if (outcome.Kind == GridDropOutcomeKind.ReturnToPlace)
            {
                _tooltip.Hide();
                return;
            }

            if (outcome.Kind == GridDropOutcomeKind.Combine)
            {
                _highlightedCell = _view.ItemCellAt(outcome.AnchorSlot);
                _highlightedCell?.AddToClassList(CombineTargetClassName);
            }

            Rect anchor = outcome.AnchorSlot >= 0 ? _view.SlotWorldBound(outcome.AnchorSlot) : _draggedIcon.worldBound;
            _tooltip.Show(new ThingTooltipContent(outcome.Title, outcome.Description, Array.Empty<string>()), anchor);
        }

        private static bool SameOutcome(GridDropOutcome first, GridDropOutcome second) =>
            first.Kind == second.Kind
            && first.TargetSlot == second.TargetSlot
            && first.AnchorSlot == second.AnchorSlot
            && first.Description == second.Description;

        private void EndDrag()
        {
            _highlightedCell?.RemoveFromClassList(CombineTargetClassName);
            _highlightedCell = null;
            if (_draggedIcon != null)
            {
                _view.ClearMovePreview();
            }

            _tooltip.Hide();
            _draggedIcon?.RemoveFromHierarchy();
            _draggedIcon = null;
            _shownOutcome = GridDropOutcome.ReturnToPlace;

            if (_pressedCell != null)
            {
                _pressedCell.RemoveFromClassList(DraggedSourceClassName);
                if (_pressedCell.HasPointerCapture(_pointerId))
                {
                    _pressedCell.ReleasePointer(_pointerId);
                }
            }

            _pressedCell = null;
            _pressedSlot = -1;
            _pointerId = -1;
        }
    }
}
