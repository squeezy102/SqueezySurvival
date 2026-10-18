using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>One pixel's color, 8 bits per channel. Immutable.</summary>
    public readonly struct PixelColor : IEquatable<PixelColor>
    {
        /// <summary>Fully see-through: an empty pixel.</summary>
        public static readonly PixelColor Transparent = new PixelColor(0, 0, 0, 0);

        /// <summary>Creates a color.</summary>
        public PixelColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        /// <summary>Red, 0 to 255.</summary>
        public byte Red { get; }

        /// <summary>Green, 0 to 255.</summary>
        public byte Green { get; }

        /// <summary>Blue, 0 to 255.</summary>
        public byte Blue { get; }

        /// <summary>Opacity, 0 (see-through) to 255 (solid).</summary>
        public byte Alpha { get; }

        /// <summary>Whether anything is drawn in this pixel.</summary>
        public bool IsVisible => Alpha > 0;

        /// <summary>Perceived brightness, 0 to 255, from the ITU-R BT.601 luma weights.</summary>
        public double Luma => (0.299 * Red) + (0.587 * Green) + (0.114 * Blue);

        /// <summary>This color moved toward white by <paramref name="amount"/> (0 to 1); opacity is kept.</summary>
        public PixelColor Lightened(double amount) => Toward(255, amount);

        /// <summary>This color moved toward black by <paramref name="amount"/> (0 to 1); opacity is kept.</summary>
        public PixelColor Darkened(double amount) => Toward(0, amount);

        /// <inheritdoc />
        public bool Equals(PixelColor other) =>
            Red == other.Red && Green == other.Green && Blue == other.Blue && Alpha == other.Alpha;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is PixelColor other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => (Red << 24) | (Green << 16) | (Blue << 8) | Alpha;

        /// <inheritdoc />
        public override string ToString() => $"({Red}, {Green}, {Blue}, {Alpha})";

        private PixelColor Toward(byte target, double amount)
        {
            if (amount < 0 || amount > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Must be from 0 to 1.");
            }

            return new PixelColor(Mix(Red, target, amount), Mix(Green, target, amount), Mix(Blue, target, amount), Alpha);
        }

        private static byte Mix(byte from, byte to, double amount) => (byte)Math.Round(from + ((to - from) * amount));
    }
}
