using SqueezySurvival.Core.Exploration;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Exploring the zone the survivor is in: one explore takes its time, adds what was found and logs it
    /// (Docs/Design/Systems/Exploration.md).
    /// </summary>
    public interface IZoneExplorationService
    {
        /// <summary>How long one explore takes, in minutes.</summary>
        int ExploreMinutes { get; }

        /// <summary>
        /// Whether the survivor can explore: not while a find waits for room on the ground, and not while immobile
        /// (Docs/Design/Systems/Inventory.md, Load).
        /// </summary>
        bool CanExplore { get; }

        /// <summary>
        /// Explores the current zone once. Finds with no room on the ground wait in
        /// <see cref="IGroundInventoryService.PendingOverflow"/>.
        /// </summary>
        ExplorationOutcome Explore();
    }
}
