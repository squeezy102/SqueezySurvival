using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class NewRunConfigurationSummaryFormatterTests
    {
        [Test]
        public void Format_DefaultRun_DescribesHandModeAndStart()
        {
            var configuration = new NewRunConfiguration(DominantHand.Right, RunOptions.Standard);

            Assert.That(NewRunConfigurationSummaryFormatter.Format(configuration),
                Is.EqualTo("Right-handed, default mode. Day 1, 6:00 AM, Beach."));
        }

        [Test]
        public void Format_LeftHandedCasualEasy_DescribesBothModes()
        {
            var configuration = new NewRunConfiguration(DominantHand.Left, new RunOptions(true, true, false));

            Assert.That(NewRunConfigurationSummaryFormatter.Format(configuration),
                Is.EqualTo("Left-handed, Casual and Easy mode. Day 1, 6:00 AM, Beach."));
        }
    }
}
