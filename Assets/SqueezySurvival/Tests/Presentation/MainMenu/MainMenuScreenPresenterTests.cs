using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.MainMenu;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Presentation.MainMenu;
using SqueezySurvival.Presentation.Session;
using SqueezySurvival.Presentation.Tests.Session;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.MainMenu
{
    /// <summary>
    /// The main menu (Docs/Design/UI/StartupFlow.md): which window shows, where focus goes, and what leaves the menu.
    /// Core decides which options are available, what each does and what Cancel does; these tests check the screen
    /// shows the result and carries it out.
    /// </summary>
    [TestFixture]
    public sealed class MainMenuScreenPresenterTests : MainMenuUITestFixture
    {
        private const string VersionLabel = "Version test";
        private const int RightHandChoice = 1;

        private RunLaunchHandoff _handoff;
        private RecordingSceneLoader _sceneLoader;
        private RecordingApplicationQuitter _quitter;
        private MainMenuOptionStateEvaluator _evaluator;

        private Focusable FocusedElement => rootVisualElement.panel.focusController.focusedElement;

        private void ComposeMenu(bool hasRunInProgress, bool hasLoadableSave)
        {
            var savedGames = new FixedSavedGames(hasRunInProgress, hasLoadableSave);
            _evaluator = new MainMenuOptionStateEvaluator(savedGames);
            _handoff = new RunLaunchHandoff();
            _sceneLoader = new RecordingSceneLoader();
            _quitter = new RecordingApplicationQuitter();
            new MainMenuScreenPresenter(
                rootVisualElement,
                _evaluator,
                new MainMenuCommandRouter(_evaluator),
                savedGames,
                _handoff,
                _sceneLoader,
                _quitter,
                VersionLabel);
            simulate.FrameUpdate();
        }

        private bool IsShowing(string elementName) => Element<VisualElement>(elementName).style.display != DisplayStyle.None;

        private void Click(string buttonName)
        {
            simulate.Click(Element<Button>(buttonName));
            simulate.FrameUpdate();
        }

        private void PressCancel()
        {
            using (NavigationCancelEvent cancel = NavigationCancelEvent.GetPooled())
            {
                cancel.target = rootVisualElement;
                rootVisualElement.SendEvent(cancel);
            }

            simulate.FrameUpdate();
        }

        private void OpenNewGameAndChooseAHand()
        {
            Click("new-game-button");
            Element<RadioButtonGroup>("dominant-hand-choice").value = RightHandChoice;
        }

        private void OpenTheStartConfirmation()
        {
            OpenNewGameAndChooseAHand();
            Click("new-game-start-button");
        }

        [Test]
        public void New_ShowsTheVersionAndEveryOptionAsCoreStatesIt()
        {
            ComposeMenu(false, false);

            Assert.That(Element<Label>("version-label").text, Is.EqualTo(VersionLabel));
            foreach (MainMenuOptionState state in _evaluator.EvaluateAll())
            {
                Button button = Element<Button>(MainMenuElementNames.OptionButton(state.Option));
                Assert.That(button.text, Is.EqualTo(state.DisplayText), state.Option.ToString());
                Assert.That(button.enabledSelf, Is.EqualTo(state.IsAvailable), state.Option.ToString());
            }
        }

        [Test]
        public void New_ShowsTheMenuAndNoOtherWindow()
        {
            ComposeMenu(false, false);

            Assert.That(IsShowing("menu-options"), Is.True);
            Assert.That(IsShowing("new-game-screen"), Is.False);
            Assert.That(IsShowing("options-screen"), Is.False);
            Assert.That(IsShowing("start-confirmation"), Is.False);
            Assert.That(IsShowing("not-built-notice"), Is.False);
        }

        [Test]
        public void New_FocusesTheFirstAvailableOption()
        {
            ComposeMenu(true, true);
            simulate.FrameUpdate();

            Assert.That(FocusedElement, Is.SameAs(Element<Button>("new-game-button")));
        }

        [Test]
        public void NewGame_Click_ShowsTheNewGameScreenInPlaceOfTheMenu()
        {
            ComposeMenu(false, false);

            Click("new-game-button");

            Assert.That(IsShowing("new-game-screen"), Is.True);
            Assert.That(IsShowing("menu-options"), Is.False);
        }

        [Test]
        public void NewGameBack_Click_ReturnsToTheMenuAndFocusesNewGame()
        {
            ComposeMenu(false, false);
            Click("new-game-button");

            Click("new-game-back-button");

            Assert.That(IsShowing("new-game-screen"), Is.False);
            Assert.That(IsShowing("menu-options"), Is.True);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("new-game-button")));
        }

        [Test]
        public void Start_WithAHandChosen_AsksToConfirmWithoutLeavingTheMenu()
        {
            ComposeMenu(false, false);

            OpenTheStartConfirmation();

            Assert.That(IsShowing("start-confirmation"), Is.True);
            Assert.That(Element<Label>("start-confirmation-message").text, Is.EqualTo(NewGameConfirmationMessageFormatter.Format(RunOptions.Standard)));
            Assert.That(_sceneLoader.Loaded, Is.Empty);
            Assert.That(_handoff.HasPendingRun, Is.False);
        }

        [Test]
        public void StartConfirmationGoBack_Click_ClosesItAndFocusesStart()
        {
            ComposeMenu(false, false);
            OpenTheStartConfirmation();

            Click("start-confirmation-cancel-button");

            Assert.That(IsShowing("start-confirmation"), Is.False);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("new-game-start-button")));
            Assert.That(_handoff.HasPendingRun, Is.False);
        }

        [Test]
        public void StartConfirmationStart_Click_HandsTheRunOverAndLoadsTheGame()
        {
            ComposeMenu(false, false);
            OpenTheStartConfirmation();

            Click("start-confirmation-confirm-button");

            Assert.That(_handoff.TryTake(out NewRunConfiguration configuration), Is.True);
            Assert.That(configuration.DominantHand, Is.EqualTo(DominantHand.Right));
            Assert.That(_sceneLoader.Loaded, Is.EqualTo(new[] { SceneNames.Game }));
        }

        [Test]
        public void Continue_Click_HandsTheRunInProgressOverAndLoadsTheGame()
        {
            ComposeMenu(true, false);

            Click("continue-button");

            Assert.That(_handoff.HasPendingRun, Is.True);
            Assert.That(_sceneLoader.Loaded, Is.EqualTo(new[] { SceneNames.Game }));
        }

        [Test]
        public void Options_Click_ShowsOptionsInPlaceOfTheMenuAndBackReturnsAndFocusesOptions()
        {
            ComposeMenu(false, false);

            Click("options-button");
            Assert.That(IsShowing("options-screen"), Is.True);
            Assert.That(IsShowing("menu-options"), Is.False);

            Click("options-back-button");

            Assert.That(IsShowing("options-screen"), Is.False);
            Assert.That(IsShowing("menu-options"), Is.True);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("options-button")));
        }

        [Test]
        public void LoadGame_Click_ShowsTheNotBuiltNoticeAndClosingItFocusesLoadGame()
        {
            ComposeMenu(false, true);

            Click("load-game-button");
            Assert.That(IsShowing("not-built-notice"), Is.True);

            Click("not-built-notice-close-button");

            Assert.That(IsShowing("not-built-notice"), Is.False);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("load-game-button")));
        }

        [Test]
        public void Quit_Click_QuitsTheApplication()
        {
            ComposeMenu(false, false);

            Click("quit-button");

            Assert.That(_quitter.QuitCount, Is.EqualTo(1));
        }

        [Test]
        public void Cancel_WithNothingOpen_ChangesNothing()
        {
            ComposeMenu(false, false);

            PressCancel();

            Assert.That(IsShowing("menu-options"), Is.True);
            Assert.That(new[] { "new-game-screen", "options-screen", "start-confirmation", "not-built-notice" }.Any(IsShowing), Is.False);
        }

        [Test]
        public void Cancel_OnTheNewGameScreen_ReturnsToTheMenu()
        {
            ComposeMenu(false, false);
            Click("new-game-button");

            PressCancel();

            Assert.That(IsShowing("new-game-screen"), Is.False);
            Assert.That(IsShowing("menu-options"), Is.True);
        }

        [Test]
        public void Cancel_OnTheStartConfirmation_GoesBackToTheNewGameScreenOnly()
        {
            ComposeMenu(false, false);
            OpenTheStartConfirmation();

            PressCancel();

            Assert.That(IsShowing("start-confirmation"), Is.False);
            Assert.That(IsShowing("new-game-screen"), Is.True);
        }

        [Test]
        public void Cancel_OnOptions_ClosesOptions()
        {
            ComposeMenu(false, false);
            Click("options-button");

            PressCancel();

            Assert.That(IsShowing("options-screen"), Is.False);
            Assert.That(IsShowing("menu-options"), Is.True);
        }

        [Test]
        public void Cancel_OnTheNotBuiltNotice_ClosesTheNotice()
        {
            ComposeMenu(false, true);
            Click("load-game-button");

            PressCancel();

            Assert.That(IsShowing("not-built-notice"), Is.False);
            Assert.That(IsShowing("menu-options"), Is.True);
        }
    }
}
