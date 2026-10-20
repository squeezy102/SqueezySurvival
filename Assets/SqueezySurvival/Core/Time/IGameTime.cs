using System;

namespace SqueezySurvival.Core.Time
{
    /// <summary>Read access to a run's game time: the current clock and notice of each advance.</summary>
    public interface IGameTime
    {
        /// <summary>Raised after the clock advances and every time-dependent system has been told.</summary>
        event EventHandler<GameTimeAdvance> TimeAdvanced;

        /// <summary>The current game time.</summary>
        GameClock Clock { get; }
    }
}
