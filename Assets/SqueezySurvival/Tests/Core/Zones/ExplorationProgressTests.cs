using NUnit.Framework;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Zones
{
    [TestFixture]
    public sealed class ExplorationProgressTests
    {
        [Test]
        public void WholePercent_OneOfTwentyFour_RoundsDownToFour()
        {
            Assert.That(new ExplorationProgress(1, 24).WholePercent, Is.EqualTo(4));
        }

        [Test]
        public void WholePercent_OneShortOfMapped_IsNinetyNineNotHundred()
        {
            Assert.That(new ExplorationProgress(99, 100).WholePercent, Is.EqualTo(99));
            Assert.That(new ExplorationProgress(23, 24).IsFullyExplored, Is.False);
        }

        [Test]
        public void WholePercent_BeyondMapped_StaysAtHundred()
        {
            var progress = new ExplorationProgress(30, 24);

            Assert.That(progress.WholePercent, Is.EqualTo(100));
            Assert.That(progress.IsFullyExplored, Is.True);
        }

        [Test]
        public void AfterOneExplore_CountsOneMore()
        {
            Assert.That(new ExplorationProgress(5, 24).AfterOneExplore().ExploresCompleted, Is.EqualTo(6));
        }
    }
}
