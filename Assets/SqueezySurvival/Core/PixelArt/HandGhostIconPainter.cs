using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Paints the faint hand shown in an empty hand slot, so the slot reads as a hand at a glance
    /// (Docs/Design/UI/InventoryGrids.md, The grids). Hands are seen from behind, fingers up: the right hand's thumb
    /// is on its left, and the left hand is its mirror.
    /// </summary>
    public static class HandGhostIconPainter
    {
        private const int OutlineMargin = 1;

        private static readonly PixelColor Skin = new PixelColor(206, 214, 236);

        /// <summary>The icon for a hand, drawn, shaded and outlined like other icons.</summary>
        public static PixelImage Create(Hand hand)
        {
            var flat = new PixelImage(PlaceholderIconCatalog.IconSize, PlaceholderIconCatalog.IconSize);
            var canvas = new PixelIconCanvas(flat, OutlineMargin);
            // Fingers are straight and upright, each with a pixel of space beside it, so none reads as bent.
            canvas.Polygon(Skin, 0.24f, 0.50f, 0.82f, 0.50f, 0.82f, 0.80f, 0.68f, 0.97f, 0.36f, 0.97f, 0.24f, 0.80f);
            canvas.Line(Skin, 0.10f, 0.31f, 0.56f, 0.31f, 0.22f);
            canvas.Line(Skin, 0.10f, 0.46f, 0.54f, 0.46f, 0.08f);
            canvas.Line(Skin, 0.10f, 0.61f, 0.54f, 0.61f, 0.12f);
            canvas.Line(Skin, 0.09f, 0.76f, 0.56f, 0.76f, 0.30f);
            canvas.Line(Skin, 0.11f, 0.26f, 0.78f, 0.08f, 0.56f);
            PixelImage right = PixelIconStylizer.Stylize(flat);
            return hand == Hand.Right ? right : Mirrored(right);
        }

        private static PixelImage Mirrored(PixelImage image)
        {
            var mirrored = new PixelImage(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    mirrored[image.Width - 1 - x, y] = image[x, y];
                }
            }

            return mirrored;
        }
    }
}
