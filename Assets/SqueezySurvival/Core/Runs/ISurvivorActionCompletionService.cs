using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// What every time-taking action shares: whether it may start, and finishing it by logging and then advancing time
    /// (Docs/Engineering/Architecture.md, The run). Completes actions; never holds them.
    /// </summary>
    public interface ISurvivorActionCompletionService
    {
        /// <summary>
        /// Whether a time-taking action may start: not while a find waits for room on the ground
        /// (Docs/Design/UI/InventoryGrids.md, When the ground is full).
        /// </summary>
        bool CanStartAction { get; }

        /// <summary>Throws unless <see cref="CanStartAction"/>.</summary>
        void RequireCanStartAction();

        /// <summary>
        /// Finishes an action whose effects are applied: logs <paramref name="messages"/>, stamped with the time the
        /// action ends, then advances time by <paramref name="duration"/>.
        /// </summary>
        void Complete(ActionDuration duration, IEnumerable<EventLogMessage> messages);
    }
}
