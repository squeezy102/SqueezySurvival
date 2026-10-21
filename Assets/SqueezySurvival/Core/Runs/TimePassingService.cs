using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Wait and Rest: actions whose only purpose is to pass time (Docs/Design/Systems/Time.md, Passing time deliberately).</summary>
    public sealed class TimePassingService
    {
        private readonly SurvivorActionCompletionService _actionCompletion;

        /// <summary>Completes each time-passing action through <paramref name="actionCompletion"/>.</summary>
        public TimePassingService(SurvivorActionCompletionService actionCompletion)
        {
            _actionCompletion = actionCompletion ?? throw new ArgumentNullException(nameof(actionCompletion));
        }

        /// <summary>Whether time can be passed now.</summary>
        public bool CanPassTime => _actionCompletion.CanStartAction;

        /// <summary>Performs a time-passing action and logs it.</summary>
        public void PassTime(TimePassingAction action)
        {
            _actionCompletion.RequireCanStartAction();
            _actionCompletion.Complete(
                ActionDuration.FromMinutes(TimePassingActions.DurationMinutes(action)),
                new[] { new EventLogMessage(TimePassingActions.LogText(action)) });
        }
    }
}
