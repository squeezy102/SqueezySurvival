using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Saving
{
    /// <summary>
    /// Reports that no saved games exist. Used until the save system is built
    /// (Docs/Design/Systems/Runs.md, Saving).
    /// </summary>
    public sealed class NoSavedGamesAvailability : ISavedGameAvailability, IRunInProgressLoader
    {
        /// <inheritdoc />
        public bool HasRunInProgress => false;

        /// <inheritdoc />
        public bool HasLoadableSave => false;

        /// <inheritdoc />
        public bool TryLoadRunInProgress(out NewRunConfiguration configuration)
        {
            configuration = null;
            return false;
        }
    }
}
