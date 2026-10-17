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
    /// Drags cells of one grid panel. The dragged icon follows the pointer anywhere; over the same grid, Core says
    /// what releasing would do and the preview shows it; releasing carries it out, and releasing anywhere else returns
    /// the thing to its place (Docs/Design/UI/DragAndDrop.md).
    /// </summary>
    public sealed class GridCellDragController
    {
        // Movement before a press becomes a drag, so a plain click stays a click (Docs/Design/UI/DragAndDrop.md).
        private const float DragStartDistance = 6f;
        private const float MarkerThickness = 4f;
        private const float DraggedIconScale = 0.8f;
        private const string DraggedSourceClassName = "grid-cell--drag-source";
        private const string CombineTargetClassName = "grid-cell--combine-target";

        private readonly GridPanelView _view;
        private readonly IGridPanelContents _contents;
        private readonly GridTooltipView _tooltip;
        private readonly VisualElement _dragLayer;
        private readonly VisualElement _insertionMarker;

        private VisualElement _pressedCell;
        private int _pressedIndex = -1;
        private int _pointerId = -1;
        private Vector2 _pressPosition;
        private VisualElement _draggedIcon;
        private float _draggedIconSize;
        private VisualElement _highlightedCell;

        /// <summary>Creates the controller for one grid.</summary>
        public GridCellDragController(GridPanelView view, IGridPanelContents contents, GridTooltipView tooltip, VisualElement dragLayer)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _contents = contents ?? throw new ArgumentNullException(nameof(contents));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _dragLayer = dragLayer ?? throw new ArgumentNullException(nameof(dragLayer));
            _insertionMarker = new VisualElement { pickingMode = PickingMode.Ignore };
            _insertionMarker.AddToClassList("grid-insertion-marker");
            _insertionMarker.style.position = Position.Absolute;
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
            ShowOutcome(_contents.ResolveDrop(_pressedIndex, TargetAt(position)));
            move.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent up)
        {
            if (_pressedCell == null || up.pointerId != _pointerId)
            {
                return;
            }

            GridDropOutcome outcome = IsDragging ? _contents.ResolveDrop(_pressedIndex, TargetAt(up.position)) : GridDropOutcome.ReturnToPlace;
            int sourceIndex = _pressedIndex;
            EndDrag();

            // Nothing happens until release, and then only what the preview showed.
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

        // Only the dragged cell's own grid accepts drops for now; anywhere else returns it.
        private GridDropTarget TargetAt(Vector2 pointerWorldPosition)
        {
            if (!_view.Grid.contentViewport.worldBound.Contains(pointerWorldPosition))
            {
                return GridDropTarget.Nowhere;
            }

            Vector2 contentPoint = _view.Grid.contentContainer.WorldToLocal(pointerWorldPosition);
            int index = _view.CurrentLayout.CellIndexAt(contentPoint.x, contentPoint.y, _contents.Count, out float x, out float y);
            return index < 0
                ? GridDropTarget.EmptyGridSpace
                : GridDropTarget.Cell(index, CellDropZoneClassifier.Classify(x, y));
        }

        private void ShowOutcome(GridDropOutcome outcome)
        {
            ClearPreview();
            if (outcome.Kind == GridDropOutcomeKind.ReturnToPlace)
            {
                return;
            }

            if (outcome.TargetCellIndex >= 0)
            {
                VisualElement target = _view.CellAt(outcome.TargetCellIndex);
                if (outcome.Kind == GridDropOutcomeKind.Combine)
                {
                    _highlightedCell = target;
                    target.AddToClassList(CombineTargetClassName);
                }

                if (outcome.MarkerSide != InsertionMarkerSide.None)
                {
                    PlaceInsertionMarker(outcome.TargetCellIndex, outcome.MarkerSide);
                }

                _tooltip.Show(new ThingTooltipContent(outcome.Title, outcome.Description, Array.Empty<string>()), target.worldBound);
            }
            else
            {
                _tooltip.Show(new ThingTooltipContent(outcome.Title, outcome.Description, Array.Empty<string>()), _draggedIcon.worldBound);
            }
        }

        // The marker sits in the gap on the side of the target where the thing will land: left or right of it in a
        // vertical grid, above or below it in a horizontal one.
        private void PlaceInsertionMarker(int targetIndex, InsertionMarkerSide side)
        {
            GridPanelLayout layout = _view.CurrentLayout;
            GridCellPosition cell = layout.PositionOf(targetIndex);
            float halfGap = GridPanelLayoutCalculator.CellGap / 2f;
            bool vertical = _view.ScrollDirection == GridPanelScrollDirection.Vertical;
            bool before = side == InsertionMarkerSide.Before;

            if (vertical)
            {
                float edge = before ? cell.Left - halfGap : cell.Left + layout.CellSize + halfGap;
                _insertionMarker.style.left = edge - (MarkerThickness / 2f);
                _insertionMarker.style.top = cell.Top;
                _insertionMarker.style.width = MarkerThickness;
                _insertionMarker.style.height = layout.CellSize;
            }
            else
            {
                float edge = before ? cell.Top - halfGap : cell.Top + layout.CellSize + halfGap;
                _insertionMarker.style.left = cell.Left;
                _insertionMarker.style.top = edge - (MarkerThickness / 2f);
                _insertionMarker.style.width = layout.CellSize;
                _insertionMarker.style.height = MarkerThickness;
            }

            _view.Grid.contentContainer.Add(_insertionMarker);
        }

        private void ClearPreview()
        {
            _insertionMarker.RemoveFromHierarchy();
            _highlightedCell?.RemoveFromClassList(CombineTargetClassName);
            _highlightedCell = null;
        }

        private void EndDrag()
        {
            ClearPreview();
            _tooltip.Hide();
            _draggedIcon?.RemoveFromHierarchy();
            _draggedIcon = null;

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
