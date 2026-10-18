using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>A small grid of pixels, row 0 at the top. Starts fully transparent.</summary>
    public sealed class PixelImage
    {
        private readonly PixelColor[] _pixels;

        /// <summary>Creates a transparent image.</summary>
        public PixelImage(int width, int height)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, "Must be at least 1.");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, "Must be at least 1.");
            }

            Width = width;
            Height = height;
            _pixels = new PixelColor[width * height];
        }

        /// <summary>Pixels across.</summary>
        public int Width { get; }

        /// <summary>Pixels down.</summary>
        public int Height { get; }

        /// <summary>The pixel at a column and row; outside the image, transparent.</summary>
        public PixelColor this[int x, int y]
        {
            get => Contains(x, y) ? _pixels[(y * Width) + x] : PixelColor.Transparent;
            set
            {
                if (!Contains(x, y))
                {
                    throw new ArgumentOutOfRangeException(nameof(x), $"({x}, {y}) is outside the {Width} x {Height} image.");
                }

                _pixels[(y * Width) + x] = value;
            }
        }

        /// <summary>Whether a column and row are inside the image.</summary>
        public bool Contains(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        /// <summary>A copy of this image.</summary>
        public PixelImage Copy()
        {
            var copy = new PixelImage(Width, Height);
            Array.Copy(_pixels, copy._pixels, _pixels.Length);
            return copy;
        }
    }
}
