namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>One entry of a zone's find or feature pool: the id of an item or feature and its weight.</summary>
    public sealed class ZonePoolEntryRecord
    {
        /// <summary>The id of the item or feature.</summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>How likely it is, relative to the pool's other entries.</summary>
        public int Weight { get; set; }
    }
}
