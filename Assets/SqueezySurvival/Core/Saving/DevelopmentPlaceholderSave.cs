using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Saving
{
    /// <summary>
    /// A stand-in run in progress for development only: makes Continue available and starts a brand-new default
    /// run (right-handed, no run options). Used in the Unity Editor and development builds until the save system
    /// exists; release builds never use it (Docs/Design/UI/StartupFlow.md, Wireframe stage).
    /// </summary>
    public sealed class DevelopmentPlaceholderSave : ISavedGameAvailability, IRunInProgressLoader
    {
        /// <inheritdoc />
        public bool HasRunInProgress => true;

        /// <inheritdoc />
        public bool HasLoadableSave => false;

        /// <inheritdoc />
        public bool TryLoadRunInProgress(out NewRunConfiguration configuration)
        {
            configuration = new NewRunConfiguration(DominantHand.Right, RunOptions.Standard);
            return true;
        }
    }
}
