using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Options;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Drags cells of one grid panel. The dragged icon follows the pointer anywhere. Over the same grid, Core says what
    /// releasing would do, and the grid shows it: for a move, the cells slide aside and the dragged cell's slot appears
    /// where it will land. Releasing carries out what is shown; releasing anywhere else returns the thing to its place
    /// (Docs/Design/UI/DragAndDrop.md).
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
        private int _pressedIndex = -1;
        private int _pointerId = -1;
        private Vector2 _pressPosition;
        private VisualElement _draggedIcon;
        private float _draggedIconSize;
        private VisualElement _highlightedCell;
        private GridDropOutcome _shownOutcome = GridDropOutcome.ReturnToPlace;
        private GridDropOutcome _outcomeInGrid = GridDropOutcome.ReturnToPlace;

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

        /// <summary>Makes a cell draggable.</summary>
        public void RegisterCell(VisualElement cell, int index)
        {
            cell.RegisterCallback<PointerDownEvent>(down => OnPointerDown(cell, index, down));
            cell.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            cell.RegisterCallback<PointerUpEvent>(OnPointerUp);
            cell.RegisterCallback<PointerCaptureOutEvent>(_ => Cancel());
        }

        /// <summary>Ends any press or drag without changing anything.</summary>
        public void Cancel()
        {
            EndDrag();
        }

        private void OnPointerDown(VisualElement cell, int index, PointerDownEvent down)
        {
            if (down.button != 0 || _pressedCell != null)
            {
                return;
            }

            _pressedCell = cell;
            _pressedIndex = index;
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
            UpdateOutcome(position);
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
            int sourceIndex = _pressedIndex;
            EndDrag();
            if (outcome.ChangesSomething)
            {
                _contents.ApplyDrop(sourceIndex, outcome);
            }

            up.StopPropagation();
        }

        private void StartDrag()
        {
            _tooltip.Hide();
            _pressedCell.AddToClassList(DraggedSourceClassName);
            _outcomeInGrid = _contents.ResolveDrop(_pressedIndex, GridDropTarget.Cell(_pressedIndex, CellDropZone.Center));

            _draggedIconSize = Math.Max(16f, _view.CurrentLayout.CellSize * DraggedIconScale);
            _draggedIcon = new PlaceholderIconElement(_contents.DefinitionIdAt(_pressedIndex)) { pickingMode = PickingMode.Ignore };
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

        private void UpdateOutcome(Vector2 pointerWorldPosition)
        {
            GridDropOutcome outcome;
            if (!_view.Grid.contentViewport.worldBound.Contains(pointerWorldPosition))
            {
                // Only the dragged cell's own grid accepts drops for now; anywhere else returns it.
                outcome = GridDropOutcome.ReturnToPlace;
            }
            else
            {
                Vector2 contentPoint = _view.Grid.contentContainer.WorldToLocal(pointerWorldPosition);
                int slot = _view.CurrentLayout.CellIndexAt(contentPoint.x, contentPoint.y, _contents.Count, out float x, out float y);
                int index = slot < 0 ? -1 : _view.IndexAtSlot(slot);
                if (index == _pressedIndex)
                {
                    // Over the dragged cell's own slot: what is already shown stands.
                    outcome = _outcomeInGrid;
                }
                else
                {
                    GridDropTarget target = index < 0
                        ? GridDropTarget.EmptyGridSpace
                        : GridDropTarget.Cell(index, CellDropZoneClassifier.Classify(x, y));
                    outcome = _contents.ResolveDrop(_pressedIndex, target);
                    _outcomeInGrid = outcome;
                }
            }

            if (!SameOutcome(outcome, _shownOutcome))
            {
                Show(outcome);
            }
        }

        private void Show(GridDropOutcome outcome)
        {
            _shownOutcome = outcome;
            _highlightedCell?.RemoveFromClassList(CombineTargetClassName);
            _highlightedCell = null;

            switch (outcome.Kind)
            {
                case GridDropOutcomeKind.ReturnToPlace:
                    _view.ClearMovePreview();
                    _tooltip.Hide();
                    return;
                case GridDropOutcomeKind.Move:
                    _view.ShowMovePreview(_pressedIndex, OrderedListMove.ResultingIndex(_pressedIndex, outcome.InsertionIndex));
                    break;
                case GridDropOutcomeKind.NoChange:
                    _view.ClearMovePreview();
                    break;
                case GridDropOutcomeKind.Combine:
                    _view.ClearMovePreview();
                    _highlightedCell = _view.CellAt(outcome.TargetCellIndex);
                    _highlightedCell.AddToClassList(CombineTargetClassName);
                    break;
                default:
                    // Can't combine: nothing moves; the tooltip says why.
                    _view.ClearMovePreview();
                    break;
            }

            // The tooltip sits beside the cell the outcome concerns: the dragged cell's slot for a move, the target
            // for a combination.
            VisualElement anchor = outcome.Kind == GridDropOutcomeKind.Combine || outcome.Kind == GridDropOutcomeKind.CannotCombine
                ? _view.CellAt(outcome.TargetCellIndex)
                : _pressedCell;
            _tooltip.Show(new ThingTooltipContent(outcome.Title, outcome.Description, Array.Empty<string>()), SlotWorldBound(anchor));
        }

        // Cells slide to their previewed slots with a short transition, so the tooltip is placed against the slot a
        // cell is going to, not where it is mid-slide.
        private Rect SlotWorldBound(VisualElement cell)
        {
            GridCellPosition position = _view.CurrentLayout.PositionOf(_view.SlotOf(_view.IndexOfCell(cell)));
            float size = _view.CurrentLayout.CellSize;
            Vector2 topLeft = _view.Grid.contentContainer.LocalToWorld(new Vector2(position.Left, position.Top));
            return new Rect(topLeft, new Vector2(size, size));
        }

        private static bool SameOutcome(GridDropOutcome first, GridDropOutcome second) =>
            first.Kind == second.Kind
            && first.InsertionIndex == second.InsertionIndex
            && first.TargetCellIndex == second.TargetCellIndex
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
            _outcomeInGrid = GridDropOutcome.ReturnToPlace;

            if (_pressedCell != null)
            {
                _pressedCell.RemoveFromClassList(DraggedSourceClassName);
                if (_pressedCell.HasPointerCapture(_pointerId))
                {
                    _pressedCell.ReleasePointer(_pointerId);
                }
            }

            _pressedCell = null;
            _pressedIndex = -1;
            _pointerId = -1;
        }
    }
}
