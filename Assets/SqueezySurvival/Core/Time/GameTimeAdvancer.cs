using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// Owns a run's clock and is the only code that changes it. Each advance tells the time-dependent systems in the
    /// order they were given, then raises <see cref="TimeAdvanced"/> (Docs/Engineering/Architecture.md, Time: one path).
    /// </summary>
    public sealed class GameTimeAdvancer : IGameTimeAdvancer
    {
        private readonly IReadOnlyList<ITimeDependentSystem> _timeDependentSystemsInOrder;
        private bool _isAdvancing;

        /// <summary>
        /// Starts the clock at <paramref name="start"/>. <paramref name="timeDependentSystemsInOrder"/> are told of each
        /// advance in this order; the order is set by the composition root and documented in Architecture.md.
        /// </summary>
        public GameTimeAdvancer(GameClock start, IReadOnlyList<ITimeDependentSystem> timeDependentSystemsInOrder)
        {
            Clock = start ?? throw new ArgumentNullException(nameof(start));
            _timeDependentSystemsInOrder = timeDependentSystemsInOrder ?? throw new ArgumentNullException(nameof(timeDependentSystemsInOrder));
            for (int index = 0; index < _timeDependentSystemsInOrder.Count; index++)
            {
                if (_timeDependentSystemsInOrder[index] == null)
                {
                    throw new ArgumentException($"Time-dependent system {index} is null.", nameof(timeDependentSystemsInOrder));
                }
            }
        }

        /// <inheritdoc />
        public event EventHandler<GameTimeAdvance> TimeAdvanced;

        /// <inheritdoc />
        public GameClock Clock { get; private set; }

        /// <summary>
        /// Advances the clock by <paramref name="duration"/>. Throws if called while an advance is still being applied:
        /// a reaction to time passing belongs to the action that caused it (Docs/Engineering/Architecture.md, Commands,
        /// state and change events).
        /// </summary>
        public GameTimeAdvance Advance(ActionDuration duration)
        {
            if (_isAdvancing)
            {
                throw new InvalidOperationException("Time cannot advance while an advance is being applied.");
            }

            _isAdvancing = true;
            try
            {
                var advance = new GameTimeAdvance(Clock, duration);
                Clock = advance.Current;
                foreach (ITimeDependentSystem system in _timeDependentSystemsInOrder)
                {
                    system.ApplyTimePassed(advance);
                }

                TimeAdvanced?.Invoke(this, advance);
                return advance;
            }
            finally
            {
                _isAdvancing = false;
            }
        }
    }
}
