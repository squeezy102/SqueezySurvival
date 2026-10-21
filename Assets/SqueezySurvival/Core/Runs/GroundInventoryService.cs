using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The ground in the zone the survivor is in, and the finds waiting for room on it
    /// (Docs/Design/UI/InventoryGrids.md, When the ground is full). Each zone keeps its own ground; this service works
    /// on the current zone's.
    /// </summary>
    public sealed class GroundInventoryService
    {
        private readonly Zone _currentZone;
        private readonly IGameTime _gameTime;
        private readonly RunEventLog _eventLog;
        private readonly List<CountedThing<ItemDefinition>> _pendingOverflow = new List<CountedThing<ItemDefinition>>();

        /// <summary>Works on <paramref name="currentZone"/>'s ground, logging to <paramref name="eventLog"/>.</summary>
        public GroundInventoryService(Zone currentZone, IGameTime gameTime, RunEventLog eventLog)
        {
            _currentZone = currentZone ?? throw new ArgumentNullException(nameof(currentZone));
            _gameTime = gameTime ?? throw new ArgumentNullException(nameof(gameTime));
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
        }

        /// <summary>Raised after finds are queued in, or removed from, <see cref="PendingOverflow"/>.</summary>
        public event Action PendingOverflowChanged;

        /// <summary>What is on the ground in the current zone.</summary>
        public CountedCollection<ItemDefinition> Ground => _currentZone.GroundPool;

        /// <summary>
        /// Finds with no room on the ground, waiting for the player to choose what to throw away, first one first.
        /// While any wait, no time-taking action can be taken.
        /// </summary>
        public IReadOnlyList<CountedThing<ItemDefinition>> PendingOverflow => _pendingOverflow;

        /// <summary>Whether a find is waiting for the player to choose what to throw away.</summary>
        public bool HasPendingOverflow => _pendingOverflow.Count > 0;

        /// <summary>Queues finds that had no room on the ground. Does nothing for none.</summary>
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

        /// <summary>Leaves the first waiting find behind, gone for good, and logs it. Takes no time.</summary>
        public void LeavePendingOverflowBehind()
        {
            CountedThing<ItemDefinition> incoming = FirstPendingOverflow();
            _pendingOverflow.RemoveAt(0);
            _eventLog.Add(_gameTime.Clock, GroundOverflowTextFormatter.FormatLeftBehindLog(incoming));
            PendingOverflowChanged?.Invoke();
        }

        /// <summary>
        /// Throws away what is in <paramref name="groundSlot"/>, gone for good, puts the first waiting find in its
        /// place, and logs it. Takes no time.
        /// </summary>
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
