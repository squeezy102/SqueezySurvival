using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The state of a run being played: its fixed configuration, the game clock, where the survivor is, and the
    /// event log. Time advances only through actions (Docs/Design/Systems/Time.md, Rules).
    /// </summary>
    public sealed class RunSession
    {
        /// <summary>The first event log entry of every run.</summary>
        public const string WakeUpText = "You wake up on a beach. You have no idea how you got here.";

        private readonly ZoneExplorer _zoneExplorer;

        /// <summary>Starts a run from its configuration: Day 1, 6:00 AM, in the starting zone.</summary>
        public RunSession(
            NewRunConfiguration configuration,
            DayPhaseSchedule dayPhaseSchedule,
            ZoneDefinition startingZone,
            ZoneExplorer zoneExplorer)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            _zoneExplorer = zoneExplorer ?? throw new ArgumentNullException(nameof(zoneExplorer));
            Clock = GameClock.RunStart;
            CurrentZone = new Zone(startingZone ?? throw new ArgumentNullException(nameof(startingZone)));
            EventLog = new RunEventLog();
            EventLog.Add(Clock, WakeUpText);
        }

        /// <summary>Raised after the clock advances.</summary>
        public event Action<GameClock> ClockAdvanced;

        /// <summary>The choices the run was started with. Never changes.</summary>
        public NewRunConfiguration Configuration { get; }

        /// <summary>Phase boundaries used to name the time of day.</summary>
        public DayPhaseSchedule DayPhaseSchedule { get; }

        /// <summary>The current game time.</summary>
        public GameClock Clock { get; private set; }

        /// <summary>The zone the survivor is in.</summary>
        public Zone CurrentZone { get; }

        /// <summary>How long one explore takes, in minutes.</summary>
        public int ExploreMinutes => _zoneExplorer.Rules.ExploreMinutes;

        /// <summary>Everything that has happened this run.</summary>
        public RunEventLog EventLog { get; }

        /// <summary>The current day phase.</summary>
        public DayPhase CurrentDayPhase => DayPhaseSchedule.PhaseAt(Clock.MinuteOfDay);

        /// <summary>Performs a time-passing action. Wait and Rest are always available.</summary>
        public void PassTime(TimePassingAction action)
        {
            Clock = Clock.Advance(TimePassingActions.DurationMinutes(action));
            EventLog.Add(Clock, TimePassingActions.LogText(action));
            ClockAdvanced?.Invoke(Clock);
        }

        /// <summary>
        /// Explores the current zone: takes the explore's time, adds what was found to the zone and logs it
        /// (Docs/Design/Systems/Exploration.md). Always available.
        /// </summary>
        public ExplorationOutcome Explore()
        {
            Clock = Clock.Advance(ExploreMinutes);
            ExplorationOutcome outcome = _zoneExplorer.Explore(CurrentZone);
            foreach (string entry in ExplorationLogTextFormatter.Format(CurrentZone.Name, outcome))
            {
                EventLog.Add(Clock, entry);
            }

            ClockAdvanced?.Invoke(Clock);
            return outcome;
        }
    }
}
