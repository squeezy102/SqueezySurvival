using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Passes time for Wait and Rest, each completed as a time-taking action.</summary>
    public sealed class TimePassingService : ITimePassingService
    {
        private readonly ISurvivorActionCompletionService _actionCompletion;

        /// <summary>Completes each time-passing action through <paramref name="actionCompletion"/>.</summary>
        public TimePassingService(ISurvivorActionCompletionService actionCompletion)
        {
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
        }

        /// <inheritdoc />
        public bool CanPassTime => _actionCompletion.CanStartAction;

        /// <inheritdoc />
        public void PassTime(TimePassingAction action)
        {
            _actionCompletion.RequireCanStartAction();
            _actionCompletion.Complete(
                ActionDuration.FromMinutes(TimePassingActions.DurationMinutes(action)),
                new[] { new EventLogMessage(TimePassingActions.LogText(action)) });
        }
    }
}
