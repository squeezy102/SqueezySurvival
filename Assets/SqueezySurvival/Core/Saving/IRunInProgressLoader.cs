using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Saving
{
    /// <summary>Loads the most recent run in progress, for Continue on the main menu.</summary>
    public interface IRunInProgressLoader
    {
        /// <summary>Loads the run in progress, if there is one.</summary>
        bool TryLoadRunInProgress(out NewRunConfiguration configuration);
    }
}
