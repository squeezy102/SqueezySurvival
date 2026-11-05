using NUnit.Framework;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Presentation.MainMenu;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.MainMenu
{
    /// <summary>
    /// The New Game screen (Docs/Design/UI/StartupFlow.md). Core's setup decides what can start and which options
    /// conflict; these tests check the screen shows it and reports the player's choices.
    /// </summary>
    [TestFixture]
    public sealed class NewGameScreenControllerTests : MainMenuUITestFixture
    {
        private const int LeftHandChoice = 0;
        private const int RightHandChoice = 1;
        private const string ConflictClass = "option-in-conflict";

        private NewGameScreenController _screen;

        [SetUp]
        public void CreateScreen()
        {
            _screen = new NewGameScreenController(Element<VisualElement>("new-game-screen"));
            _screen.Show();
            simulate.FrameUpdate();
        }

        private Button StartButton => Element<Button>("new-game-start-button");

        private RadioButtonGroup HandChoice => Element<RadioButtonGroup>("dominant-hand-choice");

        [Test]
        public void New_IsHiddenUntilShown()
        {
            var hiddenScreen = new NewGameScreenController(Element<VisualElement>("new-game-screen"));

            Assert.That(hiddenScreen.IsVisible, Is.False);
        }

        [Test]
        public void Show_ShowsTheScreenWithStartUnavailableUntilAHandIsChosen()
        {
            Assert.That(_screen.IsVisible, Is.True);
            Assert.That(StartButton.enabledSelf, Is.False);
            Assert.That(StartButton.text, Is.EqualTo(NewGameSetup.ChooseDominantHandReason));
        }

        [Test]
        public void ChoosingAHand_EnablesStart()
        {
            HandChoice.value = RightHandChoice;

            Assert.That(StartButton.enabledSelf, Is.True);
            Assert.That(StartButton.text, Is.EqualTo("Start"));
        }

        [Test]
        public void StartButton_Click_ReportsTheSetupWithTheChosenHandAndOptions()
        {
            NewGameSetup requested = null;
            _screen.StartRequested += setup => requested = setup;
            HandChoice.value = LeftHandChoice;
            Element<Toggle>("casual-mode-toggle").value = true;

            simulate.Click(StartButton);
            simulate.FrameUpdate();

            Assert.That(requested, Is.Not.Null, "Start did not report a setup.");
            Assert.That(requested.DominantHand, Is.EqualTo(DominantHand.Left));
            Assert.That(requested.IsCasual, Is.True);
        }

        [Test]
        public void BackButton_Click_ReportsBack()
        {
            int backs = 0;
            _screen.BackRequested += () => backs++;

            simulate.Click(Element<Button>("new-game-back-button"));
            simulate.FrameUpdate();

            Assert.That(backs, Is.EqualTo(1));
        }

        [Test]
        public void TickingHard_ShowsCasualAndEasyInConflictAndUpdatesThePointsSummary()
        {
            Element<Toggle>("hard-mode-toggle").value = true;

            Assert.That(Element<Toggle>("casual-mode-toggle").ClassListContains(ConflictClass), Is.True);
            Assert.That(Element<Toggle>("easy-mode-toggle").ClassListContains(ConflictClass), Is.True);
            Assert.That(Element<Toggle>("hard-mode-toggle").ClassListContains(ConflictClass), Is.False);
            Assert.That(Element<Label>("run-points-summary").text, Is.EqualTo(RunPointsSummaryFormatter.Format(new RunOptions(false, false, true))));
        }

        [Test]
        public void TickingCasual_ChangesWhatEasyModeDoesToPoints()
        {
            Element<Toggle>("casual-mode-toggle").value = true;

            Assert.That(Element<Label>("easy-mode-effect").text, Is.EqualTo(RunOptionPointEffects.ForEasy(true)));
        }

        [Test]
        public void Show_AgainAfterChoosing_StartsFromTheDefaults()
        {
            HandChoice.value = RightHandChoice;
            Element<Toggle>("casual-mode-toggle").value = true;
            _screen.Hide();

            _screen.Show();

            Assert.That(HandChoice.value, Is.EqualTo(-1));
            Assert.That(Element<Toggle>("casual-mode-toggle").value, Is.False);
            Assert.That(StartButton.enabledSelf, Is.False);
        }
    }
}
