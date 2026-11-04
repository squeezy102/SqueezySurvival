using System;
using System.Collections.Generic;
using System.Linq;
using SqueezySurvival.Core.Content;

namespace SqueezySurvival.Presentation.Content
{
    /// <summary>
    /// Turns the content files' text into the game's content catalog: parses each file, then lets the Core builder check
    /// and build them together (Docs/Decisions/ADR-0009-Data-Driven-Content.md). Throws
    /// <see cref="GameContentFormatException"/> for a file that is not valid JSON of the expected shape, and
    /// <see cref="GameContentValidationException"/> for values the builder rejects.
    /// </summary>
    public sealed class GameContentCatalogLoader
    {
        private readonly GameContentJsonReader _reader;

        /// <summary>Creates a loader that parses with the given reader.</summary>
        public GameContentCatalogLoader(GameContentJsonReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>Parses and builds the content from the text of its files.</summary>
        public GameContentCatalog Load(string itemsJson, string zoneFeaturesJson, IReadOnlyList<ZoneJsonText> zoneFiles)
        {
            if (zoneFiles == null)
            {
                throw new ArgumentNullException(nameof(zoneFiles));
            }

            return GameContentCatalogBuilder.Build(
                _reader.ReadItems(itemsJson),
                _reader.ReadZoneFeatures(zoneFeaturesJson),
                zoneFiles.Select(zoneFile => _reader.ReadZone(zoneFile.FileNameWithoutExtension, zoneFile.Json)).ToList());
        }
    }
}
