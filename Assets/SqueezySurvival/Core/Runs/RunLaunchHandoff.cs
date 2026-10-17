using System;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Carries a <see cref="NewRunConfiguration"/> from the New Game screen to the game scene. Holds at most one
    /// configuration; taking it empties the handoff, so a run can never be started twice from one choice.
    /// </summary>
    public sealed class RunLaunchHandoff
    {
        private NewRunConfiguration _pending;

        /// <summary>True while a configuration waits to be taken.</summary>
        public bool HasPendingRun => _pending != null;

        /// <summary>Hands over a configuration, replacing any not yet taken.</summary>
        public void Hand(NewRunConfiguration configuration)
        {
            _pending = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>Takes the waiting configuration, if any, and empties the handoff.</summary>
        public bool TryTake(out NewRunConfiguration configuration)
        {
            configuration = _pending;
            _pending = null;
            return configuration != null;
        }
    }
}
