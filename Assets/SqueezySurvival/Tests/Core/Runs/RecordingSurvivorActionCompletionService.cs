using System;
using System.Collections.Generic;
using System.Linq;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Runs
{
    /// <summary>
    /// Test double for <see cref="ISurvivorActionCompletionService"/>: records each completed action instead of
    /// logging and advancing time, and refuses to start actions while <see cref="CanStartAction"/> is false.
    /// </summary>
    public sealed class RecordingSurvivorActionCompletionService : ISurvivorActionCompletionService
    {
        private readonly List<RecordedAction> _completed = new List<RecordedAction>();

        /// <summary>Set to false to stand in for a find waiting for room on the ground.</summary>
        public bool CanStartAction { get; set; } = true;

        /// <summary>Every action completed, in order.</summary>
        public IReadOnlyList<RecordedAction> Completed => _completed;

        /// <inheritdoc />
        public void RequireCanStartAction()
        {
            if (!CanStartAction)
            {
                throw new InvalidOperationException("Action refused by the test double.");
            }
        }

        /// <inheritdoc />
        public void Complete(ActionDuration duration, IEnumerable<EventLogMessage> messages) =>
            _completed.Add(new RecordedAction(duration, messages.Select(message => message.Text).ToArray()));

        /// <summary>One completed action: how long it took and the text it logged.</summary>
        public sealed class RecordedAction
        {
            /// <summary>Records an action.</summary>
            public RecordedAction(ActionDuration duration, IReadOnlyList<string> messageTexts)
            {
                Duration = duration;
                MessageTexts = messageTexts;
            }

            /// <summary>How long the action took.</summary>
            public ActionDuration Duration { get; }

            /// <summary>The text of each message the action logged, in order.</summary>
            public IReadOnlyList<string> MessageTexts { get; }
        }
    }
}
