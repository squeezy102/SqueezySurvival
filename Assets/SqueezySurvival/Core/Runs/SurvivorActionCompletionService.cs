using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Completes survivor actions through the one time path (Docs/Engineering/Architecture.md, Time: one path). The
    /// actions themselves belong to their own services.
    /// </summary>
    public sealed class SurvivorActionCompletionService : ISurvivorActionCompletionService
    {
        private readonly IGameTimeAdvancer _gameTimeAdvancer;
        private readonly IRunEventLog _eventLog;
        private readonly IGroundInventoryService _groundInventory;

        /// <summary>Completes actions on <paramref name="gameTimeAdvancer"/> and <paramref name="eventLog"/>.</summary>
        public SurvivorActionCompletionService(IGameTimeAdvancer gameTimeAdvancer, IRunEventLog eventLog, IGroundInventoryService groundInventory)
        {
            _gameTimeAdvancer = gameTimeAdvancer ?? throw new ArgumentNullException(nameof(gameTimeAdvancer));
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));
        }

        /// <inheritdoc />
        public bool CanStartAction => !_groundInventory.HasPendingOverflow;

        /// <inheritdoc />
        public void RequireCanStartAction()
        {
            if (!CanStartAction)
            {
                throw new InvalidOperationException("Choose what to do with the find that has no room on the ground first.");
            }
        }

        /// <inheritdoc />
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

            // Logging before advancing keeps the action's own entries ahead of anything time-dependent systems log
            // about the time that passed.
            GameClock endsAt = _gameTimeAdvancer.Clock.Advance(duration);
            foreach (EventLogMessage message in messages)
            {
                _eventLog.Add(endsAt, message.Text, message.Severity);
            }

            _gameTimeAdvancer.Advance(duration);
        }
    }
}
