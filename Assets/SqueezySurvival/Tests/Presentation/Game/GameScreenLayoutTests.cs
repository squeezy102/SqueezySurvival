using NUnit.Framework;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class GameScreenLayoutTests : GameScreenUITestFixture
    {
        // Overlays cover the whole screen; one shown before its presenter runs would block every click beneath it.
        [TestCase("pause-menu")]
        [TestCase("ground-overflow-prompt")]
        [TestCase("not-built-notice")]
        [TestCase("options-screen")]
        public void Overlay_StartsHiddenUntilItsPresenterShowsIt(string overlayName)
        {
            Assert.That(Element<VisualElement>(overlayName).resolvedStyle.display, Is.EqualTo(DisplayStyle.None));
        }
    }
}
