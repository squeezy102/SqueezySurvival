using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Content.Records;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// Checks the zone files, resolves the item and feature ids each names, and makes a <see cref="ZoneDefinition"/>
    /// of each valid one.
    /// </summary>
    internal static class ZoneRecordConverter
    {
        public static List<ZoneDefinition> Convert(
            IReadOnlyList<ParsedZoneFile> zoneFiles,
            ContentDefinitionRegistry<ItemDefinition> items,
            ContentDefinitionRegistry<ZoneFeatureDefinition> features,
            GameContentProblems problems)
        {
            var zones = new List<ZoneDefinition>();
            var fileOfEachZoneId = new Dictionary<string, string>(StringComparer.Ordinal);
            for (int position = 0; position < zoneFiles.Count; position++)
            {
                ParsedZoneFile zoneFile = zoneFiles[position];
                ZoneRecord record = zoneFile.Zone;
                string subject = ContentRecordChecks.Subject("zone", record.Id, position);
                int problemsBefore = problems.Count;

                if (string.IsNullOrWhiteSpace(record.Id))
                {
                    problems.Add(zoneFile.FileName, subject, "id is empty");
                }
                else if (fileOfEachZoneId.TryGetValue(record.Id, out string firstFileName))
                {
                    problems.Add(zoneFile.FileName, subject, $"id is already used by {firstFileName}");
                }
                else
                {
                    fileOfEachZoneId.Add(record.Id, zoneFile.FileName);
                }

                ContentRecordChecks.RequireText(problems, zoneFile.FileName, subject, "name", record.Name);
                if (record.ExploresToMap < 1)
                {
                    problems.Add(zoneFile.FileName, subject, $"explores to map must be at least 1 (it is {ContentRecordChecks.Number(record.ExploresToMap)})");
                }

                List<WeightedTableEntry<ItemDefinition>> findPool = ResolvePool(zoneFile.FileName, subject + " find pool", "item", record.FindPool, items, problems);
                List<WeightedTableEntry<ZoneFeatureDefinition>> featurePool = ResolvePool(zoneFile.FileName, subject + " feature pool", "feature", record.FeaturePool, features, problems);
                List<IReadOnlyList<ItemDefinition>> scriptedFinds = ResolveScriptedExplores(zoneFile.FileName, subject, record.ScriptedExplores, items, problems);

                // A pool or explore that could not be fully resolved has already been reported, or names an item
                // whose own problem was, so the zone is not built.
                if (problems.Count == problemsBefore && findPool != null && featurePool != null && scriptedFinds != null)
                {
                    zones.Add(new ZoneDefinition(
                        record.Id,
                        record.Name,
                        record.ExploresToMap,
                        new WeightedTable<ItemDefinition>(findPool),
                        new WeightedTable<ZoneFeatureDefinition>(featurePool),
                        scriptedFinds));
                }
            }

            return zones;
        }

        // Returns null when the pool is not complete: empty, or with an entry that could not be resolved.
        private static List<WeightedTableEntry<TDefinition>> ResolvePool<TDefinition>(
            string fileName,
            string subject,
            string kind,
            List<ZonePoolEntryRecord> entries,
            ContentDefinitionRegistry<TDefinition> registry,
            GameContentProblems problems)
            where TDefinition : class
        {
            if (entries == null || entries.Count == 0)
            {
                problems.Add(fileName, subject, "is empty");
                return null;
            }

            var resolved = new List<WeightedTableEntry<TDefinition>>();
            bool complete = true;
            for (int position = 0; position < entries.Count; position++)
            {
                ZonePoolEntryRecord entry = entries[position];
                if (entry == null || string.IsNullOrWhiteSpace(entry.Id))
                {
                    problems.Add(fileName, subject, $"entry {position + 1} has no id");
                    complete = false;
                    continue;
                }

                if (entry.Weight < 1)
                {
                    problems.Add(fileName, subject, $"{kind} '{entry.Id}' has weight {ContentRecordChecks.Number(entry.Weight)}, which must be at least 1");
                    complete = false;
                }

                if (!registry.IsDeclared(entry.Id))
                {
                    problems.Add(fileName, subject, $"unknown {kind} id '{entry.Id}'");
                    complete = false;
                }
                else if (!registry.TryGetValid(entry.Id, out TDefinition definition))
                {
                    // The record that declares it is reported on its own.
                    complete = false;
                }
                else if (entry.Weight >= 1)
                {
                    resolved.Add(new WeightedTableEntry<TDefinition>(definition, entry.Weight));
                }
            }

            return complete ? resolved : null;
        }

        // Returns null when an explore could not be fully resolved.
        private static List<IReadOnlyList<ItemDefinition>> ResolveScriptedExplores(
            string fileName,
            string zoneSubject,
            List<List<string>> scriptedExplores,
            ContentDefinitionRegistry<ItemDefinition> items,
            GameContentProblems problems)
        {
            var resolved = new List<IReadOnlyList<ItemDefinition>>();
            bool complete = true;
            List<List<string>> explores = scriptedExplores ?? new List<List<string>>();
            for (int explorePosition = 0; explorePosition < explores.Count; explorePosition++)
            {
                string subject = $"{zoneSubject} scripted explore {explorePosition + 1}";
                List<string> itemIds = explores[explorePosition];
                if (itemIds == null || itemIds.Count == 0)
                {
                    problems.Add(fileName, subject, "is empty");
                    complete = false;
                    continue;
                }

                var finds = new List<ItemDefinition>();
                foreach (string itemId in itemIds)
                {
                    if (string.IsNullOrWhiteSpace(itemId) || !items.IsDeclared(itemId))
                    {
                        problems.Add(fileName, subject, $"unknown item id '{itemId}'");
                        complete = false;
                    }
                    else if (items.TryGetValid(itemId, out ItemDefinition item))
                    {
                        finds.Add(item);
                    }
                    else
                    {
                        complete = false;
                    }
                }

                resolved.Add(finds);
            }

            return complete ? resolved : null;
        }
    }
}
