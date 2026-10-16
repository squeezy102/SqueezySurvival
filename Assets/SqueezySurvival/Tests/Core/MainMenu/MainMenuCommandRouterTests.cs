using System;
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

        [TestCase(MainMenuOption.NewGame)]
        [TestCase(MainMenuOption.Options)]
        public void Route_AvailableOptionWithUnbuiltScreen_ShowsNotBuiltNotice(MainMenuOption option)
        {
            Assert.That(CreateRouter(false, false).Route(option), Is.EqualTo(MainMenuCommandOutcome.ShowNotBuiltNotice));
        }

        [TestCase(MainMenuOption.Continue)]
        [TestCase(MainMenuOption.LoadGame)]
        public void Route_UnavailableOption_IsIgnored(MainMenuOption option)
        {
            Assert.That(CreateRouter(false, false).Route(option), Is.EqualTo(MainMenuCommandOutcome.IgnoreUnavailableOption));
        }

        [Test]
        public void Route_Continue_WhenRunInProgress_ShowsNotBuiltNotice()
        {
            Assert.That(CreateRouter(true, true).Route(MainMenuOption.Continue), Is.EqualTo(MainMenuCommandOutcome.ShowNotBuiltNotice));
        }

        [Test]
        public void Constructor_WithNullEvaluator_Throws()
        {
            Assert.That(() => new MainMenuCommandRouter(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
