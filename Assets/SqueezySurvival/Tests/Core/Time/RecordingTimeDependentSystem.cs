using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    /// <summary>A time-dependent system that records each advance it is told of, and runs an optional reaction.</summary>
    public sealed class RecordingTimeDependentSystem : ITimeDependentSystem
    {
        private readonly List<GameTimeAdvance> _advances = new List<GameTimeAdvance>();
        private readonly Action<GameTimeAdvance> _reaction;

        /// <summary>Creates a system that only records.</summary>
        public RecordingTimeDependentSystem()
            : this(_ => { })
        {
        }

        /// <summary>Creates a system that records, then runs <paramref name="reaction"/>.</summary>
        public RecordingTimeDependentSystem(Action<GameTimeAdvance> reaction)
        {
            _reaction = reaction;
        }

        /// <summary>Every advance this system was told of, oldest first.</summary>
        public IReadOnlyList<GameTimeAdvance> Advances => _advances;

        /// <inheritdoc />
        public void ApplyTimePassed(GameTimeAdvance advance)
        {
            _advances.Add(advance);
            _reaction(advance);
        }
    }
}
