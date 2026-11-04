using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// Every item, zone feature and zone the game's content files define, as the immutable definitions the rest of
    /// Core uses. Built once from the files by <see cref="GameContentCatalogBuilder"/>
    /// (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public sealed class GameContentCatalog
    {
        private readonly Dictionary<string, ZoneDefinition> _zonesById = new Dictionary<string, ZoneDefinition>(StringComparer.Ordinal);

        /// <summary>Creates a catalog. Zone ids must be unique.</summary>
        public GameContentCatalog(
            IEnumerable<ItemDefinition> items,
            IEnumerable<ZoneFeatureDefinition> zoneFeatures,
            IEnumerable<ZoneDefinition> zones)
        {
            Items = new List<ItemDefinition>(items ?? throw new ArgumentNullException(nameof(items)));
            ZoneFeatures = new List<ZoneFeatureDefinition>(zoneFeatures ?? throw new ArgumentNullException(nameof(zoneFeatures)));
            Zones = new List<ZoneDefinition>(zones ?? throw new ArgumentNullException(nameof(zones)));
            foreach (ZoneDefinition zone in Zones)
            {
                _zonesById.Add(zone.Id, zone);
            }
        }

        /// <summary>Every item definition, in file order.</summary>
        public IReadOnlyList<ItemDefinition> Items { get; }

        /// <summary>Every zone feature definition, in file order.</summary>
        public IReadOnlyList<ZoneFeatureDefinition> ZoneFeatures { get; }

        /// <summary>Every zone definition, in the order the zone files were given.</summary>
        public IReadOnlyList<ZoneDefinition> Zones { get; }

        /// <summary>Finds the zone with the given id, or returns false when there is none.</summary>
        public bool TryGetZone(string zoneId, out ZoneDefinition zone) => _zonesById.TryGetValue(zoneId, out zone);
    }
}
