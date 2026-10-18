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
            FillWhere(color, (x, y) => IsInsidePolygon(x, y, coordinates));
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
            FillWhere(color, (x, y) =>
            {
                for (int index = 0; index + 3 < coordinates.Length; index += 2)
                {
                    if (DistanceToSegment(x, y, coordinates[index], coordinates[index + 1], coordinates[index + 2], coordinates[index + 3]) <= halfWidth)
                    {
                        return true;
                    }
                }

                return false;
            });
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

        // Even-odd ray casting: a point is inside when a ray from it crosses the edges an odd number of times.
        private static bool IsInsidePolygon(float x, float y, float[] coordinates)
        {
            bool inside = false;
            int count = coordinates.Length / 2;
            for (int current = 0, previous = count - 1; current < count; previous = current++)
            {
                float xi = coordinates[current * 2];
                float yi = coordinates[(current * 2) + 1];
                float xj = coordinates[previous * 2];
                float yj = coordinates[(previous * 2) + 1];
                if ((yi > y) != (yj > y) && x < ((xj - xi) * (y - yi) / (yj - yi)) + xi)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private static float DistanceToSegment(float x, float y, float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float lengthSquared = (dx * dx) + (dy * dy);
            float along = lengthSquared == 0f ? 0f : Math.Max(0f, Math.Min(1f, (((x - x1) * dx) + ((y - y1) * dy)) / lengthSquared));
            float nearestX = x1 + (along * dx);
            float nearestY = y1 + (along * dy);
            return (float)Math.Sqrt(((x - nearestX) * (x - nearestX)) + ((y - nearestY) * (y - nearestY)));
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
