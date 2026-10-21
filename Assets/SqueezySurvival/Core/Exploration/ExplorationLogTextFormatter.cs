using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>Writes the event log entries for an explore (Docs/Design/UI/HUD.md, Event log).</summary>
    public static class ExplorationLogTextFormatter
    {
        /// <summary>
        /// The entries for one explore, in order: what was found, any feature revealed, and a line when the explore
        /// mapped the zone fully.
        /// </summary>
        public static IReadOnlyList<EventLogMessage> Format(string zoneName, ExplorationOutcome outcome)
        {
            if (string.IsNullOrWhiteSpace(zoneName))
            {
                throw new ArgumentException("Cannot be empty.", nameof(zoneName));
            }

            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            // A find that carries a warning turns the whole entry into a warning, with the warning in its words too.
            var warnings = new List<string>();
            foreach (ItemDefinition item in outcome.ItemsFound)
            {
                if (item.FoundWarning.Length > 0 && !warnings.Contains(item.FoundWarning))
                {
                    warnings.Add(item.FoundWarning);
                }
            }

            string finds = $"You explored the {zoneName} and found {CountedThingTextFormatter.FormatSentencePhrase(outcome.ItemsFound)}.";
            var entries = new List<EventLogMessage>
            {
                warnings.Count == 0
                    ? new EventLogMessage(finds)
                    : new EventLogMessage($"{finds} {string.Join(" ", warnings)}", EventLogSeverity.Warning),
            };

            if (outcome.FeaturesFound.Count > 0)
            {
                entries.Add(new EventLogMessage($"You came across {CountedThingTextFormatter.FormatSentencePhrase(outcome.FeaturesFound)}."));
            }

            if (outcome.CompletedTheZone)
            {
                entries.Add(new EventLogMessage($"You have explored every corner of the {zoneName}, though there is always more to find."));
            }

            return entries;
        }
    }
}
