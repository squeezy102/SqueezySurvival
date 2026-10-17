using System;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>
    /// Binds NewGameScreen.uxml to a <see cref="NewGameSetup"/>. All run option rules live in Core.
    /// </summary>
    public sealed class NewGameScreenController
    {
        private const int NoChoiceIndex = -1;
        private const int LeftHandChoiceIndex = 0;
        private const int RightHandChoiceIndex = 1;
        private const string StartLabel = "Start";

        // Dims an option that conflicts with a ticked one. It stays clickable; ticking it clears the conflict.
        private const string InConflictClassName = "option-in-conflict";

        private readonly VisualElement _screen;
        private NewGameSetup _setup;
        private readonly RadioButtonGroup _dominantHandChoice;
        private readonly Toggle _casualToggle;
        private readonly Toggle _easyToggle;
        private readonly Toggle _hardToggle;
        private readonly Label _pointsSummary;
        private readonly Label _easyEffect;
        private readonly Button _startButton;

        /// <summary>Binds the screen's controls. Each time the screen is shown it starts from a new setup.</summary>
        public NewGameScreenController(VisualElement screen)
        {
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            _setup = new NewGameSetup();

            _dominantHandChoice = screen.Q<RadioButtonGroup>("dominant-hand-choice");
            _casualToggle = screen.Q<Toggle>("casual-mode-toggle");
            _easyToggle = screen.Q<Toggle>("easy-mode-toggle");
            _hardToggle = screen.Q<Toggle>("hard-mode-toggle");
            _pointsSummary = screen.Q<Label>("run-points-summary");
            _startButton = screen.Q<Button>("new-game-start-button");
            screen.Q<Label>("casual-mode-gameplay").text = RunOptionGameplayEffects.Casual;
            screen.Q<Label>("easy-mode-gameplay").text = RunOptionGameplayEffects.Easy;
            screen.Q<Label>("hard-mode-gameplay").text = RunOptionGameplayEffects.Hard;
            screen.Q<Label>("casual-mode-effect").text = RunOptionPointEffects.Casual;
            _easyEffect = screen.Q<Label>("easy-mode-effect");
            screen.Q<Label>("hard-mode-effect").text = RunOptionPointEffects.Hard;

            _dominantHandChoice.RegisterValueChangedCallback(change => ApplyAndRefresh(() => _setup.DominantHand = ToDominantHand(change.newValue)));
            _casualToggle.RegisterValueChangedCallback(change => ApplyAndRefresh(() => _setup.SetCasual(change.newValue)));
            _easyToggle.RegisterValueChangedCallback(change => ApplyAndRefresh(() => _setup.SetEasy(change.newValue)));
            _hardToggle.RegisterValueChangedCallback(change => ApplyAndRefresh(() => _setup.SetHard(change.newValue)));
            screen.Q<Button>("new-game-back-button").clicked += () => BackRequested?.Invoke();
            _startButton.clicked += () =>
            {
                if (_setup.CanStart)
                {
                    StartRequested?.Invoke(_setup);
                }
            };

            Refresh();
            Hide();
        }

        /// <summary>Raised when the player chooses Back.</summary>
        public event Action BackRequested;

        /// <summary>Raised when the player chooses Start, with the chosen setup.</summary>
        public event Action<NewGameSetup> StartRequested;

        /// <summary>True while the screen is shown.</summary>
        public bool IsVisible => _screen.style.display != DisplayStyle.None;

        /// <summary>The Start button, so focus can return to it after a notice closes.</summary>
        public Button StartButton => _startButton;

        /// <summary>Shows the screen with nothing chosen, and focuses the dominant hand choice.</summary>
        public void Show()
        {
            _setup = new NewGameSetup();
            Refresh();
            _screen.style.display = DisplayStyle.Flex;
            _dominantHandChoice.Focus();
        }

        /// <summary>Hides the screen.</summary>
        public void Hide()
        {
            _screen.style.display = DisplayStyle.None;
        }

        private void ApplyAndRefresh(Action change)
        {
            change();
            Refresh();
        }

        private void Refresh()
        {
            _dominantHandChoice.SetValueWithoutNotify(ToChoiceIndex(_setup.DominantHand));
            _casualToggle.SetValueWithoutNotify(_setup.IsCasual);
            _easyToggle.SetValueWithoutNotify(_setup.IsEasy);
            _hardToggle.SetValueWithoutNotify(_setup.IsHard);
            _casualToggle.EnableInClassList(InConflictClassName, _setup.IsCasualInConflict);
            _easyToggle.EnableInClassList(InConflictClassName, _setup.IsEasyInConflict);
            _hardToggle.EnableInClassList(InConflictClassName, _setup.IsHardInConflict);
            _easyEffect.text = RunOptionPointEffects.ForEasy(_setup.IsCasual);
            _pointsSummary.text = RunPointsSummaryFormatter.Format(_setup.ToRunOptions());
            _startButton.SetEnabled(_setup.CanStart);
            _startButton.text = _setup.CanStart ? StartLabel : _setup.StartUnavailableReason;
        }

        private static DominantHand? ToDominantHand(int choiceIndex)
        {
            switch (choiceIndex)
            {
                case LeftHandChoiceIndex:
                    return DominantHand.Left;
                case RightHandChoiceIndex:
                    return DominantHand.Right;
                default:
                    return null;
            }
        }

        private static int ToChoiceIndex(DominantHand? hand)
        {
            switch (hand)
            {
                case DominantHand.Left:
                    return LeftHandChoiceIndex;
                case DominantHand.Right:
                    return RightHandChoiceIndex;
                default:
                    return NoChoiceIndex;
            }
        }
    }
}
