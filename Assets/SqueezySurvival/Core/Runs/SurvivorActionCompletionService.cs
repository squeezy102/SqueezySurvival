using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// What every time-taking survivor action shares: whether one may start, and finishing it by logging what happened
    /// and then advancing time (Docs/Engineering/Architecture.md, Time: one path). It completes actions; the actions
    /// themselves belong to their own services.
    /// </summary>
    public sealed class SurvivorActionCompletionService
    {
        private readonly IGameTimeAdvancer _gameTimeAdvancer;
        private readonly RunEventLog _eventLog;
        private readonly GroundInventoryService _groundInventory;

        /// <summary>Completes actions on <paramref name="gameTimeAdvancer"/> and <paramref name="eventLog"/>.</summary>
        public SurvivorActionCompletionService(IGameTimeAdvancer gameTimeAdvancer, RunEventLog eventLog, GroundInventoryService groundInventory)
        {
            _gameTimeAdvancer = gameTimeAdvancer ?? throw new ArgumentNullException(nameof(gameTimeAdvancer));
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
        }

        /// <summary>
        /// Whether a time-taking action may start: not while a find waits for room on the ground
        /// (Docs/Design/UI/InventoryGrids.md, When the ground is full).
        /// </summary>
        public bool CanStartAction => !_groundInventory.HasPendingOverflow;

        /// <summary>Throws unless <see cref="CanStartAction"/>.</summary>
        public void RequireCanStartAction()
        {
            if (!CanStartAction)
            {
                throw new InvalidOperationException("Choose what to do with the find that has no room on the ground first.");
            }
        }

        /// <summary>
        /// Finishes an action whose effects are applied: logs <paramref name="messages"/>, stamped with the time the
        /// action ends, then advances time by <paramref name="duration"/>. Logging first keeps the action's own entries
        /// ahead of anything time-dependent systems log about the time that passed.
        /// </summary>
        public void Complete(ActionDuration duration, IEnumerable<EventLogMessage> messages)
        {
            if (duration == null)
            {
                throw new ArgumentNullException(nameof(duration));
            }

            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            GameClock endsAt = _gameTimeAdvancer.Clock.Advance(duration);
            foreach (EventLogMessage message in messages)
            {
                _eventLog.Add(endsAt, message.Text, message.Severity);
            }

            _gameTimeAdvancer.Advance(duration);
        }
    }
}
