using NUnit.Framework;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunOptionsTests
    {
        [Test]
        public void Constructor_CasualAndEasy_IsAllowed()
        {
            var options = new RunOptions(true, true, false);

            Assert.That(options.IsCasual && options.IsEasy, Is.True);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Constructor_HardWithCasualOrEasy_Throws(bool isCasual, bool isEasy)
        {
            Assert.That(() => new RunOptions(isCasual, isEasy, true), Throws.ArgumentException);
        }
    }
}
