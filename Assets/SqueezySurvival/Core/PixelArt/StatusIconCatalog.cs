using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Small status icons shown beside text, such as the warning mark on warning entries in the event log
    /// (Docs/Design/UI/HUD.md, Event log). Drawn and styled like item icons.
    /// </summary>
    public static class StatusIconCatalog
    {
        /// <summary>A warning: a red triangle with an exclamation mark.</summary>
        public const string Warning = "status-warning";

        private const int OutlineMargin = 1;

        private static readonly PixelColor WarningRed = new PixelColor(232, 72, 60);
        private static readonly PixelColor Mark = new PixelColor(255, 244, 220);

        /// <summary>The finished icon, drawn, shaded and outlined.</summary>
        public static PixelImage CreateIcon(string iconId)
        {
            var flat = new PixelImage(PlaceholderIconCatalog.IconSize, PlaceholderIconCatalog.IconSize);
            var canvas = new PixelIconCanvas(flat, OutlineMargin);
            switch (iconId)
            {
                case Warning:
                    canvas.Polygon(WarningRed, 0.50f, 0.04f, 0.98f, 0.94f, 0.02f, 0.94f);
                    canvas.Line(Mark, 0.12f, 0.50f, 0.32f, 0.50f, 0.62f);
                    canvas.Ellipse(Mark, 0.50f, 0.78f, 0.07f, 0.07f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iconId), iconId, "No status icon has that id.");
            }

            return PixelIconStylizer.Stylize(flat);
        }
    }
}
