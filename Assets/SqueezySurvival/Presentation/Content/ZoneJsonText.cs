using System;

namespace SqueezySurvival.Presentation.Content
{
    /// <summary>The text of one zone file and the file's name without its extension: "Beach" for Zones/Beach.json.</summary>
    public sealed class ZoneJsonText
    {
        /// <summary>Creates the text of a zone file.</summary>
        public ZoneJsonText(string fileNameWithoutExtension, string json)
        {
            FileNameWithoutExtension = fileNameWithoutExtension ?? throw new ArgumentNullException(nameof(fileNameWithoutExtension));
            Json = json ?? throw new ArgumentNullException(nameof(json));
        }

        /// <summary>The file's name without its extension.</summary>
        public string FileNameWithoutExtension { get; }

        /// <summary>The file's JSON text.</summary>
        public string Json { get; }
    }
}
