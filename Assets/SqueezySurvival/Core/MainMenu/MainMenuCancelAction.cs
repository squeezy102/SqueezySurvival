namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>What Cancel (Escape) does on the main menu (Docs/Design/UI/StartupFlow.md).</summary>
    public enum MainMenuCancelAction
    {
        /// <summary>Nothing is open to cancel.</summary>
        Nothing,

        /// <summary>Close the notice that a window is not built yet.</summary>
        CloseNotBuiltNotice,

        /// <summary>Go back from the confirmation before a run starts, to the New Game screen.</summary>
        GoBackFromStartConfirmation,

        /// <summary>Leave the New Game screen for the menu.</summary>
        ReturnFromNewGameScreen,

        /// <summary>Close Options and return to the menu.</summary>
        CloseOptionsScreen,
    }
}
