using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// Builds the <see cref="GameContentCatalog"/> from the records the content files were parsed into. Checks every
    /// record and every id a zone names, and reports every problem at once, each naming its file and id, so one pass
    /// of fixes covers them all (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public static class GameContentCatalogBuilder
    {
        /// <summary>Builds the catalog. Throws <see cref="GameContentValidationException"/> listing every problem found.</summary>
        /// <param name="itemsFile">Items.json.</param>
        /// <param name="zoneFeaturesFile">ZoneFeatures.json.</param>
        /// <param name="zoneFiles">Every file under Zones/.</param>
        public static GameContentCatalog Build(
            ItemsFileRecord itemsFile,
            ZoneFeaturesFileRecord zoneFeaturesFile,
            IReadOnlyList<ParsedZoneFile> zoneFiles)
        {
            if (itemsFile == null)
            {
                throw new ArgumentNullException(nameof(itemsFile));
            }

            if (zoneFeaturesFile == null)
            {
                throw new ArgumentNullException(nameof(zoneFeaturesFile));
            }

            if (zoneFiles == null)
            {
                throw new ArgumentNullException(nameof(zoneFiles));
            }

            var problems = new GameContentProblems();
            ContentDefinitionRegistry<ItemDefinition> items = ItemRecordConverter.Convert(itemsFile, problems);
            ContentDefinitionRegistry<ZoneFeatureDefinition> features = ZoneFeatureRecordConverter.Convert(zoneFeaturesFile, problems);
            List<ZoneDefinition> zones = ZoneRecordConverter.Convert(zoneFiles, items, features, problems);

            if (problems.Count > 0)
            {
                throw problems.ToException();
            }

            return new GameContentCatalog(items.Definitions, features.Definitions, zones);
        }
    }
}
