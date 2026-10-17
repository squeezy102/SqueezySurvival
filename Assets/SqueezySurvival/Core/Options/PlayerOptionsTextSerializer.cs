using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// Writes player options as plain "key=value" lines, and reads them back. Reading is forgiving: unknown keys are
    /// ignored and a panel whose values are missing or invalid keeps its defaults, so an old or hand-edited file
    /// never stops the game from starting.
    /// </summary>
    public static class PlayerOptionsTextSerializer
    {
        /// <summary>The format version written to every file.</summary>
        public const int FormatVersion = 1;

        private const string VersionKey = "format-version";

        /// <summary>Writes the options as text.</summary>
        public static string Serialize(PlayerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var text = new StringBuilder();
            text.Append(VersionKey).Append('=').Append(FormatVersion.ToString(CultureInfo.InvariantCulture)).Append('\n');
            foreach (GridPanel panel in PlayerOptions.AllGridPanels)
            {
                GridPanelLayoutOptions layout = options.GridPanelLayout(panel);
                string prefix = PanelKeyPrefix(panel);
                text.Append(prefix).Append("scroll-direction=").Append(layout.ScrollDirection).Append('\n');
                text.Append(prefix).Append("columns=").Append(layout.ColumnCount.ToString(CultureInfo.InvariantCulture)).Append('\n');
                text.Append(prefix).Append("rows=").Append(layout.RowCount.ToString(CultureInfo.InvariantCulture)).Append('\n');
                text.Append(prefix).Append("invert-scroll-wheel=").Append(layout.InvertScrollWheel ? "true" : "false").Append('\n');
            }

            return text.ToString();
        }

        /// <summary>Reads options from text, using defaults for anything missing or invalid.</summary>
        public static PlayerOptions Deserialize(string text)
        {
            var options = new PlayerOptions();
            if (string.IsNullOrEmpty(text))
            {
                return options;
            }

            Dictionary<string, string> values = ParseLines(text);
            foreach (GridPanel panel in PlayerOptions.AllGridPanels)
            {
                if (TryReadPanel(values, panel, out GridPanelLayoutOptions layout))
                {
                    options.SetGridPanelLayout(panel, layout);
                }
            }

            return options;
        }

        private static Dictionary<string, string> ParseLines(string text)
        {
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string rawLine in text.Split('\n'))
            {
                string line = rawLine.Trim();
                int separator = line.IndexOf('=');
                if (separator > 0)
                {
                    values[line.Substring(0, separator).Trim()] = line.Substring(separator + 1).Trim();
                }
            }

            return values;
        }

        private static bool TryReadPanel(Dictionary<string, string> values, GridPanel panel, out GridPanelLayoutOptions layout)
        {
            layout = null;
            string prefix = PanelKeyPrefix(panel);
            if (!values.TryGetValue(prefix + "scroll-direction", out string directionText)
                || !Enum.TryParse(directionText, ignoreCase: false, out GridPanelScrollDirection direction)
                || !Enum.IsDefined(typeof(GridPanelScrollDirection), direction)
                || !TryReadInt(values, prefix + "columns", out int columns)
                || !TryReadInt(values, prefix + "rows", out int rows)
                || !values.TryGetValue(prefix + "invert-scroll-wheel", out string invertText)
                || !bool.TryParse(invertText, out bool invert))
            {
                return false;
            }

            if (columns < GridPanelLayoutOptions.MinimumColumnCount || columns > GridPanelLayoutOptions.MaximumColumnCount
                || rows < GridPanelLayoutOptions.MinimumRowCount || rows > GridPanelLayoutOptions.MaximumRowCount)
            {
                return false;
            }

            layout = new GridPanelLayoutOptions(direction, columns, rows, invert);
            return true;
        }

        private static bool TryReadInt(Dictionary<string, string> values, string key, out int value)
        {
            value = 0;
            return values.TryGetValue(key, out string text)
                && int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        private static string PanelKeyPrefix(GridPanel panel)
        {
            switch (panel)
            {
                case GridPanel.ZoneFeatures:
                    return "grid-panel.zone-features.";
                case GridPanel.GroundPool:
                    return "grid-panel.ground-pool.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(panel), panel, "Unknown grid panel.");
            }
        }
    }
}
