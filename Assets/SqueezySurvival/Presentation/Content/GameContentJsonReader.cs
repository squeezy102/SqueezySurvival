using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.Content.Records;

namespace SqueezySurvival.Presentation.Content
{
    /// <summary>
    /// Parses the content files' JSON text into records, strictly: a key that matches no record property is an error,
    /// and an enum must be written by a name the game knows, never a number
    /// (Docs/Decisions/ADR-0009-Data-Driven-Content.md, Presentation parses with Newtonsoft Json). It checks only the
    /// file's shape; the Core builder checks the values.
    /// </summary>
    public sealed class GameContentJsonReader
    {
        private readonly JsonSerializerSettings _strictSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            Converters = { new StringEnumConverter { AllowIntegerValues = false } },
        };

        /// <summary>Reads the text of Items.json.</summary>
        public ItemsFileRecord ReadItems(string json) => Read<ItemsFileRecord>(GameContentFileNames.Items, json);

        /// <summary>Reads the text of ZoneFeatures.json.</summary>
        public ZoneFeaturesFileRecord ReadZoneFeatures(string json) =>
            Read<ZoneFeaturesFileRecord>(GameContentFileNames.ZoneFeatures, json);

        /// <summary>Reads the text of one zone file, given the file's name without its extension: "Beach" for Zones/Beach.json.</summary>
        public ParsedZoneFile ReadZone(string fileNameWithoutExtension, string json)
        {
            string fileName = GameContentFileNames.ForZone(fileNameWithoutExtension);
            return new ParsedZoneFile(fileName, Read<ZoneRecord>(fileName, json));
        }

        private T Read<T>(string fileName, string json)
            where T : class
        {
            T record;
            try
            {
                record = JsonConvert.DeserializeObject<T>(json ?? string.Empty, _strictSettings);
            }
            catch (JsonException exception)
            {
                throw new GameContentFormatException(fileName, exception.Message, exception);
            }

            return record ?? throw new GameContentFormatException(fileName, "the file is empty.");
        }
    }
}
