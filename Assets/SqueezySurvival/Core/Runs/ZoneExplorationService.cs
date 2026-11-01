using System;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Explores the current zone with an explorer, then completes the explore as a time-taking action.</summary>
    public sealed class ZoneExplorationService : IZoneExplorationService
    {
        private readonly Zone _currentZone;
        private readonly IZoneExplorer _zoneExplorer;
        private readonly ISurvivorInventoryService _survivorInventory;
        private readonly IGroundInventoryService _groundInventory;
        private readonly ISurvivorActionCompletionService _actionCompletion;

        /// <summary>Explores <paramref name="currentZone"/> with <paramref name="zoneExplorer"/>.</summary>
        public ZoneExplorationService(
            Zone currentZone,
            IZoneExplorer zoneExplorer,
            ISurvivorInventoryService survivorInventory,
            IGroundInventoryService groundInventory,
            ISurvivorActionCompletionService actionCompletion)
        {
            _currentZone = currentZone ?? throw new ArgumentNullException(nameof(currentZone));
            _zoneExplorer = zoneExplorer ?? throw new ArgumentNullException(nameof(zoneExplorer));
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
        }

        /// <inheritdoc />
        public int ExploreMinutes => _zoneExplorer.Rules.ExploreMinutes;

        /// <inheritdoc />
        public bool CanExplore => _actionCompletion.CanStartAction && _survivorInventory.CanMove;

        /// <inheritdoc />
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
