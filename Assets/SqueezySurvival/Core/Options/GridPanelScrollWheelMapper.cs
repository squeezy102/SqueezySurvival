using System;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// Turns mouse wheel movement into scrolling along a grid panel's scroll direction, reversed when the wheel is
    /// inverted (Docs/Design/UI/GridPanels.md, Scroll direction). In a horizontal panel the ordinary wheel scrolls
    /// sideways.
    /// </summary>
    public static class GridPanelScrollWheelMapper
    {
        /// <summary>
        /// How far to move the scroll offset along the panel's scroll direction. A positive result moves toward the
        /// end of the content (down, or right).
        /// </summary>
        /// <param name="options">The panel's settings.</param>
        /// <param name="wheelDeltaX">Sideways wheel movement, from a tilting wheel or touchpad.</param>
        /// <param name="wheelDeltaY">Ordinary wheel movement; positive when rolled toward the player.</param>
        /// <param name="pixelsPerWheelUnit">Scroll distance for one unit of wheel movement.</param>
        public static float ScrollOffsetChange(GridPanelLayoutOptions options, float wheelDeltaX, float wheelDeltaY, float pixelsPerWheelUnit)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // The ordinary wheel drives either direction; sideways movement only counts when there is no ordinary
            // movement, so a touchpad's diagonal swipe does not scroll twice.
            float wheelDelta = wheelDeltaY != 0f ? wheelDeltaY : wheelDeltaX;
            float direction = options.InvertScrollWheel ? -1f : 1f;
            return wheelDelta * pixelsPerWheelUnit * direction;
        }
    }
}
