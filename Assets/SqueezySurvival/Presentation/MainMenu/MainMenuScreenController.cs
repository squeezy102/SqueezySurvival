using System;
using System.Collections.Generic;
using SqueezySurvival.Core.MainMenu;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>
    /// Binds the main menu's UI Toolkit document to the menu logic in Core. Holds no menu rules itself.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuScreenController : MonoBehaviour
    {
        private MainMenuCommandRouter _commandRouter;
        private VisualElement _notBuiltNotice;
        private Button _notBuiltNoticeCloseButton;
        private Button _lastChosenButton;

        /// <summary>Fills in the menu and connects its buttons. Called once by the scene's composition root.</summary>
        public void Bind(MainMenuOptionStateEvaluator optionStateEvaluator, MainMenuCommandRouter commandRouter, string versionLabelText)
        {
            if (optionStateEvaluator == null)
            {
                throw new ArgumentNullException(nameof(optionStateEvaluator));
            }

            _commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Label>(MainMenuElementNames.VersionLabel).text = versionLabelText;

            _notBuiltNotice = root.Q<VisualElement>(MainMenuElementNames.NotBuiltNotice);
            _notBuiltNoticeCloseButton = root.Q<Button>(MainMenuElementNames.NotBuiltNoticeCloseButton);
            _notBuiltNoticeCloseButton.clicked += HideNotBuiltNotice;
            root.RegisterCallback<NavigationCancelEvent>(_ => HideNotBuiltNotice());
            HideNotBuiltNotice();

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

            MainMenuOption option = state.Option;
            button.clicked += () => OnOptionChosen(option, button);
            return button;
        }

        private void OnOptionChosen(MainMenuOption option, Button button)
        {
            switch (_commandRouter.Route(option))
            {
                case MainMenuCommandOutcome.ShowNotBuiltNotice:
                    _lastChosenButton = button;
                    _notBuiltNotice.style.display = DisplayStyle.Flex;
                    _notBuiltNoticeCloseButton.Focus();
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

        private void HideNotBuiltNotice()
        {
            _notBuiltNotice.style.display = DisplayStyle.None;
            _lastChosenButton?.Focus();
        }
    }
}
