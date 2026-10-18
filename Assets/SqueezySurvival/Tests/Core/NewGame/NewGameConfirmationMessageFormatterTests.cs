using NUnit.Framework;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Tests.NewGame
{
    [TestFixture]
    public sealed class NewGameConfirmationMessageFormatterTests
    {
        [Test]
        public void Format_Default_DescribesUnmodifiedGameAndEarnings()
        {
            Assert.That(NewGameConfirmationMessageFormatter.Format(RunOptions.Standard), Is.EqualTo(
                "You are starting an unmodified default game. You will earn 1 survival point per day survived, " +
                "and 1 challenge point per 30 days survived. Are you sure?"));
        }

        [Test]
        public void Format_CasualAndEasy_DescribesNoPoints()
        {
            Assert.That(NewGameConfirmationMessageFormatter.Format(new RunOptions(true, true, false)), Is.EqualTo(
                "You are starting a Casual and Easy mode game. You will be awarded no points for this adventure. Are you sure?"));
        }

        [Test]
        public void Format_Casual_DescribesNoPoints()
        {
            Assert.That(NewGameConfirmationMessageFormatter.Format(new RunOptions(true, false, false)), Is.EqualTo(
                "You are starting a Casual mode game. You will be awarded no points for this adventure. Are you sure?"));
        }

        [Test]
        public void Format_Easy_DescribesReducedEarnings()
        {
            Assert.That(NewGameConfirmationMessageFormatter.Format(new RunOptions(false, true, false)), Is.EqualTo(
                "You are starting an Easy mode game. You will earn 1 survival point per 5 days survived, " +
                "and no challenge points. Are you sure?"));
        }

        [Test]
        public void Format_Hard_DescribesDoubleEarnings()
        {
            Assert.That(NewGameConfirmationMessageFormatter.Format(new RunOptions(false, false, true)), Is.EqualTo(
                "You are starting a Hard mode game. You will earn 2 survival points per day survived, " +
                "and 2 challenge points per 30 days survived. Are you sure?"));
        }
    }
}
