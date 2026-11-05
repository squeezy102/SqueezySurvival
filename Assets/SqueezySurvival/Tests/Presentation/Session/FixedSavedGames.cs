using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Saving;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Presentation.Tests.Session
{
    /// <summary>Saved games set directly by a test; a run in progress loads as a standard run with a right-handed survivor.</summary>
    public sealed class FixedSavedGames : ISavedGameAvailability, IRunInProgressLoader
    {
        /// <summary>Describes what the player has saved.</summary>
        public FixedSavedGames(bool hasRunInProgress, bool hasLoadableSave)
        {
            HasRunInProgress = hasRunInProgress;
            HasLoadableSave = hasLoadableSave;
        }

        /// <inheritdoc />
        public bool HasRunInProgress { get; }

        /// <inheritdoc />
        public bool HasLoadableSave { get; }

        /// <inheritdoc />
        public bool TryLoadRunInProgress(out NewRunConfiguration configuration)
        {
            configuration = HasRunInProgress ? new NewRunConfiguration(DominantHand.Right, RunOptions.Standard) : null;
            return HasRunInProgress;
        }
    }
}
