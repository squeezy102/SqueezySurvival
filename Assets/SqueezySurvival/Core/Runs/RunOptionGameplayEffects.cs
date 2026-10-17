namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Short descriptions of how each run option changes play, shown beside each option on the New Game screen
    /// (Docs/Design/Systems/Runs.md, Run options).
    /// </summary>
    public static class RunOptionGameplayEffects
    {
        /// <summary>Casual mode allows free saving and loading.</summary>
        public const string Casual = "Save and load any time; death is not final";

        /// <summary>Easy mode makes survival gentler.</summary>
        public const string Easy = "Slower hunger, gentler penalties, slower decay";

        /// <summary>Hard mode makes survival harsher.</summary>
        public const string Hard = "Faster hunger, things break more often, harsher weather";
    }
}
