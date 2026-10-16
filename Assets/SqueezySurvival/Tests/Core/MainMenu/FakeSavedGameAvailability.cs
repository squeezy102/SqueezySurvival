using SqueezySurvival.Core.Saving;

namespace SqueezySurvival.Core.Tests.MainMenu
{
    /// <summary>Saved game availability set directly by a test.</summary>
    internal sealed class FakeSavedGameAvailability : ISavedGameAvailability
    {
        public FakeSavedGameAvailability(bool hasRunInProgress, bool hasLoadableSave)
        {
            HasRunInProgress = hasRunInProgress;
            HasLoadableSave = hasLoadableSave;
        }

        public bool HasRunInProgress { get; }

        public bool HasLoadableSave { get; }
    }
}
