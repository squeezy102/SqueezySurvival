using NUnit.Framework;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunPointsSummaryFormatterTests
    {
        [Test]
        public void Format_Standard_EarnsBothCurrencies()
        {
            Assert.That(RunPointsSummaryFormatter.Format(RunOptions.Standard),
                Is.EqualTo("1 survival point per day, 1 challenge point per 30 days"));
        }

        [Test]
        public void Format_Easy_EarnsOneFifthSurvivalAndNoChallenge()
        {
            Assert.That(RunPointsSummaryFormatter.Format(new RunOptions(false, true, false)),
                Is.EqualTo("1 survival point per 5 days, no challenge points"));
        }

        [Test]
        public void Format_Hard_EarnsDouble()
        {
            Assert.That(RunPointsSummaryFormatter.Format(new RunOptions(false, false, true)),
                Is.EqualTo("2 survival points per day, 2 challenge points per 30 days"));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Format_Casual_EarnsNoPoints(bool isEasy)
        {
            Assert.That(RunPointsSummaryFormatter.Format(new RunOptions(true, isEasy, false)),
                Is.EqualTo(RunPointsSummaryFormatter.NoPoints));
        }
    }
}
