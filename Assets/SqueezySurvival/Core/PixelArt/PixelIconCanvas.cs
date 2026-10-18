using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Draws simple shapes, described on a 1 x 1 square, into a pixel image: a pixel is filled when its center is
    /// inside the shape (Docs/Design/ArtDirection.md, Icons). A margin is left around the square for outlines.
    /// </summary>
    public sealed class PixelIconCanvas
    {
        private readonly PixelImage _image;
        private readonly int _margin;
        private readonly int _drawnSize;

        /// <summary>Draws into <paramref name="image"/>, keeping <paramref name="margin"/> pixels clear on each side.</summary>
        public PixelIconCanvas(PixelImage image, int margin)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            if (margin < 0 || (margin * 2) >= Math.Min(image.Width, image.Height))
            {
                throw new ArgumentOutOfRangeException(nameof(margin), margin, "Must leave room to draw.");
            }

            _margin = margin;
            _drawnSize = Math.Min(image.Width, image.Height) - (margin * 2);
        }

        /// <summary>Fills a polygon given as x, y pairs.</summary>
        public void Polygon(PixelColor color, params float[] coordinates)
        {
            RequirePoints(coordinates, 3);
            FillWhere(color, (x, y) => PixelGeometry.IsInsidePolygon(x, y, coordinates));
        }

        /// <summary>Fills an ellipse.</summary>
        public void Ellipse(PixelColor color, float centerX, float centerY, float radiusX, float radiusY)
        {
            if (radiusX <= 0f || radiusY <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(radiusX), "Radii must be more than 0.");
            }

            // A radius under half a pixel would draw nothing, so dots are at least one pixel.
            float minimumRadius = 0.5f / _drawnSize;
            float rx = Math.Max(radiusX, minimumRadius);
            float ry = Math.Max(radiusY, minimumRadius);
            FillWhere(color, (x, y) =>
            {
                float dx = (x - centerX) / rx;
                float dy = (y - centerY) / ry;
                return (dx * dx) + (dy * dy) <= 1f;
            });
        }

        /// <summary>Draws a line through x, y pairs, <paramref name="width"/> wide (at least one pixel), with round ends.</summary>
        public void Line(PixelColor color, float width, params float[] coordinates)
        {
            RequirePoints(coordinates, 2);
            float halfWidth = Math.Max(width, 1f / _drawnSize) / 2f;
            FillWhere(color, (x, y) => PixelGeometry.DistanceToPolyline(x, y, coordinates) <= halfWidth);
        }

        private void FillWhere(PixelColor color, Func<float, float, bool> isInside)
        {
            for (int row = 0; row < _drawnSize; row++)
            {
                for (int column = 0; column < _drawnSize; column++)
                {
                    float x = (column + 0.5f) / _drawnSize;
                    float y = (row + 0.5f) / _drawnSize;
                    if (isInside(x, y))
                    {
                        _image[column + _margin, row + _margin] = color;
                    }
                }
            }
        }

        private static void RequirePoints(float[] coordinates, int minimumPoints)
        {
            if (coordinates == null || coordinates.Length % 2 != 0 || coordinates.Length < minimumPoints * 2)
            {
                throw new ArgumentException($"Needs at least {minimumPoints} x, y pairs.", nameof(coordinates));
            }
        }
    }
}
