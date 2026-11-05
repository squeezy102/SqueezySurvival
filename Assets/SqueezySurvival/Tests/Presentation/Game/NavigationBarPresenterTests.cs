using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.GameScreen;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using SqueezySurvival.Presentation.Tests.Session;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// The nav bar: each button's icon and tooltip; Options opens Options, and every gameplay window, unbuilt at the
    /// wireframe stage, says so (Docs/Design/UI/ScreenLayout.md, Nav bar).
    /// </summary>
    [TestFixture]
    public sealed class NavigationBarPresenterTests : GameScreenUITestFixture
    {
        private static readonly NavigationBarEntry UnbuiltWindow = NavigationBarEntries.GameplayWindows[0];

        private PixelArtTextureCache _textures;
        private GridTooltipView _tooltip;
        private GameScreenOverlayStack _overlays;
        private GameScreenOverlayPresenter _overlayPresenter;
        private NavigationBarPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _textures = new PixelArtTextureCache();
            _tooltip = new GridTooltipView(Element<VisualElement>("grid-tooltip"), _textures);
            _overlays = new GameScreenOverlayStack();
            _overlayPresenter = new GameScreenOverlayPresenter(rootVisualElement, _overlays, () => { }, new RecordingSceneLoader());
            _presenter = new NavigationBarPresenter(rootVisualElement, _textures, _tooltip, _overlayPresenter);
            simulate.FrameUpdate();
        }

        [TearDown]
        public void DisposePresenter()
        {
            _overlayPresenter.Dispose();
            _textures.Dispose();
        }

        private static string ButtonName(NavigationBarEntry entry) => $"nav-{entry.Key}-button";

        private bool TooltipIsShowing => Element<VisualElement>("grid-tooltip").style.display == DisplayStyle.Flex;

        [Test]
        public void New_GivesEveryButtonAnIcon()
        {
            foreach (NavigationBarEntry entry in NavigationBarEntries.GameplayWindows.Append(NavigationBarEntries.Options))
            {
                Assert.That(Element<Button>(ButtonName(entry)).Query<PixelIconElement>().ToList(), Has.Count.EqualTo(1), entry.Name);
            }
        }

        [Test]
        public void HoveringAButton_ShowsTheWindowsNameAndWhyItIsUnavailable()
        {
            simulate.MouseMove(Element<Button>(ButtonName(UnbuiltWindow)));
            simulate.FrameUpdate();

            Assert.That(TooltipIsShowing, Is.True);
            Assert.That(Element<Label>("grid-tooltip-title").text, Is.EqualTo(UnbuiltWindow.Name));
            Assert.That(Element<Label>("grid-tooltip-description").text, Is.EqualTo(UnbuiltWindow.Description));
            Assert.That(
                Element<VisualElement>("grid-tooltip-details").Query<Label>(className: "grid-tooltip-detail").ToList().Select(line => line.text),
                Has.Member(UnbuiltWindow.AvailabilityNote));
        }

        [Test]
        public void MovingOffAButton_HidesItsTooltip()
        {
            simulate.MouseMove(Element<Button>(ButtonName(UnbuiltWindow)));
            simulate.FrameUpdate();

            simulate.MouseMove(new Vector2(1f, 1f));
            simulate.FrameUpdate();

            Assert.That(TooltipIsShowing, Is.False);
        }

        [Test]
        public void ClickingAnUnbuiltWindowsButton_HidesTheTooltipAndShowsTheNotBuiltNotice()
        {
            simulate.MouseMove(Element<Button>(ButtonName(UnbuiltWindow)));
            simulate.FrameUpdate();

            simulate.Click(Element<Button>(ButtonName(UnbuiltWindow)));
            simulate.FrameUpdate();

            Assert.That(TooltipIsShowing, Is.False);
            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.NotBuiltNotice));
            Assert.That(Element<Label>("not-built-notice-text").text, Is.EqualTo(NotBuiltNoticeTextFormatter.Format(UnbuiltWindow)));
        }

        [Test]
        public void ClickingOptions_OpensOptions()
        {
            simulate.Click(Element<Button>(ButtonName(NavigationBarEntries.Options)));
            simulate.FrameUpdate();

            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.Options));
        }
    }
}
