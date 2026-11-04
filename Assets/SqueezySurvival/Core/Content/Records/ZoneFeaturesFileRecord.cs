using System.Collections.Generic;

namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>ZoneFeatures.json: every zone feature definition. A file format, see <see cref="CountableThingRecord"/>.</summary>
    public sealed class ZoneFeaturesFileRecord
    {
        /// <summary>Every zone feature, in file order.</summary>
        public List<ZoneFeatureRecord> ZoneFeatures { get; set; } = new List<ZoneFeatureRecord>();
    }
}
