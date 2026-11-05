namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>Which windows are open over the main menu, as Cancel needs to know it. Immutable.</summary>
    public sealed class MainMenuWindowState
    {
        /// <summary>Describes the open windows.</summary>
        public MainMenuWindowState(bool isNotBuiltNoticeOpen, bool isStartConfirmationOpen, bool isNewGameScreenOpen, bool isOptionsScreenOpen)
        {
            IsNotBuiltNoticeOpen = isNotBuiltNoticeOpen;
            IsStartConfirmationOpen = isStartConfirmationOpen;
            IsNewGameScreenOpen = isNewGameScreenOpen;
            IsOptionsScreenOpen = isOptionsScreenOpen;
        }

        /// <summary>The notice that a window is not built yet is showing.</summary>
        public bool IsNotBuiltNoticeOpen { get; }

        /// <summary>The confirmation before a run starts is showing.</summary>
        public bool IsStartConfirmationOpen { get; }

        /// <summary>The New Game screen is showing.</summary>
        public bool IsNewGameScreenOpen { get; }

        /// <summary>The Options screen is showing.</summary>
        public bool IsOptionsScreenOpen { get; }
    }
}
