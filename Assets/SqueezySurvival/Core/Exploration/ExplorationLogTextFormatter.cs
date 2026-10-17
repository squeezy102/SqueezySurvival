using System;
using System.Collections.Generic;
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
        public static IReadOnlyList<string> Format(string zoneName, ExplorationOutcome outcome)
        {
            if (string.IsNullOrWhiteSpace(zoneName))
            {
                throw new ArgumentException("Cannot be empty.", nameof(zoneName));
            }

            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }

            var entries = new List<string>
            {
                $"You explored the {zoneName} and found {CountedThingTextFormatter.FormatSentencePhrase(outcome.ItemsFound)}.",
            };

            if (outcome.FeaturesFound.Count > 0)
            {
                entries.Add($"You came across {CountedThingTextFormatter.FormatSentencePhrase(outcome.FeaturesFound)}.");
            }

            if (outcome.CompletedTheZone)
            {
                entries.Add($"You have explored every corner of the {zoneName}, though there is always more to find.");
            }

            return entries;
        }
    }
}
