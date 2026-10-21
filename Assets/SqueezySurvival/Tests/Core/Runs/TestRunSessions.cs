using System;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Randomness;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    /// <summary>Starts runs for tests through <see cref="RunSessionFactory"/>, the same path the game uses.</summary>
    public static class TestRunSessions
    {
        /// <summary>A run in the test Cove, drawing the given random numbers.</summary>
        public static RunSession Start(params int[] randomNumbers) =>
            Start(TestZoneDefinitions.Create(4), Array.Empty<ITimeDependentSystem>(), randomNumbers);

        /// <summary>A run in <paramref name="startingZone"/>, telling <paramref name="timeDependentSystems"/> of each advance.</summary>
        public static RunSession Start(ZoneDefinition startingZone, ITimeDependentSystem[] timeDependentSystems, params int[] randomNumbers) =>
            RunSessionFactory.StartNewRun(
                new NewRunConfiguration(DominantHand.Right, RunOptions.Standard),
                DayPhaseSchedule.Default,
                startingZone,
                ExplorationRules.Wireframe,
                new QueuedRandomNumberSource(randomNumbers),
                timeDependentSystems);

        /// <summary>A run in the test Cove with <paramref name="count"/> of <paramref name="definition"/> on the ground.</summary>
        public static RunSession WithOnTheGround(ItemDefinition definition, int count, params ITimeDependentSystem[] timeDependentSystems)
        {
            RunSession session = Start(TestZoneDefinitions.Create(4), timeDependentSystems);
            session.GroundInventory.Ground.Receive(definition, count, -1);
            return session;
        }

        /// <summary>
        /// A run whose ground is full and whose first explore found a pebble with no room, so the pebble waits
        /// (one find, a pebble, and no feature).
        /// </summary>
        public static RunSession WithPebbleWaitingForRoom()
        {
            RunSession session = Start(1, 0, 99);
            session.GroundInventory.Ground.AddRange(TestZoneDefinitions.DistinctItems(session.GroundInventory.Ground.SlotCapacity));
            session.ZoneExploration.Explore();
            return session;
        }

        /// <summary>The slot <paramref name="definition"/> is in, or would go to, in <paramref name="collection"/>.</summary>
        public static int SlotOf(CountedCollection<ItemDefinition> collection, ItemDefinition definition) =>
            collection.SlotToReceive(definition, -1);
    }
}
