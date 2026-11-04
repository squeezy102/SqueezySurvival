using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>One zone feature as ZoneFeatures.json writes it. A file format, see <see cref="CountableThingRecord"/>.</summary>
    public sealed class ZoneFeatureRecord : CountableThingRecord
    {
        /// <summary>How long the feature lasts, written by name. Null when the file leaves it out.</summary>
        public ZoneFeaturePermanence? Permanence { get; set; }
    }
}
