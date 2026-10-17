using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SqueezySurvival.Core.Items
{
    /// <summary>Writes counted things for grid cells (a count badge) and sentences ("2 stones and a stick").</summary>
    public static class CountedThingTextFormatter
    {
        /// <summary>
        /// The count shown in the corner of a grid cell: the number when there are 2 or more, empty for a single
        /// thing (Docs/Design/UI/GridPanels.md, The grid).
        /// </summary>
        public static string FormatCountBadge(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be at least 1.");
            }

            return count == 1 ? string.Empty : count.ToString(CultureInfo.InvariantCulture);
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
