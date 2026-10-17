namespace SqueezySurvival.Core.Saving
{
    /// <summary>
    /// Reports which saved games exist, so screens can offer Continue and Load Game only when they can work.
    /// </summary>
    public interface ISavedGameAvailability
    {
        /// <summary>True when at least one run is in progress and can be continued.</summary>
        bool HasRunInProgress { get; }

        /// <summary>True when at least one run in progress or casual save can be loaded.</summary>
        bool HasLoadableSave { get; }
    }
}
