using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The current zone's ground and its waiting finds. Each zone keeps its own ground; this service works on the
    /// current zone's.
    /// </summary>
    public sealed class GroundInventoryService : IGroundInventoryService
    {
        private readonly Zone _currentZone;
        private readonly IGameTime _gameTime;
        private readonly IRunEventLog _eventLog;
        private readonly List<CountedThing<ItemDefinition>> _pendingOverflow = new List<CountedThing<ItemDefinition>>();

        /// <summary>Works on <paramref name="currentZone"/>'s ground, logging to <paramref name="eventLog"/>.</summary>
        public GroundInventoryService(Zone currentZone, IGameTime gameTime, IRunEventLog eventLog)
        {
            _currentZone = currentZone ?? throw new ArgumentNullException(nameof(currentZone));
            _gameTime = gameTime ?? throw new ArgumentNullException(nameof(gameTime));
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
        }

        /// <inheritdoc />
        public event Action PendingOverflowChanged;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> Ground => _currentZone.GroundPool;

        /// <inheritdoc />
        public IReadOnlyList<CountedThing<ItemDefinition>> PendingOverflow => _pendingOverflow;

        /// <inheritdoc />
        public bool HasPendingOverflow => _pendingOverflow.Count > 0;

        /// <inheritdoc />
        public void QueueOverflow(IReadOnlyList<CountedThing<ItemDefinition>> finds)
        {
            if (finds == null)
            {
                throw new ArgumentNullException(nameof(finds));
            }

            if (finds.Count == 0)
            {
                return;
            }

            _pendingOverflow.AddRange(finds);
            PendingOverflowChanged?.Invoke();
        }

        /// <inheritdoc />
        public void LeavePendingOverflowBehind()
        {
            CountedThing<ItemDefinition> incoming = FirstPendingOverflow();
            _pendingOverflow.RemoveAt(0);
            _eventLog.Add(_gameTime.Clock, GroundOverflowTextFormatter.FormatLeftBehindLog(incoming));
            PendingOverflowChanged?.Invoke();
        }

        /// <inheritdoc />
        public void DiscardToMakeRoomForPendingOverflow(int groundSlot)
        {
            CountedThing<ItemDefinition> incoming = FirstPendingOverflow();
            CountedEntry<ItemDefinition> discarded = Ground.ReplaceSlot(groundSlot, incoming);
            _pendingOverflow.RemoveAt(0);
            _eventLog.Add(_gameTime.Clock, GroundOverflowTextFormatter.FormatDiscardedLog(discarded, incoming));
            PendingOverflowChanged?.Invoke();
        }

        private CountedThing<ItemDefinition> FirstPendingOverflow() =>
            HasPendingOverflow
                ? _pendingOverflow[0]
                : throw new InvalidOperationException("No find is waiting for room on the ground.");
    }
}
