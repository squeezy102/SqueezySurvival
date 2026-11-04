using System.Collections.Generic;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Content
{
    /// <summary>Checks the records of Items.json and makes an <see cref="ItemDefinition"/> of each valid one.</summary>
    internal static class ItemRecordConverter
    {
        public static ContentDefinitionRegistry<ItemDefinition> Convert(ItemsFileRecord file, GameContentProblems problems) =>
            CountableThingRecordConverter.Convert(
                GameContentFileNames.Items,
                "item",
                file.Items ?? new List<ItemRecord>(),
                problems,
                (record, subject) => CheckItemFields(record, subject, problems),
                record => new ItemDefinition(
                    record.Id,
                    record.Name,
                    record.SingularPhrase,
                    record.PluralName,
                    record.Description,
                    record.Category.Value,
                    record.WeightPounds,
                    record.FoundWarning ?? string.Empty));

        private static void CheckItemFields(ItemRecord record, string subject, GameContentProblems problems)
        {
            ContentRecordChecks.RequireKnownValue(problems, GameContentFileNames.Items, subject, "category", record.Category);
            if (record.WeightPounds <= 0m)
            {
                problems.Add(GameContentFileNames.Items, subject, $"weight must be more than 0 (it is {ContentRecordChecks.Number(record.WeightPounds)})");
            }
        }
    }
}
