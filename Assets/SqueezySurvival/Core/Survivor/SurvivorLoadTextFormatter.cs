using System;
using System.Globalization;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Survivor
{
    /// <summary>
    /// The load as text, until the load gauge replaces it (Docs/Design/UI/InventoryGrids.md, The grids):
    /// "Load: 34 lb of 100 lb (34%), Normal".
    /// </summary>
    public static class SurvivorLoadTextFormatter
    {
        /// <summary>
        /// Weight, limit, percent and state. The percent is rounded down, so a survivor shown at 99% is never immobile.
        /// </summary>
        public static string Format(SurvivorLoad load)
        {
            if (load == null)
            {
                throw new ArgumentNullException(nameof(load));
            }

            string percent = decimal.Floor(load.Percent).ToString("0", CultureInfo.InvariantCulture);
            return $"Load: {WeightTextFormatter.FormatPounds(load.CarriedPounds)} of {WeightTextFormatter.FormatPounds(load.LimitPounds)} ({percent}%), {load.State}";
        }
    }
}
