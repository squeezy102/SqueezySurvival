namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>The interface's colors (Docs/Design/ArtDirection.md, Palette).</summary>
    public static class InterfacePalette
    {
        /// <summary>Frame outlines.</summary>
        public static readonly PixelColor Outline = new PixelColor(42, 24, 16);

        /// <summary>Wood rim highlight.</summary>
        public static readonly PixelColor WoodLight = new PixelColor(196, 136, 76);

        /// <summary>Wood rim.</summary>
        public static readonly PixelColor Wood = new PixelColor(150, 96, 52);

        /// <summary>Wood rim shadow.</summary>
        public static readonly PixelColor WoodDark = new PixelColor(98, 58, 32);

        /// <summary>Panel interiors, slightly see-through.</summary>
        public static readonly PixelColor PanelFill = new PixelColor(52, 34, 26, 235);

        /// <summary>Window interiors.</summary>
        public static readonly PixelColor WindowFill = new PixelColor(52, 34, 26);

        /// <summary>Button highlight.</summary>
        public static readonly PixelColor TanLight = new PixelColor(246, 214, 160);

        /// <summary>Button face.</summary>
        public static readonly PixelColor Tan = new PixelColor(222, 182, 122);

        /// <summary>Button shadow.</summary>
        public static readonly PixelColor TanDark = new PixelColor(170, 126, 74);

        /// <summary>Slot interiors.</summary>
        public static readonly PixelColor Slot = new PixelColor(34, 22, 16);

        /// <summary>Tooltip face.</summary>
        public static readonly PixelColor Parchment = new PixelColor(250, 236, 200);
    }
}
