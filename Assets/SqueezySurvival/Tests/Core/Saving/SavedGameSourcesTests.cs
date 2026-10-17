using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Saving;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.Saving
{
    [TestFixture]
    public sealed class SavedGameSourcesTests
    {
        [Test]
        public void DevelopmentPlaceholderSave_OffersARunInProgress()
        {
            Assert.That(new DevelopmentPlaceholderSave().HasRunInProgress, Is.True);
        }

        [Test]
        public void DevelopmentPlaceholderSave_LoadsDefaultRightHandedRun()
        {
            bool loaded = new DevelopmentPlaceholderSave().TryLoadRunInProgress(out NewRunConfiguration configuration);

            Assert.That(loaded, Is.True);
            Assert.That(configuration.DominantHand, Is.EqualTo(DominantHand.Right));
            Assert.That(configuration.RunOptions.IsCasual || configuration.RunOptions.IsEasy || configuration.RunOptions.IsHard, Is.False);
        }

        [Test]
        public void NoSavedGamesAvailability_LoadsNothing()
        {
            var noSaves = new NoSavedGamesAvailability();

            Assert.That(noSaves.HasRunInProgress, Is.False);
            Assert.That(noSaves.TryLoadRunInProgress(out _), Is.False);
        }
    }
}
