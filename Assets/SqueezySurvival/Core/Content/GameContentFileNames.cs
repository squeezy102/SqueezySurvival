namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// The names of the content files under Assets/SqueezySurvival/Content/, as problems report them
    /// (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public static class GameContentFileNames
    {
        /// <summary>The file holding every item definition.</summary>
        public const string Items = "Items.json";

        /// <summary>The file holding every zone feature definition.</summary>
        public const string ZoneFeatures = "ZoneFeatures.json";

        /// <summary>The name of a zone's file, given the file's name without its extension: "Beach" is "Zones/Beach.json".</summary>
        public static string ForZone(string zoneFileNameWithoutExtension) => "Zones/" + zoneFileNameWithoutExtension + ".json";
    }
}
