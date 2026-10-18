using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// How a pixel-art frame looks: a 1-pixel outline with see-through corners, rim rings from the outside in, and a
    /// fill (Docs/Design/ArtDirection.md, Interface). Immutable.
    /// </summary>
    public sealed class PixelFrameStyle
    {
        /// <summary>Creates a style.</summary>
        public PixelFrameStyle(PixelColor outline, IReadOnlyList<PixelFrameRing> rings, PixelColor fill)
        {
            Outline = outline;
            Rings = rings ?? throw new ArgumentNullException(nameof(rings));
            Fill = fill;
        }

        /// <summary>The outermost pixel ring.</summary>
        public PixelColor Outline { get; }

        /// <summary>The rim rings inside the outline, outermost first.</summary>
        public IReadOnlyList<PixelFrameRing> Rings { get; }

        /// <summary>The inside.</summary>
        public PixelColor Fill { get; }

        /// <summary>How thick the border is, in pixels: the outline plus the rings. This is also the nine-slice inset.</summary>
        public int BorderThickness => 1 + Rings.Count;
    }
}
