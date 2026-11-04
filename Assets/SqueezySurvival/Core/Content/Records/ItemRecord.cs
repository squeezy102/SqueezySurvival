using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>One item as Items.json writes it. A file format, see <see cref="CountableThingRecord"/>.</summary>
    public sealed class ItemRecord : CountableThingRecord
    {
        /// <summary>What kind of thing it is, written by name. Null when the file leaves it out.</summary>
        public ItemCategory? Category { get; set; }

        /// <summary>How much one weighs, in pounds.</summary>
        public decimal WeightPounds { get; set; }

        /// <summary>A warning added to the event log whenever one is found, or empty for none.</summary>
        public string FoundWarning { get; set; } = string.Empty;
    }
}
