using System;
using System.Collections.Generic;
using SqueezySurvival.Core.MainMenu;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Saving;
using SqueezySurvival.Presentation.Options;
using SqueezySurvival.Presentation.Session;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>
    /// Binds the main menu's elements to the menu logic in Core and switches between the menu, the New Game screen,
    /// the Options screen and the not-built notice. Holds no menu rules itself. Built from a root element and injected
    /// collaborators, so it is tested without a scene; <see cref="MainMenuScreenController"/> supplies Unity's.
    /// </summary>
    public sealed class MainMenuScreenPresenter
    {
        private readonly MainMenuCommandRouter _commandRouter;
        private readonly RunLaunchHandoff _runLaunchHandoff;
        private readonly IRunInProgressLoader _runInProgressLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly IApplicationQuitter _applicationQuitter;
        private readonly VisualElement _menuOptions;
        private readonly NewGameScreenController _newGameScreen;
        private readonly VisualElement _startConfirmation;
        private readonly Label _startConfirmationMessage;
        private readonly Button _startConfirmationCancelButton;
        private readonly VisualElement _notBuiltNotice;
        private readonly Button _notBuiltNoticeCloseButton;
        private readonly OptionsScreenController _optionsScreen;
        private NewGameSetup _setupAwaitingConfirmation;
        private Button _newGameButton;
        private Focusable _focusAfterNotice;
        private Button _optionsButton;

        /// <summary>Fills in the menu in <paramref name="root"/> and connects its controls.</summary>
        public MainMenuScreenPresenter(
            VisualElement root,
            MainMenuOptionStateEvaluator optionStateEvaluator,
            MainMenuCommandRouter commandRouter,
            IRunInProgressLoader runInProgressLoader,
            RunLaunchHandoff runLaunchHandoff,
            ISceneLoader sceneLoader,
            IApplicationQuitter applicationQuitter,
            string versionLabelText)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (optionStateEvaluator == null)
            {
                throw new ArgumentNullException(nameof(optionStateEvaluator));
            }

            _commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
            _runLaunchHandoff = runLaunchHandoff ?? throw new ArgumentNullException(nameof(runLaunchHandoff));
            _runInProgressLoader = runInProgressLoader ?? throw new ArgumentNullException(nameof(runInProgressLoader));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _applicationQuitter = applicationQuitter ?? throw new ArgumentNullException(nameof(applicationQuitter));

            root.Q<Label>(MainMenuElementNames.VersionLabel).text = versionLabelText;
            _menuOptions = root.Q<VisualElement>(MainMenuElementNames.MenuOptions);

            _notBuiltNotice = root.Q<VisualElement>(MainMenuElementNames.NotBuiltNotice);
            _notBuiltNoticeCloseButton = root.Q<Button>(MainMenuElementNames.NotBuiltNoticeCloseButton);
            _notBuiltNoticeCloseButton.clicked += HideNotBuiltNotice;
            _notBuiltNotice.style.display = DisplayStyle.None;

            _newGameScreen = new NewGameScreenController(root.Q<VisualElement>(MainMenuElementNames.NewGameScreen));
            _newGameScreen.BackRequested += ReturnToMenu;
            _newGameScreen.StartRequested += ShowStartConfirmation;

            _startConfirmation = root.Q<VisualElement>(MainMenuElementNames.StartConfirmation);
            _startConfirmationMessage = root.Q<Label>(MainMenuElementNames.StartConfirmationMessage);
            _startConfirmationCancelButton = root.Q<Button>(MainMenuElementNames.StartConfirmationCancelButton);
            _startConfirmationCancelButton.clicked += HideStartConfirmation;
            root.Q<Button>(MainMenuElementNames.StartConfirmationConfirmButton).clicked += ConfirmStart;
            _startConfirmation.style.display = DisplayStyle.None;

            _optionsScreen = new OptionsScreenController(root.Q<VisualElement>(MainMenuElementNames.OptionsScreen));
            _optionsScreen.Closed += HideOptionsScreen;

            root.RegisterCallback<NavigationCancelEvent>(_ => OnCancel());

            var availableButtons = new List<Button>();
            foreach (MainMenuOptionState state in optionStateEvaluator.EvaluateAll())
            {
                Button button = BindOption(root, state);
                if (state.IsAvailable)
                {
                    availableButtons.Add(button);
                }
            }

            // Focus must be requested a frame after layout for UI Toolkit to apply it
            // (Unity Manual: runtime UI event system FAQ).
            if (availableButtons.Count > 0)
            {
                root.schedule.Execute(() => availableButtons[0].Focus());
            }
        }

        private Button BindOption(VisualElement root, MainMenuOptionState state)
        {
            Button button = root.Q<Button>(MainMenuElementNames.OptionButton(state.Option));
            button.text = state.DisplayText;
            button.SetEnabled(state.IsAvailable);

            if (state.Option == MainMenuOption.NewGame)
            {
                _newGameButton = button;
            }

            MainMenuOption option = state.Option;
            button.clicked += () => OnOptionChosen(option, button);
            return button;
        }

        private void OnOptionChosen(MainMenuOption option, Button button)
        {
            switch (_commandRouter.Route(option))
            {
                case MainMenuCommandOutcome.ContinueRun:
                    if (_runInProgressLoader.TryLoadRunInProgress(out NewRunConfiguration configuration))
                    {
                        _runLaunchHandoff.Hand(configuration);
                        _sceneLoader.Load(SceneNames.Game);
                    }

                    break;
                case MainMenuCommandOutcome.ShowNewGameScreen:
                    _menuOptions.style.display = DisplayStyle.None;
                    _newGameScreen.Show();
                    break;
                case MainMenuCommandOutcome.ShowOptionsScreen:
                    _optionsButton = button;
                    _menuOptions.style.display = DisplayStyle.None;
                    _optionsScreen.Show();
                    break;
                case MainMenuCommandOutcome.ShowNotBuiltNotice:
                    ShowNotBuiltNotice(button);
                    break;
                case MainMenuCommandOutcome.QuitApplication:
                    _applicationQuitter.Quit();
                    break;
                case MainMenuCommandOutcome.IgnoreUnavailableOption:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, "Unhandled main menu outcome.");
            }
        }

        private void OnCancel()
        {
            var windows = new MainMenuWindowState(
                IsShowing(_notBuiltNotice), IsShowing(_startConfirmation), _newGameScreen.IsVisible, _optionsScreen.IsVisible);
            switch (MainMenuCancelResolver.Resolve(windows))
            {
                case MainMenuCancelAction.CloseNotBuiltNotice:
                    HideNotBuiltNotice();
                    break;
                case MainMenuCancelAction.GoBackFromStartConfirmation:
                    HideStartConfirmation();
                    break;
                case MainMenuCancelAction.ReturnFromNewGameScreen:
                    ReturnToMenu();
                    break;
                case MainMenuCancelAction.CloseOptionsScreen:
                    HideOptionsScreen();
                    break;
                case MainMenuCancelAction.Nothing:
                    break;
                default:
                    throw new InvalidOperationException("Unknown main menu cancel action.");
            }
        }

        private static bool IsShowing(VisualElement element) => element.style.display != DisplayStyle.None;

        private void HideOptionsScreen()
        {
            _optionsScreen.Hide();
            _menuOptions.style.display = DisplayStyle.Flex;
            _optionsButton?.Focus();
        }

        private void ReturnToMenu()
        {
            _newGameScreen.Hide();
            _menuOptions.style.display = DisplayStyle.Flex;
            _newGameButton.Focus();
        }

        private void ShowStartConfirmation(NewGameSetup setup)
        {
            _setupAwaitingConfirmation = setup;
            _startConfirmationMessage.text = NewGameConfirmationMessageFormatter.Format(setup.ToRunOptions());
            _startConfirmation.style.display = DisplayStyle.Flex;
            _startConfirmationCancelButton.Focus();
        }

        private void HideStartConfirmation()
        {
            _startConfirmation.style.display = DisplayStyle.None;
            _newGameScreen.StartButton.Focus();
        }

        // The run's choices are recorded only here, on confirmation (Docs/Design/UI/StartupFlow.md, Starting the run).
        private void ConfirmStart()
        {
            _startConfirmation.style.display = DisplayStyle.None;
            _runLaunchHandoff.Hand(_setupAwaitingConfirmation.CreateRunConfiguration());
            _sceneLoader.Load(SceneNames.Game);
        }

        private void ShowNotBuiltNotice(Focusable focusAfterClose)
        {
            _focusAfterNotice = focusAfterClose;
            _notBuiltNotice.style.display = DisplayStyle.Flex;
            _notBuiltNoticeCloseButton.Focus();
        }

        private void HideNotBuiltNotice()
        {
            _notBuiltNotice.style.display = DisplayStyle.None;
            _focusAfterNotice?.Focus();
        }
    }
}
