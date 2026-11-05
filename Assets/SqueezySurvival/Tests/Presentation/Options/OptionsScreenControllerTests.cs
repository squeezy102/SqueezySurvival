using NUnit.Framework;
using SqueezySurvival.Presentation.Options;
using SqueezySurvival.Presentation.Tests.MainMenu;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Options
{
    /// <summary>The Options screen shown from the main menu and the game (Docs/Design/UI/Options.md).</summary>
    [TestFixture]
    public sealed class OptionsScreenControllerTests : MainMenuUITestFixture
    {
        private OptionsScreenController _screen;

        [SetUp]
        public void CreateScreen() => _screen = new OptionsScreenController(Element<VisualElement>("options-screen"));

        [Test]
        public void New_IsHidden()
        {
            Assert.That(_screen.IsVisible, Is.False);
        }

        [Test]
        public void Show_ShowsTheScreen()
        {
            _screen.Show();

            Assert.That(_screen.IsVisible, Is.True);
        }

        [Test]
        public void Hide_HidesTheScreen()
        {
            _screen.Show();

            _screen.Hide();

            Assert.That(_screen.IsVisible, Is.False);
        }

        [Test]
        public void BackButton_Click_ReportsClosed()
        {
            int closes = 0;
            _screen.Closed += () => closes++;
            _screen.Show();
            simulate.FrameUpdate();

            simulate.Click(Element<Button>("options-back-button"));
            simulate.FrameUpdate();

            Assert.That(closes, Is.EqualTo(1));
        }
    }
}
