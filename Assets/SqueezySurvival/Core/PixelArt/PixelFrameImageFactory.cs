using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Draws a frame as the smallest image that nine-slices cleanly: its border on every side around a 2 x 2 center,
    /// which stretches to fill any size (Docs/Design/ArtDirection.md, Interface).
    /// </summary>
    public static class PixelFrameImageFactory
    {
        private const int CenterSize = 2;

        /// <summary>The frame image for a style.</summary>
        public static PixelImage Create(PixelFrameStyle style)
        {
            if (style == null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            int border = style.BorderThickness;
            int size = (border * 2) + CenterSize;
            var image = new PixelImage(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    image[x, y] = ColorAt(style, x, y, size);
                }
            }

            return image;
        }

        private static PixelColor ColorAt(PixelFrameStyle style, int x, int y, int size)
        {
            int last = size - 1;
            int ring = Math.Min(Math.Min(x, y), Math.Min(last - x, last - y));
            if (ring == 0)
            {
                // The outline's corners are left out, so the frame's corners look rounded.
                bool corner = (x == 0 || x == last) && (y == 0 || y == last);
                return corner ? PixelColor.Transparent : style.Outline;
            }

            if (ring > style.Rings.Count)
            {
                return style.Fill;
            }

            PixelFrameRing rim = style.Rings[ring - 1];
            int far = last - ring;
            bool onTopOrLeft = (y == ring && x < far) || (x == ring && y < far);
            return onTopOrLeft ? rim.TopLeft : rim.BottomRight;
        }
    }
}
