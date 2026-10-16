using System;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>Names of the elements in MainMenu.uxml that the screen controller binds to.</summary>
    public static class MainMenuElementNames
    {
        /// <summary>Label in the bottom right that shows the game version.</summary>
        public const string VersionLabel = "version-label";

        /// <summary>Overlay shown when the chosen option's screen does not exist yet.</summary>
        public const string NotBuiltNotice = "not-built-notice";

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
