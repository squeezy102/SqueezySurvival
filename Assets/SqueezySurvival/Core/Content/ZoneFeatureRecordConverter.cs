using System.Collections.Generic;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Content
{
    /// <summary>Checks the records of ZoneFeatures.json and makes a <see cref="ZoneFeatureDefinition"/> of each valid one.</summary>
    internal static class ZoneFeatureRecordConverter
    {
        public static ContentDefinitionRegistry<ZoneFeatureDefinition> Convert(ZoneFeaturesFileRecord file, GameContentProblems problems) =>
            CountableThingRecordConverter.Convert(
                GameContentFileNames.ZoneFeatures,
                "feature",
                file.ZoneFeatures ?? new List<ZoneFeatureRecord>(),
                problems,
                (record, subject) => ContentRecordChecks.RequireKnownValue(problems, GameContentFileNames.ZoneFeatures, subject, "permanence", record.Permanence),
                record => new ZoneFeatureDefinition(
                    record.Id,
                    record.Name,
                    record.SingularPhrase,
                    record.PluralName,
                    record.Description,
                    record.Permanence.Value));
    }
}
