using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>Layers pixel images: every visible pixel of the top layer replaces the one beneath it.</summary>
    public static class PixelImageCompositor
    {
        /// <summary>Draws <paramref name="layer"/> over <paramref name="target"/>, which must be the same size.</summary>
        public static void DrawOver(PixelImage target, PixelImage layer)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (layer == null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            if (target.Width != layer.Width || target.Height != layer.Height)
            {
                throw new ArgumentException("The layer must be the same size as the target.", nameof(layer));
            }

            for (int y = 0; y < target.Height; y++)
            {
                for (int x = 0; x < target.Width; x++)
                {
                    if (layer[x, y].IsVisible)
                    {
                        target[x, y] = layer[x, y];
                    }
                }
            }
        }
    }
}
