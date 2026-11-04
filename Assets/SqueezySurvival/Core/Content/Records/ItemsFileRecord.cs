using System.Collections.Generic;

namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>Items.json: every item definition. A file format, see <see cref="CountableThingRecord"/>.</summary>
    public sealed class ItemsFileRecord
    {
        /// <summary>Every item, in file order.</summary>
        public List<ItemRecord> Items { get; set; } = new List<ItemRecord>();
    }
}
