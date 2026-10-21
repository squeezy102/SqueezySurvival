using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The state of a run being played: its fixed configuration, the game clock, where the survivor is, what they
    /// carry, and the event log. Every action advances time through the injected <see cref="IGameTimeAdvancer"/>
    /// (Docs/Engineering/Architecture.md, Time: one path).
    /// </summary>
    public sealed class RunSession
    {
        /// <summary>The first event log entry of every run.</summary>
        public const string WakeUpText = "You wake up on a beach. You have no idea how you got here.";

        private readonly IGameTimeAdvancer _gameTimeAdvancer;
        private readonly ZoneExplorer _zoneExplorer;
        private readonly List<CountedThing<ItemDefinition>> _pendingGroundOverflow = new List<CountedThing<ItemDefinition>>();

        /// <summary>
        /// Starts a run from its configuration, in the starting zone, at the game time's current clock
        /// (<see cref="GameClock.RunStart"/> for a new run).
        /// </summary>
        public RunSession(
            NewRunConfiguration configuration,
            DayPhaseSchedule dayPhaseSchedule,
            IGameTimeAdvancer gameTimeAdvancer,
            ZoneDefinition startingZone,
            ZoneExplorer zoneExplorer)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            _gameTimeAdvancer = gameTimeAdvancer ?? throw new ArgumentNullException(nameof(gameTimeAdvancer));
            _zoneExplorer = zoneExplorer ?? throw new ArgumentNullException(nameof(zoneExplorer));
            CurrentZone = new Zone(startingZone ?? throw new ArgumentNullException(nameof(startingZone)));
            EventLog = new RunEventLog();
            EventLog.Add(Clock, WakeUpText);
        }

        /// <summary>Raised after finds are queued for, or removed from, <see cref="PendingGroundOverflow"/>.</summary>
        public event Action PendingGroundOverflowChanged;

        /// <summary>The choices the run was started with. Never changes.</summary>
        public NewRunConfiguration Configuration { get; }

        /// <summary>Phase boundaries used to name the time of day.</summary>
        public DayPhaseSchedule DayPhaseSchedule { get; }

        /// <summary>The run's game time, for reading the clock and hearing when it advances.</summary>
        public IGameTime GameTime => _gameTimeAdvancer;

        /// <summary>The current game time.</summary>
        public GameClock Clock => _gameTimeAdvancer.Clock;

        /// <summary>The zone the survivor is in.</summary>
        public Zone CurrentZone { get; }

        /// <summary>What the survivor carries (Docs/Design/Systems/Inventory.md, Slots).</summary>
        public CountedCollection<ItemDefinition> Inventory { get; } =
            new CountedCollection<ItemDefinition>(InventoryGridShapes.Inventory.SlotCount);

        /// <summary>The tool hand and offhand (Docs/Design/Systems/Equipment.md).</summary>
        public CountedCollection<ItemDefinition> Hands { get; } =
            new CountedCollection<ItemDefinition>(InventoryGridShapes.Hands.SlotCount);

        /// <summary>
        /// What the survivor carries, the inventory and both hands, against their weight limit
        /// (Docs/Design/Systems/Inventory.md, Load).
        /// </summary>
        public SurvivorLoad Load => new SurvivorLoad(WeightOf(Inventory) + WeightOf(Hands), SurvivorLoad.PlaceholderLimitPounds);

        /// <summary>
        /// Whether the survivor can explore: not while a find waits for room on the ground, and not while immobile
        /// (Docs/Design/Systems/Inventory.md, Load).
        /// </summary>
        public bool CanExplore => !HasPendingGroundOverflow && Load.State != SurvivorLoadState.Immobile;

        /// <summary>How long one explore takes, in minutes.</summary>
        public int ExploreMinutes => _zoneExplorer.Rules.ExploreMinutes;

        /// <summary>Everything that has happened this run.</summary>
        public RunEventLog EventLog { get; }

        /// <summary>The current day phase.</summary>
        public DayPhase CurrentDayPhase => DayPhaseSchedule.PhaseAt(Clock.MinuteOfDay);

        /// <summary>
        /// Finds with no room on the ground, waiting for the player to choose what to throw away, first one first
        /// (Docs/Design/UI/InventoryGrids.md, When the ground is full). While any wait, no time-taking action can be taken.
        /// </summary>
        public IReadOnlyList<CountedThing<ItemDefinition>> PendingGroundOverflow => _pendingGroundOverflow;

        /// <summary>Whether a find is waiting for the player to choose what to throw away.</summary>
        public bool HasPendingGroundOverflow => _pendingGroundOverflow.Count > 0;

        /// <summary>Performs a time-passing action. Not available while a find waits for room on the ground.</summary>
        public void PassTime(TimePassingAction action)
        {
            RequireNoPendingGroundOverflow();
            CompleteAction(
                ActionDuration.FromMinutes(TimePassingActions.DurationMinutes(action)),
                new[] { new EventLogMessage(TimePassingActions.LogText(action)) });
        }

        /// <summary>
        /// Explores the current zone: takes the explore's time, adds what was found to the zone and logs it
        /// (Docs/Design/Systems/Exploration.md). Finds with no room on the ground wait in
        /// <see cref="PendingGroundOverflow"/>. Not available while a find already waits.
        /// </summary>
        public ExplorationOutcome Explore()
        {
            RequireNoPendingGroundOverflow();
            if (Load.State == SurvivorLoadState.Immobile)
            {
                throw new InvalidOperationException("The survivor carries too much to move.");
            }

            ExplorationOutcome outcome = _zoneExplorer.Explore(CurrentZone);
            _pendingGroundOverflow.AddRange(outcome.GroundOverflow);
            CompleteAction(
                ActionDuration.FromMinutes(ExploreMinutes),
                ExplorationLogTextFormatter.Format(CurrentZone.Name, outcome));
            if (outcome.GroundOverflow.Count > 0)
            {
                PendingGroundOverflowChanged?.Invoke();
            }

            return outcome;
        }

        /// <summary>Leaves the first waiting find behind, gone for good, and logs it. Takes no time.</summary>
        public void LeavePendingGroundOverflowBehind()
        {
            CountedThing<ItemDefinition> incoming = FirstPendingGroundOverflow();
            _pendingGroundOverflow.RemoveAt(0);
            EventLog.Add(Clock, GroundOverflowTextFormatter.FormatLeftBehindLog(incoming));
            PendingGroundOverflowChanged?.Invoke();
        }

        /// <summary>
        /// Throws away what is in <paramref name="groundSlot"/>, gone for good, puts the first waiting find in its
        /// place, and logs it. Takes no time.
        /// </summary>
        public void DiscardToMakeRoomForPendingGroundOverflow(int groundSlot)
        {
            CountedThing<ItemDefinition> incoming = FirstPendingGroundOverflow();
            CountedEntry<ItemDefinition> discarded = CurrentZone.GroundPool.ReplaceSlot(groundSlot, incoming);
            _pendingGroundOverflow.RemoveAt(0);
            EventLog.Add(Clock, GroundOverflowTextFormatter.FormatDiscardedLog(discarded, incoming));
            PendingGroundOverflowChanged?.Invoke();
        }

        /// <summary>The slots of a panel things move between (Docs/Design/UI/DragAndDrop.md, Between panels).</summary>
        public CountedCollection<ItemDefinition> ContentsOf(ItemPanel panel) =>
            panel == ItemPanel.Inventory ? Inventory : CurrentZone.GroundPool;

        /// <summary>
        /// What moving <paramref name="quantity"/> of the stack in <paramref name="fromSlot"/> to the other panel would
        /// do, letting go over <paramref name="targetSlot"/> (-1 for no particular slot). Changes nothing.
        /// </summary>
        public ItemMovePreview PreviewItemMove(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            ItemPanel to = Other(from);
            CountedEntry<ItemDefinition> stack = ContentsOf(from).EntryAtSlot(fromSlot)
                ?? throw new ArgumentOutOfRangeException(nameof(fromSlot), fromSlot, "Nothing is in that slot.");
            CountedCollection<ItemDefinition> destination = ContentsOf(to);
            int count = ItemTransferQuantityCalculator.CountToMove(stack.Count, quantity);
            return new ItemMovePreview(
                stack.Definition,
                count,
                from,
                to,
                destination.SlotToReceive(stack.Definition, targetSlot),
                destination.CountOf(stack.Definition),
                ItemMoveTimeRules.SecondsToMove(stack.Definition.WeightPounds * count));
        }

        /// <summary>
        /// Moves things to the other panel as <see cref="PreviewItemMove"/> describes: takes its seconds and logs it.
        /// There is no weight cap. Not available while a find waits for room on the ground, or when there is no room.
        /// </summary>
        public void MoveItems(ItemPanel from, int fromSlot, int targetSlot, ItemTransferQuantity quantity)
        {
            RequireNoPendingGroundOverflow();
            ItemMovePreview preview = PreviewItemMove(from, fromSlot, targetSlot, quantity);
            if (!preview.CanMove)
            {
                throw new InvalidOperationException("There is no room there.");
            }

            ContentsOf(from).Take(fromSlot, preview.Count);
            ContentsOf(preview.To).Receive(preview.Definition, preview.Count, preview.DestinationSlot);
            CompleteAction(
                ActionDuration.FromSeconds(preview.Seconds),
                new[] { new EventLogMessage(ItemMoveTextFormatter.FormatLog(preview)) });
        }

        private static ItemPanel Other(ItemPanel panel) => panel == ItemPanel.Inventory ? ItemPanel.Ground : ItemPanel.Inventory;

        private static decimal WeightOf(CountedCollection<ItemDefinition> collection)
        {
            decimal pounds = 0m;
            foreach (CountedEntry<ItemDefinition> entry in collection.Entries)
            {
                pounds += entry.Definition.WeightPounds * entry.Count;
            }

            return pounds;
        }

        /// <summary>
        /// Finishes an action whose effects are applied: logs what happened, stamped with the time the action ends,
        /// then advances time. Logging first keeps the action's own entries ahead of anything time-dependent systems
        /// log about the time that passed.
        /// </summary>
        private void CompleteAction(ActionDuration duration, IEnumerable<EventLogMessage> messages)
        {
            GameClock endsAt = Clock.Advance(duration);
            foreach (EventLogMessage message in messages)
            {
                EventLog.Add(endsAt, message.Text, message.Severity);
            }

            _gameTimeAdvancer.Advance(duration);
        }

        private CountedThing<ItemDefinition> FirstPendingGroundOverflow() =>
            HasPendingGroundOverflow
                ? _pendingGroundOverflow[0]
                : throw new InvalidOperationException("No find is waiting for room on the ground.");

        private void RequireNoPendingGroundOverflow()
        {
            if (HasPendingGroundOverflow)
            {
                throw new InvalidOperationException("Choose what to do with the find that has no room on the ground first.");
            }
        }
    }
}
