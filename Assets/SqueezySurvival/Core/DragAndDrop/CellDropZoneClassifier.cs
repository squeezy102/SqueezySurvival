using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Finds which drop zone of a cell a point is in. The center is the middle half of the cell in each direction;
    /// the rest belongs to the nearest edge (Docs/Design/UI/DragAndDrop.md, Drop zones).
    /// </summary>
    public static class CellDropZoneClassifier
    {
        /// <summary>How far in from each side the center starts, as a fraction of the cell.</summary>
        public const float EdgeDepth = 0.25f;

        /// <summary>
        /// The zone at a point given as fractions of the cell's width and height, from its top-left corner
        /// (0 to 1; values outside are clamped).
        /// </summary>
        public static CellDropZone Classify(float xFraction, float yFraction)
        {
            float x = Clamp01(xFraction);
            float y = Clamp01(yFraction);
            if (x >= EdgeDepth && x <= 1f - EdgeDepth && y >= EdgeDepth && y <= 1f - EdgeDepth)
            {
                return CellDropZone.Center;
            }

            float toLeft = x;
            float toRight = 1f - x;
            float toTop = y;
            float toBottom = 1f - y;
            float nearest = Math.Min(Math.Min(toLeft, toRight), Math.Min(toTop, toBottom));

            if (nearest == toLeft)
            {
                return CellDropZone.Left;
            }

            if (nearest == toRight)
            {
                return CellDropZone.Right;
            }

            return nearest == toTop ? CellDropZone.Top : CellDropZone.Bottom;
        }

        private static float Clamp01(float value) => float.IsNaN(value) ? 0.5f : Math.Max(0f, Math.Min(1f, value));
    }
}
