using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Paints the HUD's day and night dial at a time of day (Docs/Design/Systems/Time.md, Display): the top half of a
    /// disc, half day sky with the sun and half night sky with the moon and stars, turned so the sun is at the top at
    /// noon. Each pixel is worked out from the disc's design at that angle, so the dial stays crisp as it turns.
    /// </summary>
    public static class DayNightDialPainter
    {
        /// <summary>The disc's radius in art pixels.</summary>
        public const int Radius = 24;

        /// <summary>The painted image's width: the disc's diameter plus its outline.</summary>
        public const int Width = (Radius * 2) + 1;

        /// <summary>The painted image's height: the top half of the disc plus the window's lower edge.</summary>
        public const int Height = Radius + 1;

        private const int MinutesPerDay = 24 * 60;
        private const int NoonMinute = 12 * 60;

        private static readonly PixelColor Rim = new PixelColor(222, 190, 110);
        private static readonly PixelColor RimShadow = new PixelColor(150, 112, 52);
        private static readonly PixelColor SkyHigh = new PixelColor(96, 162, 230);
        private static readonly PixelColor SkyLow = new PixelColor(170, 210, 240);
        private static readonly PixelColor Sun = new PixelColor(255, 236, 140);
        private static readonly PixelColor SunCore = new PixelColor(255, 250, 214);
        private static readonly PixelColor NightHigh = new PixelColor(14, 18, 50);
        private static readonly PixelColor NightLow = new PixelColor(34, 40, 90);
        private static readonly PixelColor Moon = new PixelColor(232, 230, 200);
        private static readonly PixelColor Star = new PixelColor(240, 240, 255);

        // Stars on the night half, as fractions of the radius from the center (positive y is toward the moon).
        private static readonly float[] Stars =
        {
            -0.55f, 0.20f, 0.50f, 0.28f, -0.25f, 0.42f, 0.30f, 0.12f, -0.70f, 0.48f, 0.62f, 0.55f, 0.05f, 0.84f, -0.40f, 0.78f,
        };

        /// <summary>
        /// How far the disc is turned at a minute of the day, in degrees clockwise from noon: 0 at noon, 180 at
        /// midnight, -90 at 6:00 AM (sun on the left) and 90 at 6:00 PM (sun on the right).
        /// </summary>
        public static double RotationDegrees(int minuteOfDay)
        {
            if (minuteOfDay < 0 || minuteOfDay >= MinutesPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(minuteOfDay), minuteOfDay, "Must be from 0 to 1439.");
            }

            return (minuteOfDay - NoonMinute) * 360.0 / MinutesPerDay;
        }

        /// <summary>Paints the dial at a minute of the day.</summary>
        public static PixelImage Paint(int minuteOfDay)
        {
            double angle = RotationDegrees(minuteOfDay) * Math.PI / 180.0;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            var image = new PixelImage(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double screenX = x + 0.5 - (Width / 2.0);
                    double screenY = y + 0.5 - Radius - 0.5;
                    double distance = Math.Sqrt((screenX * screenX) + (screenY * screenY));
                    if (distance > Radius + 0.5)
                    {
                        continue;
                    }

                    if (distance > Radius - 0.5 || y == Height - 1)
                    {
                        image[x, y] = y == Height - 1 ? RimShadow : Rim;
                        continue;
                    }

                    // Turn the screen point back by the disc's rotation to find where on the disc's design it falls.
                    double discX = ((screenX * cos) + (screenY * sin)) / Radius;
                    double discY = ((-screenX * sin) + (screenY * cos)) / Radius;
                    image[x, y] = DesignColorAt(discX, discY);
                }
            }

            return image;
        }

        // The disc's design, unturned, in fractions of the radius: day above the middle with the sun at the top,
        // night below with the moon at the bottom.
        private static PixelColor DesignColorAt(double x, double y)
        {
            double fromCenter = Math.Sqrt((x * x) + (y * y));
            if (y < 0)
            {
                if (IsWithin(x, y, 0, -0.62, 0.16))
                {
                    return SunCore;
                }

                if (IsWithin(x, y, 0, -0.62, 0.26))
                {
                    return Sun;
                }

                return fromCenter > 0.55 ? SkyHigh : SkyLow;
            }

            // A crescent: the moon's disc with a second disc taken out of one side.
            if (IsWithin(x, y, 0, 0.62, 0.22) && !IsWithin(x, y, 0.10, 0.56, 0.18))
            {
                return Moon;
            }

            for (int index = 0; index < Stars.Length; index += 2)
            {
                if (Math.Abs(x - Stars[index]) < 0.03 && Math.Abs(y - Stars[index + 1]) < 0.03)
                {
                    return Star;
                }
            }

            return fromCenter > 0.55 ? NightHigh : NightLow;
        }

        private static bool IsWithin(double x, double y, double centerX, double centerY, double radius) =>
            ((x - centerX) * (x - centerX)) + ((y - centerY) * (y - centerY)) <= radius * radius;
    }
}
