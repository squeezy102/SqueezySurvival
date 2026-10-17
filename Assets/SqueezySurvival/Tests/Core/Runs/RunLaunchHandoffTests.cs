using System;
using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class RunLaunchHandoffTests
    {
        private static NewRunConfiguration CreateConfiguration() => new NewRunConfiguration(DominantHand.Left, RunOptions.Standard);

        [Test]
        public void TryTake_WhenEmpty_ReturnsFalse()
        {
            Assert.That(new RunLaunchHandoff().TryTake(out _), Is.False);
        }

        [Test]
        public void TryTake_AfterHand_ReturnsTheConfiguration()
        {
            var handoff = new RunLaunchHandoff();
            NewRunConfiguration configuration = CreateConfiguration();
            handoff.Hand(configuration);

            bool taken = handoff.TryTake(out NewRunConfiguration takenConfiguration);

            Assert.That(taken, Is.True);
            Assert.That(takenConfiguration, Is.SameAs(configuration));
        }

        [Test]
        public void TryTake_Twice_SecondReturnsFalse()
        {
            var handoff = new RunLaunchHandoff();
            handoff.Hand(CreateConfiguration());
            handoff.TryTake(out _);

            Assert.That(handoff.TryTake(out _), Is.False);
            Assert.That(handoff.HasPendingRun, Is.False);
        }

        [Test]
        public void Hand_WithNull_Throws()
        {
            Assert.That(() => new RunLaunchHandoff().Hand(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
