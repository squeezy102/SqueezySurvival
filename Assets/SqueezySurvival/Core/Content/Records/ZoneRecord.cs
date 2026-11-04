using System.Collections.Generic;

namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>One zone as its file under Zones/ writes it. A file format, see <see cref="CountableThingRecord"/>.</summary>
    public sealed class ZoneRecord
    {
        /// <summary>Stable identifier, never shown.</summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>Name shown to the player: "Beach".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Explores needed to map the zone fully.</summary>
        public int ExploresToMap { get; set; }

        /// <summary>Loose items exploring can find, by item id.</summary>
        public List<ZonePoolEntryRecord> FindPool { get; set; } = new List<ZonePoolEntryRecord>();

        /// <summary>Features exploring can reveal, by feature id.</summary>
        public List<ZonePoolEntryRecord> FeaturePool { get; set; } = new List<ZonePoolEntryRecord>();

        /// <summary>The fixed finds of the first explores, one list of item ids per explore, in order.</summary>
        public List<List<string>> ScriptedExplores { get; set; } = new List<List<string>>();
    }
}
