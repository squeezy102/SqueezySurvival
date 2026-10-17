using System;
using System.Collections.Generic;
using System.Text;

namespace SqueezySurvival.Core.Items
{
    /// <summary>Writes counted things for lists ("Stone (2)") and sentences ("2 stones and a stick").</summary>
    public static class CountedThingTextFormatter
    {
        /// <summary>A list line: the name, followed by the count when there is more than one. "Stick", "Stone (2)".</summary>
        public static string FormatListLine<TDefinition>(CountedEntry<TDefinition> entry)
            where TDefinition : class, ICountableThingDefinition
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return entry.Count == 1 ? entry.Definition.Name : $"{entry.Definition.Name} ({entry.Count})";
        }

        /// <summary>
        /// A phrase for a sentence, grouping repeats in the order each kind first appears:
        /// "a stone", "2 stones and a stick", "a stone, a stick and a seashell". Empty when there is nothing.
        /// </summary>
        public static string FormatSentencePhrase(IEnumerable<ICountableThingDefinition> things)
        {
            if (things == null)
            {
                throw new ArgumentNullException(nameof(things));
            }

            var kinds = new List<ICountableThingDefinition>();
            var counts = new List<int>();
            foreach (ICountableThingDefinition thing in things)
            {
                int index = kinds.FindIndex(kind => kind.Id == thing.Id);
                if (index < 0)
                {
                    kinds.Add(thing);
                    counts.Add(1);
                }
                else
                {
                    counts[index]++;
                }
            }

            var phrase = new StringBuilder();
            for (int index = 0; index < kinds.Count; index++)
            {
                if (index > 0)
                {
                    phrase.Append(index == kinds.Count - 1 ? " and " : ", ");
                }

                phrase.Append(counts[index] == 1 ? kinds[index].SingularPhrase : $"{counts[index]} {kinds[index].PluralName}");
            }

            return phrase.ToString();
        }
    }
}
