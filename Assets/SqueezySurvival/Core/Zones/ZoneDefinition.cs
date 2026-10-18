using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// The fixed description of a zone: its name, how long it takes to map, and what exploring it can turn up
    /// (Docs/Design/Systems/Zones.md, Docs/Design/Systems/Exploration.md). Immutable; the zone's changing state is a
    /// <see cref="Zone"/>.
    /// </summary>
    public sealed class ZoneDefinition
    {
        private readonly List<IReadOnlyList<ItemDefinition>> _scriptedFinds;

        /// <summary>Creates a zone definition.</summary>
        /// <param name="id">Stable identifier, never shown.</param>
        /// <param name="name">Name shown to the player: "Beach".</param>
        /// <param name="exploresToMap">Explores needed to map the zone fully.</param>
        /// <param name="findPool">Loose items exploring can find.</param>
        /// <param name="featurePool">Features exploring can reveal.</param>
        /// <param name="scriptedFinds">
        /// The fixed finds of the zone's first explores, one list per explore, in order. Empty for zones without
        /// scripted explores (Docs/Design/Systems/Exploration.md, First explores are scripted).
        /// </param>
        public ZoneDefinition(
            string id,
            string name,
            int exploresToMap,
            WeightedTable<ItemDefinition> findPool,
            WeightedTable<ZoneFeatureDefinition> featurePool,
            IEnumerable<IReadOnlyList<ItemDefinition>> scriptedFinds)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Cannot be empty.", nameof(name));
            }

            if (exploresToMap < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(exploresToMap), exploresToMap, "Must be at least 1.");
            }

            Id = id;
            Name = name;
            ExploresToMap = exploresToMap;
            FindPool = findPool ?? throw new ArgumentNullException(nameof(findPool));
            FeaturePool = featurePool ?? throw new ArgumentNullException(nameof(featurePool));
            _scriptedFinds = new List<IReadOnlyList<ItemDefinition>>(scriptedFinds ?? throw new ArgumentNullException(nameof(scriptedFinds)));
            foreach (IReadOnlyList<ItemDefinition> explore in _scriptedFinds)
            {
                if (explore == null || explore.Count == 0)
                {
                    throw new ArgumentException("Every scripted explore must find something.", nameof(scriptedFinds));
                }
            }
        }

        /// <summary>Stable identifier, never shown.</summary>
        public string Id { get; }

        /// <summary>Name shown to the player.</summary>
        public string Name { get; }

        /// <summary>Explores needed to map the zone fully.</summary>
        public int ExploresToMap { get; }

        /// <summary>Loose items exploring can find, with their weights.</summary>
        public WeightedTable<ItemDefinition> FindPool { get; }

        /// <summary>Features exploring can reveal, with their weights.</summary>
        public WeightedTable<ZoneFeatureDefinition> FeaturePool { get; }

        /// <summary>The fixed finds of the first explores, one list per explore.</summary>
        public IReadOnlyList<IReadOnlyList<ItemDefinition>> ScriptedFinds => _scriptedFinds;
    }
}
