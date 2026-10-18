using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Gives a flat pixel icon 16-bit shading and a colored outline (Docs/Design/ArtDirection.md, Icons): light from
    /// the top left on each color region's edges, then an outline in a darkened neighboring color.
    /// </summary>
    public static class PixelIconStylizer
    {
        /// <summary>How far toward white a lit edge moves.</summary>
        public const double HighlightAmount = 0.25;

        /// <summary>How far toward black a shadowed edge moves.</summary>
        public const double ShadowAmount = 0.25;

        /// <summary>How far toward black an outline moves from the color it surrounds.</summary>
        public const double OutlineAmount = 0.65;

        /// <summary>Returns a shaded, outlined copy of <paramref name="flat"/>.</summary>
        public static PixelImage Stylize(PixelImage flat)
        {
            if (flat == null)
            {
                throw new ArgumentNullException(nameof(flat));
            }

            PixelImage shaded = Shade(flat);
            return Outline(flat, shaded);
        }

        private static PixelImage Shade(PixelImage flat)
        {
            PixelImage shaded = flat.Copy();
            for (int y = 0; y < flat.Height; y++)
            {
                for (int x = 0; x < flat.Width; x++)
                {
                    PixelColor color = flat[x, y];
                    if (!color.IsVisible)
                    {
                        continue;
                    }

                    // Shadow wins where a pixel is both, so thin shapes read as rounded rather than flat.
                    if (!flat[x + 1, y].Equals(color) || !flat[x, y + 1].Equals(color))
                    {
                        shaded[x, y] = color.Darkened(ShadowAmount);
                    }
                    else if (!flat[x - 1, y].Equals(color) || !flat[x, y - 1].Equals(color))
                    {
                        shaded[x, y] = color.Lightened(HighlightAmount);
                    }
                }
            }

            return shaded;
        }

        private static PixelImage Outline(PixelImage flat, PixelImage shaded)
        {
            PixelImage outlined = shaded.Copy();
            for (int y = 0; y < flat.Height; y++)
            {
                for (int x = 0; x < flat.Width; x++)
                {
                    if (flat[x, y].IsVisible)
                    {
                        continue;
                    }

                    PixelColor? darkest = Darkest(flat[x - 1, y], flat[x + 1, y], flat[x, y - 1], flat[x, y + 1]);
                    if (darkest.HasValue)
                    {
                        outlined[x, y] = darkest.Value.Darkened(OutlineAmount);
                    }
                }
            }

            return outlined;
        }

        private static PixelColor? Darkest(params PixelColor[] neighbors)
        {
            PixelColor? darkest = null;
            foreach (PixelColor neighbor in neighbors)
            {
                if (neighbor.IsVisible && (!darkest.HasValue || neighbor.Luma < darkest.Value.Luma))
                {
                    darkest = neighbor;
                }
            }

            return darkest;
        }
    }
}
