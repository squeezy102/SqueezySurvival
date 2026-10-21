using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.GameScreen
{
    /// <summary>
    /// The overlays open over the game screen, newest on top. Only the top one is shown; closing it shows the one it
    /// was opened from, so Options opened from the pause menu returns there (Docs/Design/UI/Options.md).
    /// </summary>
    public sealed class GameScreenOverlayStack
    {
        private readonly List<GameScreenOverlay> _openOldestFirst = new List<GameScreenOverlay>();

        /// <summary>Raised after an overlay opens or closes.</summary>
        public event EventHandler<GameScreenOverlayChange> Changed;

        /// <summary>The overlay shown, or null when none is open.</summary>
        public GameScreenOverlay? Top => _openOldestFirst.Count > 0 ? _openOldestFirst[_openOldestFirst.Count - 1] : (GameScreenOverlay?)null;

        /// <summary>Whether <paramref name="overlay"/> is open, shown or not.</summary>
        public bool IsOpen(GameScreenOverlay overlay) => _openOldestFirst.Contains(overlay);

        /// <summary>Opens <paramref name="overlay"/> on top. Does nothing if it is already open.</summary>
        public void Open(GameScreenOverlay overlay)
        {
            if (IsOpen(overlay))
            {
                return;
            }

            _openOldestFirst.Add(overlay);
            Changed?.Invoke(this, new GameScreenOverlayChange(overlay, opened: true, Top));
        }

        /// <summary>Closes the overlay on top, showing the one beneath it. Does nothing when none is open.</summary>
        public void CloseTop()
        {
            GameScreenOverlay? closing = Top;
            if (closing == null)
            {
                return;
            }

            _openOldestFirst.RemoveAt(_openOldestFirst.Count - 1);
            Changed?.Invoke(this, new GameScreenOverlayChange(closing.Value, opened: false, Top));
        }
    }
}
