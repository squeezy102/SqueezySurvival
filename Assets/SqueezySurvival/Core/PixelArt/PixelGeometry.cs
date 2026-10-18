using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>The point tests shared by the pixel-art canvases.</summary>
    public static class PixelGeometry
    {
        /// <summary>
        /// Whether a point is inside a polygon given as x, y pairs, by even-odd ray casting: a point is inside when a
        /// ray from it crosses the edges an odd number of times.
        /// </summary>
        public static bool IsInsidePolygon(float x, float y, float[] coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

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

        /// <summary>The distance from a point to the segment between two points.</summary>
        public static float DistanceToSegment(float x, float y, float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float lengthSquared = (dx * dx) + (dy * dy);
            float along = lengthSquared == 0f ? 0f : Math.Max(0f, Math.Min(1f, (((x - x1) * dx) + ((y - y1) * dy)) / lengthSquared));
            float nearestX = x1 + (along * dx);
            float nearestY = y1 + (along * dy);
            return (float)Math.Sqrt(((x - nearestX) * (x - nearestX)) + ((y - nearestY) * (y - nearestY)));
        }

        /// <summary>The distance from a point to the nearest segment of a polyline given as x, y pairs.</summary>
        public static float DistanceToPolyline(float x, float y, float[] coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            float nearest = float.MaxValue;
            for (int index = 0; index + 3 < coordinates.Length; index += 2)
            {
                nearest = Math.Min(nearest, DistanceToSegment(x, y, coordinates[index], coordinates[index + 1], coordinates[index + 2], coordinates[index + 3]));
            }

            return nearest;
        }
    }
}
