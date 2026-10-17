namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Short descriptions of how each run option changes the points a run earns, shown beside each option on the
    /// New Game screen (Docs/Design/Systems/Runs.md, Run options).
    /// </summary>
    public static class RunOptionPointEffects
    {
        /// <summary>Casual runs earn nothing.</summary>
        public const string Casual = "No points";

        /// <summary>Easy runs earn survival points at 1/5 value and no challenge points.</summary>
        public const string Easy = "1/5 survival points, no challenge points";

        /// <summary>Hard runs earn double.</summary>
        public const string Hard = "Double points";

        /// <summary>Shown beside Easy mode while Casual mode is also ticked.</summary>
        public const string EasyOverriddenByCasual = "No points (Casual mode)";

        /// <summary>
        /// The effect shown beside Easy mode. Casual mode's "no points" overrides Easy mode's reduced points
        /// when both are ticked.
        /// </summary>
        public static string ForEasy(bool isCasualTicked) => isCasualTicked ? EasyOverriddenByCasual : Easy;
    }
}
