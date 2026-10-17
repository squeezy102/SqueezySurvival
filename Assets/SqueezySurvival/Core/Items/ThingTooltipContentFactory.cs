using System;
using System.Collections.Generic;
using System.Globalization;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Builds the tooltip for a grid cell: name and description for anything, plus the weight of one and of the
    /// whole stack for items (Docs/Design/UI/GridPanels.md, Tooltips).
    /// </summary>
    public static class ThingTooltipContentFactory
    {
        /// <summary>The tooltip for an entry of a counted collection.</summary>
        public static ThingTooltipContent Create<TDefinition>(CountedEntry<TDefinition> entry)
            where TDefinition : class, ICountableThingDefinition
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var details = new List<string>();
            if (entry.Definition is ItemDefinition item)
            {
                details.Add($"Weight: {WeightTextFormatter.FormatPounds(item.WeightPounds)} each");
                details.Add($"Stack of {entry.Count.ToString(CultureInfo.InvariantCulture)}: {WeightTextFormatter.FormatPounds(item.WeightPounds * entry.Count)}");
            }

            return new ThingTooltipContent(entry.Definition.Name, entry.Definition.Description, details);
        }
    }
}
