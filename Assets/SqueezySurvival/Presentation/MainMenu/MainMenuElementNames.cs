using System;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>Names of the elements in MainMenu.uxml that the screen controller binds to.</summary>
    public static class MainMenuElementNames
    {
        /// <summary>Panel holding the menu option buttons.</summary>
        public const string MenuOptions = "menu-options";

        /// <summary>Instance of NewGameScreen.uxml.</summary>
        public const string NewGameScreen = "new-game-screen";

        /// <summary>Label in the bottom right that shows the game version.</summary>
        public const string VersionLabel = "version-label";

        /// <summary>Overlay shown when the chosen option's screen does not exist yet.</summary>
        public const string NotBuiltNotice = "not-built-notice";

        /// <summary>Overlay asking the player to confirm their New Game choices.</summary>
        public const string StartConfirmation = "start-confirmation";

        /// <summary>Text of the start confirmation.</summary>
        public const string StartConfirmationMessage = "start-confirmation-message";

        /// <summary>Button that confirms and starts the run.</summary>
        public const string StartConfirmationConfirmButton = "start-confirmation-confirm-button";

        /// <summary>Button that returns to the New Game screen.</summary>
        public const string StartConfirmationCancelButton = "start-confirmation-cancel-button";

        /// <summary>Optional second line of the not-built notice.</summary>
        public const string NotBuiltNoticeDetail = "not-built-notice-detail";

        /// <summary>Button that closes the not-built notice.</summary>
        public const string NotBuiltNoticeCloseButton = "not-built-notice-close-button";

        /// <summary>Name of the button for a menu option.</summary>
        public static string OptionButton(MainMenuOption option) => OptionNamePrefix(option) + "-button";

        private static string OptionNamePrefix(MainMenuOption option)
        {
            switch (option)
            {
                case MainMenuOption.NewGame:
                    return "new-game";
                case MainMenuOption.Continue:
                    return "continue";
                case MainMenuOption.LoadGame:
                    return "load-game";
                case MainMenuOption.Options:
                    return "options";
                case MainMenuOption.Quit:
                    return "quit";
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, "Unknown main menu option.");
            }
        }
    }
}
