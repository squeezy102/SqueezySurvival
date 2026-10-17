using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

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

        /// <summary>Starts a run from its configuration: Day 1, 6:00 AM, on the Beach.</summary>
        public RunSession(NewRunConfiguration configuration, DayPhaseSchedule dayPhaseSchedule)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            Clock = GameClock.RunStart;
            CurrentZoneName = NewRunConfiguration.StartingZoneName;
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
        public string CurrentZoneName { get; }

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
    }
}
