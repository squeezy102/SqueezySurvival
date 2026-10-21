using System;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Exploring the zone the survivor is in: one explore takes its time, adds what was found and logs it
    /// (Docs/Design/Systems/Exploration.md).
    /// </summary>
    public sealed class ZoneExplorationService
    {
        private readonly Zone _currentZone;
        private readonly ZoneExplorer _zoneExplorer;
        private readonly SurvivorInventoryService _survivorInventory;
        private readonly GroundInventoryService _groundInventory;
        private readonly SurvivorActionCompletionService _actionCompletion;

        /// <summary>Explores <paramref name="currentZone"/> with <paramref name="zoneExplorer"/>.</summary>
        public ZoneExplorationService(
            Zone currentZone,
            ZoneExplorer zoneExplorer,
            SurvivorInventoryService survivorInventory,
            GroundInventoryService groundInventory,
            SurvivorActionCompletionService actionCompletion)
        {
            _currentZone = currentZone ?? throw new ArgumentNullException(nameof(currentZone));
            _zoneExplorer = zoneExplorer ?? throw new ArgumentNullException(nameof(zoneExplorer));
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
        }

        /// <summary>How long one explore takes, in minutes.</summary>
        public int ExploreMinutes => _zoneExplorer.Rules.ExploreMinutes;

        /// <summary>
        /// Whether the survivor can explore: not while a find waits for room on the ground, and not while immobile
        /// (Docs/Design/Systems/Inventory.md, Load).
        /// </summary>
        public bool CanExplore => _actionCompletion.CanStartAction && _survivorInventory.CanMove;

        /// <summary>
        /// Explores the current zone once. Finds with no room on the ground wait in
        /// <see cref="GroundInventoryService.PendingOverflow"/>.
        /// </summary>
        public ExplorationOutcome Explore()
        {
            _actionCompletion.RequireCanStartAction();
            if (!_survivorInventory.CanMove)
            {
                throw new InvalidOperationException("The survivor carries too much to move.");
            }

            ExplorationOutcome outcome = _zoneExplorer.Explore(_currentZone);
            _actionCompletion.Complete(
                ActionDuration.FromMinutes(ExploreMinutes),
                ExplorationLogTextFormatter.Format(_currentZone.Name, outcome));
            _groundInventory.QueueOverflow(outcome.GroundOverflow);
            return outcome;
        }
    }
}
