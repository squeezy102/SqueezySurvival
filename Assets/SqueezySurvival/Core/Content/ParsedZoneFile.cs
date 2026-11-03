using System;
using SqueezySurvival.Core.Content.Records;

namespace SqueezySurvival.Core.Content
{
    /// <summary>One zone file after parsing: the file's name, for problems to report, and the zone it holds.</summary>
    public sealed class ParsedZoneFile
    {
        /// <summary>Creates a parsed zone file.</summary>
        public ParsedZoneFile(string fileName, ZoneRecord zone)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Zone = zone ?? throw new ArgumentNullException(nameof(zone));
        }

        /// <summary>The file's name, such as "Zones/Beach.json".</summary>
        public string FileName { get; }

        /// <summary>The zone the file holds.</summary>
        public ZoneRecord Zone { get; }
    }
}
