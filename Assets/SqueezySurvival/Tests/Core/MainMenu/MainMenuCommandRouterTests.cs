using NUnit.Framework;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Core.Tests.MainMenu
{
    [TestFixture]
    public sealed class MainMenuCommandRouterTests
    {
        private static MainMenuCommandRouter CreateRouter(bool hasRunInProgress, bool hasLoadableSave)
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(hasRunInProgress, hasLoadableSave));
            return new MainMenuCommandRouter(evaluator);
        }

        [Test]
        public void Route_Quit_QuitsApplication()
        {
            Assert.That(CreateRouter(false, false).Route(MainMenuOption.Quit), Is.EqualTo(MainMenuCommandOutcome.QuitApplication));
        }

        [Test]
        public void Route_NewGame_ShowsNewGameScreen()
        {
            Assert.That(CreateRouter(false, false).Route(MainMenuOption.NewGame), Is.EqualTo(MainMenuCommandOutcome.ShowNewGameScreen));
        }

        [Test]
        public void Route_Options_ShowsOptionsScreen()
        {
            Assert.That(CreateRouter(false, false).Route(MainMenuOption.Options), Is.EqualTo(MainMenuCommandOutcome.ShowOptionsScreen));
        }

        [TestCase(MainMenuOption.Continue)]
        [TestCase(MainMenuOption.LoadGame)]
        public void Route_UnavailableOption_IsIgnored(MainMenuOption option)
        {
            Assert.That(CreateRouter(false, false).Route(option), Is.EqualTo(MainMenuCommandOutcome.IgnoreUnavailableOption));
        }

        [Test]
        public void Route_Continue_WhenRunInProgress_ContinuesRun()
        {
            Assert.That(CreateRouter(true, true).Route(MainMenuOption.Continue), Is.EqualTo(MainMenuCommandOutcome.ContinueRun));
        }
    }
}
