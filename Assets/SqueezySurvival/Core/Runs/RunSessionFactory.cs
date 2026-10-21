using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Builds a run's services and wires them together, so the game and its tests build a run the same way
    /// (Docs/Engineering/Architecture.md, The run).
    /// </summary>
    public static class RunSessionFactory
    {
        /// <summary>The first event log entry of every run.</summary>
        public const string WakeUpText = "You wake up on a beach. You have no idea how you got here.";

        /// <summary>
        /// Starts a new run: Day 1, 6:00 AM, in <paramref name="startingZone"/>, with waking up logged
        /// (Docs/Design/Systems/Time.md, Start of a run). <paramref name="timeDependentSystemsInOrder"/> are told of
        /// every advance of time, in this order.
        /// </summary>
        public static RunSession StartNewRun(
            NewRunConfiguration configuration,
            DayPhaseSchedule dayPhaseSchedule,
            ZoneDefinition startingZone,
            ExplorationRules explorationRules,
            IRandomNumberSource randomNumberSource,
            IReadOnlyList<ITimeDependentSystem> timeDependentSystemsInOrder)
        {
            if (startingZone == null)
            {
                throw new ArgumentNullException(nameof(startingZone));
            }

            var gameTimeAdvancer = new GameTimeAdvancer(GameClock.RunStart, timeDependentSystemsInOrder);
            var eventLog = new RunEventLog();
            var currentZone = new Zone(startingZone);
            var survivorInventory = new SurvivorInventoryService();
            var groundInventory = new GroundInventoryService(currentZone, gameTimeAdvancer, eventLog);
            var actionCompletion = new SurvivorActionCompletionService(gameTimeAdvancer, eventLog, groundInventory);
            var zoneExploration = new ZoneExplorationService(
                currentZone,
                new ZoneExplorer(explorationRules, randomNumberSource),
                survivorInventory,
                groundInventory,
                actionCompletion);

            eventLog.Add(gameTimeAdvancer.Clock, WakeUpText);
            return new RunSession(
                configuration,
                dayPhaseSchedule,
                gameTimeAdvancer,
                eventLog,
                currentZone,
                survivorInventory,
                groundInventory,
                new InventoryTransferService(survivorInventory, groundInventory, actionCompletion),
                zoneExploration,
                new TimePassingService(actionCompletion));
        }
    }
}
