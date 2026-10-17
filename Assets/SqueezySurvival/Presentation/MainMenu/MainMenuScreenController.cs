using System;
using System.Collections.Generic;
using SqueezySurvival.Core.MainMenu;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>
    /// Binds the main menu's UI Toolkit document to the menu logic in Core and switches between the menu,
    /// the New Game screen and the not-built notice. Holds no menu rules itself.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuScreenController : MonoBehaviour
    {
        private MainMenuCommandRouter _commandRouter;
        private RunLaunchHandoff _runLaunchHandoff;
        private NewGameSetup _setupAwaitingConfirmation;
        private Label _notBuiltNoticeDetail;
        private VisualElement _menuOptions;
        private NewGameScreenController _newGameScreen;
        private VisualElement _startConfirmation;
        private Label _startConfirmationMessage;
        private Button _startConfirmationCancelButton;
        private VisualElement _notBuiltNotice;
        private Button _notBuiltNoticeCloseButton;
        private Button _newGameButton;
        private Focusable _focusAfterNotice;

        /// <summary>Fills in the menu and connects its controls. Called once by the scene's composition root.</summary>
        public void Bind(
            MainMenuOptionStateEvaluator optionStateEvaluator,
            MainMenuCommandRouter commandRouter,
            RunLaunchHandoff runLaunchHandoff,
            string versionLabelText)
        {
            if (optionStateEvaluator == null)
            {
                throw new ArgumentNullException(nameof(optionStateEvaluator));
            }

            _commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
            _runLaunchHandoff = runLaunchHandoff ?? throw new ArgumentNullException(nameof(runLaunchHandoff));

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Label>(MainMenuElementNames.VersionLabel).text = versionLabelText;
            _menuOptions = root.Q<VisualElement>(MainMenuElementNames.MenuOptions);

            _notBuiltNotice = root.Q<VisualElement>(MainMenuElementNames.NotBuiltNotice);
            _notBuiltNoticeCloseButton = root.Q<Button>(MainMenuElementNames.NotBuiltNoticeCloseButton);
            _notBuiltNoticeDetail = root.Q<Label>(MainMenuElementNames.NotBuiltNoticeDetail);
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
                case MainMenuCommandOutcome.ShowNewGameScreen:
                    _menuOptions.style.display = DisplayStyle.None;
                    _newGameScreen.Show();
                    break;
                case MainMenuCommandOutcome.ShowNotBuiltNotice:
                    ShowNotBuiltNotice(button);
                    break;
                case MainMenuCommandOutcome.QuitApplication:
                    // Application.Quit is ignored in the Unity Editor; it closes the built player.
                    Application.Quit();
                    break;
                case MainMenuCommandOutcome.IgnoreUnavailableOption:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, "Unhandled main menu outcome.");
            }
        }

        private void OnCancel()
        {
            if (_notBuiltNotice.style.display != DisplayStyle.None)
            {
                HideNotBuiltNotice();
            }
            else if (_startConfirmation.style.display != DisplayStyle.None)
            {
                HideStartConfirmation();
            }
            else if (_newGameScreen.IsVisible)
            {
                ReturnToMenu();
            }
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

        // The run's configuration is handed off as it will be once the game scene exists. Until then the notice
        // shows what was handed off (Docs/Design/UI/StartupFlow.md, Wireframe stage).
        private void ConfirmStart()
        {
            _startConfirmation.style.display = DisplayStyle.None;
            NewRunConfiguration configuration = _setupAwaitingConfirmation.CreateRunConfiguration();
            _runLaunchHandoff.Hand(configuration);
            ShowNotBuiltNotice(_newGameScreen.StartButton, "Run configured: " + NewRunConfigurationSummaryFormatter.Format(configuration));
        }

        private void ShowNotBuiltNotice(Focusable focusAfterClose, string detail = "")
        {
            _focusAfterNotice = focusAfterClose;
            _notBuiltNoticeDetail.text = detail;
            _notBuiltNoticeDetail.style.display = detail.Length == 0 ? DisplayStyle.None : DisplayStyle.Flex;
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
