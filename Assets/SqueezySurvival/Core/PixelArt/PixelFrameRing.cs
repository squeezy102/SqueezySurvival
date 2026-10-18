namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// One 1-pixel ring of a frame's rim: one color along its top and left, another along its bottom and right, which
    /// makes it look raised (light top left) or sunken (dark top left). Immutable.
    /// </summary>
    public sealed class PixelFrameRing
    {
        /// <summary>Creates a ring.</summary>
        public PixelFrameRing(PixelColor topLeft, PixelColor bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        /// <summary>The color along the top and left.</summary>
        public PixelColor TopLeft { get; }

        /// <summary>The color along the bottom and right.</summary>
        public PixelColor BottomRight { get; }
    }
}
