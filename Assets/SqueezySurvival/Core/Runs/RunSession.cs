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
    /// carry, and the event log. Time advances only through actions (Docs/Design/Systems/Time.md, Rules).
    /// </summary>
    public sealed class RunSession
    {
        /// <summary>The first event log entry of every run.</summary>
        public const string WakeUpText = "You wake up on a beach. You have no idea how you got here.";

        private readonly ZoneExplorer _zoneExplorer;
        private readonly List<CountedThing<ItemDefinition>> _pendingGroundOverflow = new List<CountedThing<ItemDefinition>>();

        /// <summary>Starts a run from its configuration: Day 1, 6:00 AM, in the starting zone.</summary>
        public RunSession(
            NewRunConfiguration configuration,
            DayPhaseSchedule dayPhaseSchedule,
            ZoneDefinition startingZone,
            ZoneExplorer zoneExplorer)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            _zoneExplorer = zoneExplorer ?? throw new ArgumentNullException(nameof(zoneExplorer));
            Clock = GameClock.RunStart;
            CurrentZone = new Zone(startingZone ?? throw new ArgumentNullException(nameof(startingZone)));
            EventLog = new RunEventLog();
            EventLog.Add(Clock, WakeUpText);
        }

        /// <summary>Raised after the clock advances.</summary>
        public event Action<GameClock> ClockAdvanced;

        /// <summary>Raised after finds are queued for, or removed from, <see cref="PendingGroundOverflow"/>.</summary>
        public event Action PendingGroundOverflowChanged;

        /// <summary>The choices the run was started with. Never changes.</summary>
        public NewRunConfiguration Configuration { get; }

        /// <summary>Phase boundaries used to name the time of day.</summary>
        public DayPhaseSchedule DayPhaseSchedule { get; }

        /// <summary>The current game time.</summary>
        public GameClock Clock { get; private set; }

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
            Clock = Clock.AdvanceMinutes(TimePassingActions.DurationMinutes(action));
            EventLog.Add(Clock, TimePassingActions.LogText(action));
            ClockAdvanced?.Invoke(Clock);
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

            Clock = Clock.AdvanceMinutes(ExploreMinutes);
            ExplorationOutcome outcome = _zoneExplorer.Explore(CurrentZone);
            foreach (EventLogMessage message in ExplorationLogTextFormatter.Format(CurrentZone.Name, outcome))
            {
                EventLog.Add(Clock, message.Text, message.Severity);
            }

            _pendingGroundOverflow.AddRange(outcome.GroundOverflow);
            ClockAdvanced?.Invoke(Clock);
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
            Clock = Clock.AdvanceSeconds(preview.Seconds);
            EventLog.Add(Clock, ItemMoveTextFormatter.FormatLog(preview));
            ClockAdvanced?.Invoke(Clock);
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
