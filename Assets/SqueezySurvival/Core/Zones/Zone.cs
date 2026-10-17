using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// A zone's state during a run: how much is explored, what lies on the ground and which features have been found
    /// (Docs/Design/Systems/Zones.md).
    /// </summary>
    public sealed class Zone
    {
        /// <summary>Creates an unexplored zone with nothing on the ground.</summary>
        public Zone(ZoneDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            ExplorationProgress = new ExplorationProgress(0, definition.ExploresToMap);
        }

        /// <summary>Raised after the exploration progress changes.</summary>
        public event Action<ExplorationProgress> ExplorationProgressChanged;

        /// <summary>What the zone is.</summary>
        public ZoneDefinition Definition { get; }

        /// <summary>Name shown to the player.</summary>
        public string Name => Definition.Name;

        /// <summary>How much of the zone has been explored.</summary>
        public ExplorationProgress ExplorationProgress { get; private set; }

        /// <summary>Items lying on the ground here (Docs/Design/Systems/Zones.md, Ground pool).</summary>
        public CountedCollection<ItemDefinition> GroundPool { get; } = new CountedCollection<ItemDefinition>();

        /// <summary>Features found here: resource nodes and other permanent or semi-permanent fixtures.</summary>
        public CountedCollection<ZoneFeatureDefinition> Features { get; } = new CountedCollection<ZoneFeatureDefinition>();

        /// <summary>Counts one more explore of the zone.</summary>
        public void RecordExplore()
        {
            ExplorationProgress = ExplorationProgress.AfterOneExplore();
            ExplorationProgressChanged?.Invoke(ExplorationProgress);
        }
    }
}
