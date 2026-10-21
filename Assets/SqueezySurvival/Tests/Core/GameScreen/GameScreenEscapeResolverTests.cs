using NUnit.Framework;
using SqueezySurvival.Core.GameScreen;

namespace SqueezySurvival.Core.Tests.GameScreen
{
    [TestFixture]
    public sealed class GameScreenEscapeResolverTests
    {
        private static readonly GameScreenInteraction Idle = new GameScreenInteraction(false, false, false, false);

        // Each case has its own condition and every lower-priority one, so it passes only if the order is right.
        [TestCase(true, true, true, true, GameScreenEscapeAction.CancelDrag)]
        [TestCase(false, true, true, true, GameScreenEscapeAction.ClearSelection)]
        [TestCase(false, false, true, true, GameScreenEscapeAction.ReturnToGroundOverflowQuestion)]
        [TestCase(false, false, false, true, GameScreenEscapeAction.WaitForGroundOverflowAnswer)]
        public void PressEscape_InteractionsComeBeforeOverlaysInTheDocumentedOrder(
            bool isDragging, bool hasSelection, bool isChoosingDiscard, bool isAsking, GameScreenEscapeAction expected)
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);
            var resolver = new GameScreenEscapeResolver(overlays);

            GameScreenEscapeAction action = resolver.PressEscape(new GameScreenInteraction(isDragging, hasSelection, isChoosingDiscard, isAsking));

            Assert.That(action, Is.EqualTo(expected));
            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.Options));
        }

        [Test]
        public void PressEscape_NothingOpen_OpensThePauseMenu()
        {
            var overlays = new GameScreenOverlayStack();

            GameScreenEscapeAction action = new GameScreenEscapeResolver(overlays).PressEscape(Idle);

            Assert.That(action, Is.EqualTo(GameScreenEscapeAction.OpenPauseMenu));
            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
        }

        [Test]
        public void PressEscape_PauseMenuOpen_ClosesIt()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);

            GameScreenEscapeAction action = new GameScreenEscapeResolver(overlays).PressEscape(Idle);

            Assert.That(action, Is.EqualTo(GameScreenEscapeAction.CloseTopOverlay));
            Assert.That(overlays.Top, Is.Null);
        }

        [Test]
        public void PressEscape_OptionsOpenedFromThePauseMenu_ReturnsToThePauseMenu()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);

            new GameScreenEscapeResolver(overlays).PressEscape(Idle);

            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
        }

        [Test]
        public void PressEscape_NotBuiltNoticeOpen_ClosesItBeforeAnythingBeneath()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.NotBuiltNotice);

            new GameScreenEscapeResolver(overlays).PressEscape(Idle);

            Assert.That(overlays.IsOpen(GameScreenOverlay.NotBuiltNotice), Is.False);
            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
        }

        [Test]
        public void PressEscape_Repeatedly_BacksOutToTheGame()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);
            var resolver = new GameScreenEscapeResolver(overlays);

            resolver.PressEscape(Idle);
            resolver.PressEscape(Idle);

            Assert.That(overlays.Top, Is.Null);
        }

        [Test]
        public void PressEscape_NullInteraction_IsRefused()
        {
            var resolver = new GameScreenEscapeResolver(new GameScreenOverlayStack());

            Assert.That(() => resolver.PressEscape(null), Throws.ArgumentNullException);
        }
    }
}
