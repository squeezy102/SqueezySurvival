using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Saving;

namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>
    /// Decides the label and availability of each main menu option (Docs/Design/UI/StartupFlow.md, Menu options).
    /// </summary>
    public sealed class MainMenuOptionStateEvaluator
    {
        /// <summary>Shown under Continue when no run is in progress.</summary>
        public const string NoGameInProgressReason = "No game in progress";

        /// <summary>Shown under Load Game when nothing can be loaded.</summary>
        public const string NoSavedGamesReason = "No saved games";

        private static readonly MainMenuOption[] OptionsInDisplayOrder =
        {
            MainMenuOption.NewGame,
            MainMenuOption.Continue,
            MainMenuOption.LoadGame,
            MainMenuOption.Options,
            MainMenuOption.Quit,
        };

        private readonly ISavedGameAvailability _savedGameAvailability;

        /// <summary>Creates an evaluator that reads saved game availability from the given source.</summary>
        public MainMenuOptionStateEvaluator(ISavedGameAvailability savedGameAvailability)
        {
            _savedGameAvailability = savedGameAvailability ?? throw new ArgumentNullException(nameof(savedGameAvailability));
        }

        /// <summary>Returns the state of every option, in display order.</summary>
        public IReadOnlyList<MainMenuOptionState> EvaluateAll()
        {
            var states = new List<MainMenuOptionState>(OptionsInDisplayOrder.Length);
            foreach (MainMenuOption option in OptionsInDisplayOrder)
            {
                states.Add(Evaluate(option));
            }

            return states;
        }

        /// <summary>Returns the current state of one option.</summary>
        public MainMenuOptionState Evaluate(MainMenuOption option)
        {
            switch (option)
            {
                case MainMenuOption.NewGame:
                    return new MainMenuOptionState(option, "New Game", string.Empty);
                case MainMenuOption.Continue:
                    return new MainMenuOptionState(
                        option,
                        "Continue",
                        _savedGameAvailability.HasRunInProgress ? string.Empty : NoGameInProgressReason);
                case MainMenuOption.LoadGame:
                    return new MainMenuOptionState(
                        option,
                        "Load Game",
                        _savedGameAvailability.HasLoadableSave ? string.Empty : NoSavedGamesReason);
                case MainMenuOption.Options:
                    return new MainMenuOptionState(option, "Options", string.Empty);
                case MainMenuOption.Quit:
                    return new MainMenuOptionState(option, "Quit", string.Empty);
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, "Unknown main menu option.");
            }
        }
    }
}
