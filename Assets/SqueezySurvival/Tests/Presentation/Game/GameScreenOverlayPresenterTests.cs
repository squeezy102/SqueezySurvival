using NUnit.Framework;
using SqueezySurvival.Core.GameScreen;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.Session;
using SqueezySurvival.Presentation.Tests.Session;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// Shows the top window over the game screen and returns focus when it closes (Docs/Design/UI/ScreenLayout.md).
    /// Core's stack decides which window is on top; these tests check the screen shows it and moves focus.
    /// </summary>
    [TestFixture]
    public sealed class GameScreenOverlayPresenterTests : GameScreenUITestFixture
    {
        private static readonly NavigationBarEntry UnbuiltWindow = NavigationBarEntries.GameplayWindows[0];

        private GameScreenOverlayStack _overlays;
        private GameScreenOverlayPresenter _presenter;
        private int _gameFocusRequests;
        private RecordingSceneLoader _sceneLoader;

        [SetUp]
        public void CreatePresenter()
        {
            _overlays = new GameScreenOverlayStack();
            _gameFocusRequests = 0;
            _sceneLoader = new RecordingSceneLoader();
            _presenter = new GameScreenOverlayPresenter(rootVisualElement, _overlays, () => _gameFocusRequests++, _sceneLoader);
        }

        [TearDown]
        public void DisposePresenter() => _presenter.Dispose();

        private bool IsShowing(string elementName) => Element<VisualElement>(elementName).style.display == DisplayStyle.Flex;

        private Focusable FocusedElement => rootVisualElement.panel.focusController.focusedElement;

        [Test]
        public void New_ShowsNoWindow()
        {
            Assert.That(IsShowing("pause-menu"), Is.False);
            Assert.That(IsShowing("not-built-notice"), Is.False);
            Assert.That(IsShowing("options-screen"), Is.False);
        }

        [Test]
        public void OpeningThePauseMenu_ShowsItAndFocusesItsFirstButton()
        {
            _overlays.Open(GameScreenOverlay.PauseMenu);

            Assert.That(IsShowing("pause-menu"), Is.True);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("pause-options-button")));
        }

        [Test]
        public void PauseMenuOptionsButton_Click_ShowsOptionsInPlaceOfThePauseMenu()
        {
            _overlays.Open(GameScreenOverlay.PauseMenu);
            simulate.FrameUpdate();

            simulate.Click(Element<Button>("pause-options-button"));
            simulate.FrameUpdate();

            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(IsShowing("options-screen"), Is.True);
            Assert.That(IsShowing("pause-menu"), Is.False);
        }

        [Test]
        public void OptionsBack_ReturnsToThePauseMenuAndFocusesTheButtonThatOpenedOptions()
        {
            _overlays.Open(GameScreenOverlay.PauseMenu);
            simulate.FrameUpdate();
            simulate.Click(Element<Button>("pause-options-button"));
            simulate.FrameUpdate();

            simulate.Click(Element<Button>("options-back-button"));
            simulate.FrameUpdate();

            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
            Assert.That(IsShowing("options-screen"), Is.False);
            Assert.That(IsShowing("pause-menu"), Is.True);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("pause-options-button")));
        }

        [Test]
        public void ShowNotBuiltNotice_NamesTheWindowAndShowsTheNotice()
        {
            _presenter.ShowNotBuiltNotice(UnbuiltWindow, Element<Button>("nav-map-button"));

            Assert.That(IsShowing("not-built-notice"), Is.True);
            Assert.That(Element<Label>("not-built-notice-text").text, Is.EqualTo(NotBuiltNoticeTextFormatter.Format(UnbuiltWindow)));
            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.NotBuiltNotice));
        }

        [Test]
        public void NoticeCloseButton_Click_ClosesTheNoticeAndFocusesTheButtonThatOpenedIt()
        {
            _presenter.ShowNotBuiltNotice(UnbuiltWindow, Element<Button>("nav-map-button"));
            simulate.FrameUpdate();

            simulate.Click(Element<Button>("not-built-notice-close-button"));
            simulate.FrameUpdate();

            Assert.That(_overlays.Top, Is.Null);
            Assert.That(IsShowing("not-built-notice"), Is.False);
            Assert.That(FocusedElement, Is.SameAs(Element<Button>("nav-map-button")));
        }

        [Test]
        public void ClosingAWindowNothingOpened_ReturnsFocusToTheGame()
        {
            _overlays.Open(GameScreenOverlay.PauseMenu);

            _overlays.CloseTop();

            Assert.That(IsShowing("pause-menu"), Is.False);
            Assert.That(_gameFocusRequests, Is.EqualTo(1));
        }

        [Test]
        public void SaveAndExit_Click_LoadsTheMainMenu()
        {
            _overlays.Open(GameScreenOverlay.PauseMenu);
            simulate.FrameUpdate();

            simulate.Click(Element<Button>("save-and-exit-button"));
            simulate.FrameUpdate();

            Assert.That(_sceneLoader.Loaded, Is.EqualTo(new[] { SceneNames.MainMenu }));
        }

        [Test]
        public void AfterDispose_NoLongerFollowsTheStack()
        {
            _presenter.Dispose();

            _overlays.Open(GameScreenOverlay.PauseMenu);

            Assert.That(IsShowing("pause-menu"), Is.False);
        }
    }
}
