using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Draws scene shapes onto a pixel image in pixel coordinates (0, 0 is the top-left corner). A pixel is filled
    /// when its center is inside the shape, as for icons (Docs/Design/ArtDirection.md, Backgrounds).
    /// </summary>
    public sealed class PixelSceneCanvas
    {
        // A 2 x 2 ordered-dither matrix: the share of a band step at which each pixel of a 2 x 2 block turns over.
        private static readonly double[,] DitherThresholds = { { 0.125, 0.625 }, { 0.875, 0.375 } };

        private readonly PixelImage _image;

        /// <summary>Creates a canvas that draws onto <paramref name="image"/>.</summary>
        public PixelSceneCanvas(PixelImage image)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
        }

        /// <summary>
        /// Fills the rows from <paramref name="top"/> to <paramref name="bottom"/> (exclusive) with flat bands of
        /// <paramref name="bands"/>, top to bottom, ordered-dithered where one band meets the next, as 16-bit skies are.
        /// </summary>
        public void DitheredBands(int top, int bottom, IReadOnlyList<PixelColor> bands)
        {
            if (bands == null || bands.Count < 2)
            {
                throw new ArgumentException("Needs at least two bands.", nameof(bands));
            }

            if (top < 0 || bottom > _image.Height || bottom <= top)
            {
                throw new ArgumentOutOfRangeException(nameof(bottom), "The rows must lie inside the image, top before bottom.");
            }

            int rows = bottom - top;
            for (int y = top; y < bottom; y++)
            {
                double position = (double)(y - top) / rows * (bands.Count - 1);
                int band = Math.Min(bands.Count - 2, (int)Math.Floor(position));
                double step = position - band;
                for (int x = 0; x < _image.Width; x++)
                {
                    bool next = step > DitherThresholds[y % 2, x % 2];
                    _image[x, y] = bands[next ? band + 1 : band];
                }
            }
        }

        /// <summary>Fills an ellipse centered on a point.</summary>
        public void Ellipse(PixelColor color, float centerX, float centerY, float radiusX, float radiusY)
        {
            if (radiusX <= 0f || radiusY <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(radiusX), "Radii must be more than 0.");
            }

            FillWhere(color, (x, y) =>
            {
                float dx = (x - centerX) / radiusX;
                float dy = (y - centerY) / radiusY;
                return (dx * dx) + (dy * dy) <= 1f;
            });
        }

        /// <summary>Draws a line of the given width through x, y pairs.</summary>
        public void Line(PixelColor color, float width, params float[] coordinates)
        {
            if (coordinates == null || coordinates.Length < 4 || coordinates.Length % 2 != 0)
            {
                throw new ArgumentException("Needs at least 2 x, y pairs.", nameof(coordinates));
            }

            float halfWidth = Math.Max(width, 1f) / 2f;
            FillWhere(color, (x, y) => PixelGeometry.DistanceToPolyline(x, y, coordinates) <= halfWidth);
        }

        /// <summary>Fills a horizontal run of pixels.</summary>
        public void HorizontalRun(PixelColor color, int left, int y, int length)
        {
            for (int x = Math.Max(0, left); x < Math.Min(_image.Width, left + length); x++)
            {
                if (_image.Contains(x, y))
                {
                    _image[x, y] = color;
                }
            }
        }

        private void FillWhere(PixelColor color, Func<float, float, bool> isInside)
        {
            for (int y = 0; y < _image.Height; y++)
            {
                for (int x = 0; x < _image.Width; x++)
                {
                    if (isInside(x + 0.5f, y + 0.5f))
                    {
                        _image[x, y] = color;
                    }
                }
            }
        }
    }
}
