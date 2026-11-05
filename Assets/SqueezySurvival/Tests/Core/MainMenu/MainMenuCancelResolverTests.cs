using NUnit.Framework;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Core.Tests.MainMenu
{
    [TestFixture]
    public sealed class MainMenuCancelResolverTests
    {
        [Test]
        public void Resolve_NothingOpen_DoesNothing()
        {
            Assert.That(MainMenuCancelResolver.Resolve(new MainMenuWindowState(false, false, false, false)), Is.EqualTo(MainMenuCancelAction.Nothing));
        }

        [Test]
        public void Resolve_OptionsOpen_ClosesOptions()
        {
            Assert.That(MainMenuCancelResolver.Resolve(new MainMenuWindowState(false, false, false, true)), Is.EqualTo(MainMenuCancelAction.CloseOptionsScreen));
        }

        [Test]
        public void Resolve_NewGameScreenOpen_ReturnsToTheMenu()
        {
            Assert.That(MainMenuCancelResolver.Resolve(new MainMenuWindowState(false, false, true, false)), Is.EqualTo(MainMenuCancelAction.ReturnFromNewGameScreen));
        }

        [Test]
        public void Resolve_StartConfirmationOverTheNewGameScreen_GoesBackFromTheConfirmationFirst()
        {
            Assert.That(MainMenuCancelResolver.Resolve(new MainMenuWindowState(false, true, true, false)), Is.EqualTo(MainMenuCancelAction.GoBackFromStartConfirmation));
        }

        [Test]
        public void Resolve_NoticeOverEveryOtherWindow_ClosesTheNoticeFirst()
        {
            Assert.That(MainMenuCancelResolver.Resolve(new MainMenuWindowState(true, true, true, true)), Is.EqualTo(MainMenuCancelAction.CloseNotBuiltNotice));
        }

        [Test]
        public void Resolve_NoWindowState_IsRefused()
        {
            Assert.That(() => MainMenuCancelResolver.Resolve(null), Throws.ArgumentNullException);
        }
    }
}
