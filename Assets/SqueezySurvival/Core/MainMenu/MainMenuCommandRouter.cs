using System;

namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>
    /// Maps a chosen main menu option to what the screen should do. At the wireframe stage every screen
    /// behind the menu is unbuilt (Docs/Design/UI/StartupFlow.md, Wireframe stage).
    /// </summary>
    public sealed class MainMenuCommandRouter
    {
        private readonly MainMenuOptionStateEvaluator _optionStateEvaluator;

        /// <summary>Creates a router that checks availability with the given evaluator.</summary>
        public MainMenuCommandRouter(MainMenuOptionStateEvaluator optionStateEvaluator)
        {
            _optionStateEvaluator = optionStateEvaluator ?? throw new ArgumentNullException(nameof(optionStateEvaluator));
        }

        /// <summary>Returns what the screen should do for the chosen option.</summary>
        public MainMenuCommandOutcome Route(MainMenuOption option)
        {
            if (!_optionStateEvaluator.Evaluate(option).IsAvailable)
            {
                return MainMenuCommandOutcome.IgnoreUnavailableOption;
            }

            return option == MainMenuOption.Quit
                ? MainMenuCommandOutcome.QuitApplication
                : MainMenuCommandOutcome.ShowNotBuiltNotice;
        }
    }
}
