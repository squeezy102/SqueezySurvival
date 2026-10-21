namespace SqueezySurvival.Core.GameScreen
{
    /// <summary>What the player is in the middle of on the game screen, as Escape needs to know it. Immutable.</summary>
    public sealed class GameScreenInteraction
    {
        /// <summary>Describes the player's current interaction.</summary>
        public GameScreenInteraction(bool isDragging, bool hasSelection, bool isChoosingDiscard, bool isAskingAboutGroundOverflow)
        {
            IsDragging = isDragging;
            HasSelection = hasSelection;
            IsChoosingDiscard = isChoosingDiscard;
            IsAskingAboutGroundOverflow = isAskingAboutGroundOverflow;
        }

        /// <summary>A thing is being dragged in any grid.</summary>
        public bool IsDragging { get; }

        /// <summary>A slot is selected in any grid.</summary>
        public bool HasSelection { get; }

        /// <summary>The player is choosing what to discard for a find with no room.</summary>
        public bool IsChoosingDiscard { get; }

        /// <summary>The question about a find with no room is showing.</summary>
        public bool IsAskingAboutGroundOverflow { get; }
    }
}
