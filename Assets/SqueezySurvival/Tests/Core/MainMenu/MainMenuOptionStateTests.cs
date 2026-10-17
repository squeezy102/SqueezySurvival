using System;
using NUnit.Framework;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Core.Tests.MainMenu
{
    [TestFixture]
    public sealed class MainMenuOptionStateTests
    {
        [Test]
        public void IsAvailable_WithEmptyReason_IsTrue()
        {
            Assert.That(new MainMenuOptionState(MainMenuOption.NewGame, "New Game", string.Empty).IsAvailable, Is.True);
        }

        [Test]
        public void IsAvailable_WithNullReason_IsTrue()
        {
            Assert.That(new MainMenuOptionState(MainMenuOption.NewGame, "New Game", null).IsAvailable, Is.True);
        }

        [Test]
        public void IsAvailable_WithReason_IsFalse()
        {
            Assert.That(new MainMenuOptionState(MainMenuOption.Continue, "Continue", "No game in progress").IsAvailable, Is.False);
        }

        [Test]
        public void DisplayText_WhenAvailable_IsLabel()
        {
            Assert.That(new MainMenuOptionState(MainMenuOption.Continue, "Continue", string.Empty).DisplayText, Is.EqualTo("Continue"));
        }

        [Test]
        public void DisplayText_WhenUnavailable_IsReason()
        {
            Assert.That(new MainMenuOptionState(MainMenuOption.Continue, "Continue", "No game in progress").DisplayText, Is.EqualTo("No game in progress"));
        }

        [TestCase(null)]
        [TestCase("   ")]
        public void Constructor_WithoutLabel_Throws(string label)
        {
            Assert.That(() => new MainMenuOptionState(MainMenuOption.NewGame, label, string.Empty), Throws.ArgumentException);
        }
    }
}
