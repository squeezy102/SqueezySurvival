using NUnit.Framework;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunOptionPointEffectsTests
    {
        [Test]
        public void ForEasy_WithoutCasual_DescribesReducedPoints()
        {
            Assert.That(RunOptionPointEffects.ForEasy(false), Is.EqualTo(RunOptionPointEffects.Easy));
        }

        [Test]
        public void ForEasy_WithCasual_DescribesNoPoints()
        {
            Assert.That(RunOptionPointEffects.ForEasy(true), Is.EqualTo(RunOptionPointEffects.EasyOverriddenByCasual));
        }
    }
}
