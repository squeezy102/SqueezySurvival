using System;

namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>
    /// Decides what Cancel (Escape) does on the main menu: the first that applies, from the window nearest the
    /// player, which is the notice, then the start confirmation, the New Game screen and Options.
    /// </summary>
    public static class MainMenuCancelResolver
    {
        /// <summary>What Cancel does with <paramref name="windows"/> open.</summary>
        public static MainMenuCancelAction Resolve(MainMenuWindowState windows)
        {
            if (windows == null)
            {
                throw new ArgumentNullException(nameof(windows));
            }

            if (windows.IsNotBuiltNoticeOpen)
            {
                return MainMenuCancelAction.CloseNotBuiltNotice;
            }

            if (windows.IsStartConfirmationOpen)
            {
                return MainMenuCancelAction.GoBackFromStartConfirmation;
            }

            if (windows.IsNewGameScreenOpen)
            {
                return MainMenuCancelAction.ReturnFromNewGameScreen;
            }

            return windows.IsOptionsScreenOpen ? MainMenuCancelAction.CloseOptionsScreen : MainMenuCancelAction.Nothing;
        }
    }
}
