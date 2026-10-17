namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>What the main menu screen does when the player chooses an option.</summary>
    public enum MainMenuCommandOutcome
    {
        /// <summary>The option is unavailable; nothing happens.</summary>
        IgnoreUnavailableOption,

        /// <summary>Open the New Game screen.</summary>
        ShowNewGameScreen,

        /// <summary>The option's screen does not exist yet; show the "This isn't built yet." notice.</summary>
        ShowNotBuiltNotice,

        /// <summary>Close the game.</summary>
        QuitApplication,
    }
}
