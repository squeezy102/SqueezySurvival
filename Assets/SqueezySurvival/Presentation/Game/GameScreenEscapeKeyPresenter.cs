using System;
using System.Collections.Generic;
using SqueezySurvival.Core.GameScreen;
using UnityEngine.InputSystem;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Reads Escape and carries out what Core's <see cref="GameScreenEscapeResolver"/> decides
    /// (Docs/Design/UI/Controls.md, Escape).
    /// </summary>
    public sealed class GameScreenEscapeKeyPresenter : IDisposable
    {
        private readonly GameScreenEscapeResolver _resolver;
        private readonly IReadOnlyList<InventoryGridView> _grids;
        private readonly GroundOverflowPromptController _groundOverflowPrompt;
        private readonly InputAction _cancelAction;

        /// <summary>Starts listening for Escape.</summary>
        public GameScreenEscapeKeyPresenter(
            GameScreenEscapeResolver resolver,
            IReadOnlyList<InventoryGridView> grids,
            GroundOverflowPromptController groundOverflowPrompt)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _grids = grids ?? throw new ArgumentNullException(nameof(grids));
            _groundOverflowPrompt = groundOverflowPrompt ?? throw new ArgumentNullException(nameof(groundOverflowPrompt));

            // Escape is read from the keyboard directly rather than from UI navigation, which goes only to the focused
            // element and is lost once a non-focusable grid cell is clicked (#48).
            _cancelAction = new InputAction("Cancel", InputActionType.Button, "<Keyboard>/escape");
            _cancelAction.performed += OnEscapePressed;
            _cancelAction.Enable();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _cancelAction.performed -= OnEscapePressed;
            _cancelAction.Disable();
            _cancelAction.Dispose();
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            var interaction = new GameScreenInteraction(
                AnyGrid(grid => grid.IsDragging),
                AnyGrid(grid => grid.HasSelection),
                _groundOverflowPrompt.IsChoosingDiscard,
                _groundOverflowPrompt.IsAsking);

            switch (_resolver.PressEscape(interaction))
            {
                case GameScreenEscapeAction.CancelDrag:
                    ForEachGrid(grid => grid.CancelDrag());
                    break;
                case GameScreenEscapeAction.ClearSelection:
                    ForEachGrid(grid => grid.ClearSelection());
                    break;
                case GameScreenEscapeAction.ReturnToGroundOverflowQuestion:
                    _groundOverflowPrompt.ReturnToQuestion();
                    break;
                case GameScreenEscapeAction.WaitForGroundOverflowAnswer:
                case GameScreenEscapeAction.CloseTopOverlay:
                case GameScreenEscapeAction.OpenPauseMenu:
                    // Nothing to do here: the question waits, and Core has already changed the overlays.
                    break;
                default:
                    throw new InvalidOperationException("Unknown Escape action.");
            }
        }

        private bool AnyGrid(Func<InventoryGridView, bool> condition)
        {
            foreach (InventoryGridView grid in _grids)
            {
                if (condition(grid))
                {
                    return true;
                }
            }

            return false;
        }

        private void ForEachGrid(Action<InventoryGridView> action)
        {
            foreach (InventoryGridView grid in _grids)
            {
                action(grid);
            }
        }
    }
}
