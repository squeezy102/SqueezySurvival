namespace SqueezySurvival.Core.GameScreen
{
    /// <summary>One overlay opening or closing, and the overlay shown after it. Immutable.</summary>
    public sealed class GameScreenOverlayChange
    {
        /// <summary>Describes <paramref name="overlay"/> opening or closing, leaving <paramref name="shownAfter"/> on top.</summary>
        public GameScreenOverlayChange(GameScreenOverlay overlay, bool opened, GameScreenOverlay? shownAfter)
        {
            Overlay = overlay;
            Opened = opened;
            ShownAfter = shownAfter;
        }

        /// <summary>The overlay that opened or closed.</summary>
        public GameScreenOverlay Overlay { get; }

        /// <summary>True when it opened; false when it closed.</summary>
        public bool Opened { get; }

        /// <summary>The overlay on top after the change, or null when the game screen is uncovered.</summary>
        public GameScreenOverlay? ShownAfter { get; }
    }
}
