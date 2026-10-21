using System;

namespace SqueezySurvival.Core.GameScreen
{
    /// <summary>
    /// Decides what Escape does on the game screen: the first that applies, in the order
    /// Docs/Design/UI/Controls.md (Escape) defines. Overlays are opened and closed through
    /// <see cref="GameScreenOverlayStack"/>; the rest is carried out by the screen.
    /// </summary>
    public sealed class GameScreenEscapeResolver
    {
        private readonly GameScreenOverlayStack _overlays;

        /// <summary>Resolves Escape against <paramref name="overlays"/>.</summary>
        public GameScreenEscapeResolver(GameScreenOverlayStack overlays)
        {
            _overlays = overlays ?? throw new ArgumentNullException(nameof(overlays));
        }

        /// <summary>
        /// Handles Escape: opens or closes an overlay itself, and returns what was decided so the screen can carry out
        /// the rest (cancel a drag, clear a selection, return to the ground overflow question).
        /// </summary>
        public GameScreenEscapeAction PressEscape(GameScreenInteraction interaction)
        {
            GameScreenEscapeAction action = Resolve(interaction);
            if (action == GameScreenEscapeAction.CloseTopOverlay)
            {
                _overlays.CloseTop();
            }
            else if (action == GameScreenEscapeAction.OpenPauseMenu)
            {
                _overlays.Open(GameScreenOverlay.PauseMenu);
            }

            return action;
        }

        private GameScreenEscapeAction Resolve(GameScreenInteraction interaction)
        {
            if (interaction == null)
            {
                throw new ArgumentNullException(nameof(interaction));
            }

            if (interaction.IsDragging)
            {
                return GameScreenEscapeAction.CancelDrag;
            }

            if (interaction.HasSelection)
            {
                return GameScreenEscapeAction.ClearSelection;
            }

            if (interaction.IsChoosingDiscard)
            {
                return GameScreenEscapeAction.ReturnToGroundOverflowQuestion;
            }

            if (interaction.IsAskingAboutGroundOverflow)
            {
                return GameScreenEscapeAction.WaitForGroundOverflowAnswer;
            }

            // Open overlays stack, so the top one is the notice or Options when either is open, else the pause menu.
            return _overlays.Top != null ? GameScreenEscapeAction.CloseTopOverlay : GameScreenEscapeAction.OpenPauseMenu;
        }
    }
}
