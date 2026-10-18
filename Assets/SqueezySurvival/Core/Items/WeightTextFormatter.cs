using System;
using System.Globalization;

namespace SqueezySurvival.Core.Items
{
    /// <summary>Writes weights in pounds (Docs/Design/Systems/Inventory.md): "1 lb", "0.5 lb", "2.25 lb".</summary>
    public static class WeightTextFormatter
    {
        /// <summary>The weight in pounds, with at most two decimal places and no trailing zeros.</summary>
        public static string FormatPounds(decimal pounds) =>
            $"{decimal.Round(pounds, 2, MidpointRounding.AwayFromZero).ToString("0.##", CultureInfo.InvariantCulture)} lb";
    }
}
