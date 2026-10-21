using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// A run being played: its fixed configuration and the focused services that hold its state, one job each
    /// (Docs/Engineering/Architecture.md, The run). Built by <see cref="RunSessionFactory"/>. Commands go to the
    /// service that owns them, not to the run; a new system becomes a new service here.
    /// </summary>
    public sealed class RunSession
    {
        private readonly IGameTimeAdvancer _gameTimeAdvancer;

        /// <summary>Holds a run's services. Use <see cref="RunSessionFactory"/> to build one.</summary>
        public RunSession(
            NewRunConfiguration configuration,
            DayPhaseSchedule dayPhaseSchedule,
            IGameTimeAdvancer gameTimeAdvancer,
            RunEventLog eventLog,
            Zone currentZone,
            SurvivorInventoryService survivorInventory,
            GroundInventoryService groundInventory,
            InventoryTransferService inventoryTransfers,
            ZoneExplorationService zoneExploration,
            TimePassingService timePassing)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            _gameTimeAdvancer = gameTimeAdvancer ?? throw new ArgumentNullException(nameof(gameTimeAdvancer));
            EventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            CurrentZone = currentZone ?? throw new ArgumentNullException(nameof(currentZone));
            SurvivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            GroundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
            InventoryTransfers = inventoryTransfers ?? throw new ArgumentNullException(nameof(inventoryTransfers));
            ZoneExploration = zoneExploration ?? throw new ArgumentNullException(nameof(zoneExploration));
            TimePassing = timePassing ?? throw new ArgumentNullException(nameof(timePassing));
        }

        /// <summary>The choices the run was started with. Never changes.</summary>
        public NewRunConfiguration Configuration { get; }

        /// <summary>Phase boundaries used to name the time of day.</summary>
        public DayPhaseSchedule DayPhaseSchedule { get; }

        /// <summary>The run's game time, for reading the clock and hearing when it advances.</summary>
        public IGameTime GameTime => _gameTimeAdvancer;

        /// <summary>The current game time.</summary>
        public GameClock Clock => _gameTimeAdvancer.Clock;

        /// <summary>The current day phase.</summary>
        public DayPhase CurrentDayPhase => DayPhaseSchedule.PhaseAt(Clock.MinuteOfDay);

        /// <summary>Everything that has happened this run.</summary>
        public RunEventLog EventLog { get; }

        /// <summary>The zone the survivor is in.</summary>
        public Zone CurrentZone { get; }

        /// <summary>What the survivor carries.</summary>
        public SurvivorInventoryService SurvivorInventory { get; }

        /// <summary>The current zone's ground and the finds waiting for room on it.</summary>
        public GroundInventoryService GroundInventory { get; }

        /// <summary>Moving things between the survivor's inventory and the ground.</summary>
        public InventoryTransferService InventoryTransfers { get; }

        /// <summary>Exploring the current zone.</summary>
        public ZoneExplorationService ZoneExploration { get; }

        /// <summary>Wait and Rest.</summary>
        public TimePassingService TimePassing { get; }
    }
}
