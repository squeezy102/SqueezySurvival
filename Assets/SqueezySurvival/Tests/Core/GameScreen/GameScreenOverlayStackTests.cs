using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.GameScreen;

namespace SqueezySurvival.Core.Tests.GameScreen
{
    [TestFixture]
    public sealed class GameScreenOverlayStackTests
    {
        [Test]
        public void NewStack_ShowsNothing()
        {
            Assert.That(new GameScreenOverlayStack().Top, Is.Null);
        }

        [Test]
        public void Open_ShowsTheNewestOnTopAndKeepsTheOthersOpen()
        {
            var overlays = new GameScreenOverlayStack();

            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);

            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(overlays.IsOpen(GameScreenOverlay.PauseMenu), Is.True);
        }

        [Test]
        public void Open_AlreadyOpen_ChangesNothingAndRaisesNothing()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);
            int changes = 0;
            overlays.Changed += (sender, change) => changes++;

            overlays.Open(GameScreenOverlay.PauseMenu);

            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(changes, Is.Zero);
        }

        [Test]
        public void CloseTop_ShowsTheOneBeneath()
        {
            var overlays = new GameScreenOverlayStack();
            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);

            overlays.CloseTop();

            Assert.That(overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
            Assert.That(overlays.IsOpen(GameScreenOverlay.Options), Is.False);
        }

        [Test]
        public void CloseTop_NothingOpen_RaisesNothing()
        {
            var overlays = new GameScreenOverlayStack();
            int changes = 0;
            overlays.Changed += (sender, change) => changes++;

            overlays.CloseTop();

            Assert.That(changes, Is.Zero);
        }

        [Test]
        public void Changed_ReportsWhatOpenedOrClosedAndWhatIsShownAfter()
        {
            var overlays = new GameScreenOverlayStack();
            var changes = new List<GameScreenOverlayChange>();
            overlays.Changed += (sender, change) => changes.Add(change);

            overlays.Open(GameScreenOverlay.PauseMenu);
            overlays.Open(GameScreenOverlay.Options);
            overlays.CloseTop();

            Assert.That(changes[1].Overlay, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(changes[1].Opened, Is.True);
            Assert.That(changes[1].ShownAfter, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(changes[2].Overlay, Is.EqualTo(GameScreenOverlay.Options));
            Assert.That(changes[2].Opened, Is.False);
            Assert.That(changes[2].ShownAfter, Is.EqualTo(GameScreenOverlay.PauseMenu));
        }
    }
}
