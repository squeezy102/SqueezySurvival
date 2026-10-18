namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>The interface's colors: navy glass with gold trim (Docs/Design/ArtDirection.md, Palette).</summary>
    public static class InterfacePalette
    {
        /// <summary>Frame outlines.</summary>
        public static readonly PixelColor Outline = new PixelColor(14, 18, 40);

        /// <summary>Gold trim highlight.</summary>
        public static readonly PixelColor GoldLight = new PixelColor(222, 190, 110);

        /// <summary>Gold trim shadow.</summary>
        public static readonly PixelColor GoldDark = new PixelColor(150, 112, 52);

        /// <summary>The band inside the gold trim.</summary>
        public static readonly PixelColor NavyRim = new PixelColor(60, 80, 140);

        /// <summary>Panel interiors, see-through so the scene shows behind them.</summary>
        public static readonly PixelColor PanelFill = new PixelColor(24, 36, 84, 165);

        /// <summary>Window interiors, nearly opaque so they read over anything.</summary>
        public static readonly PixelColor WindowFill = new PixelColor(24, 36, 84, 240);

        /// <summary>Button highlight.</summary>
        public static readonly PixelColor ButtonLight = new PixelColor(120, 150, 220);

        /// <summary>Button face.</summary>
        public static readonly PixelColor Button = new PixelColor(70, 96, 170);

        /// <summary>Button shadow.</summary>
        public static readonly PixelColor ButtonDark = new PixelColor(40, 56, 110);

        /// <summary>Slot interiors, slightly see-through.</summary>
        public static readonly PixelColor Slot = new PixelColor(10, 16, 40, 190);

        /// <summary>Slot shadow, along the top and left.</summary>
        public static readonly PixelColor SlotShadow = new PixelColor(8, 12, 30);

        /// <summary>Slot highlight, along the bottom and right.</summary>
        public static readonly PixelColor SlotLight = new PixelColor(90, 110, 170);

        /// <summary>Tooltip face.</summary>
        public static readonly PixelColor Tooltip = new PixelColor(236, 240, 250);

        /// <summary>Tooltip highlight.</summary>
        public static readonly PixelColor TooltipLight = new PixelColor(255, 255, 255);

        /// <summary>Tooltip shadow.</summary>
        public static readonly PixelColor TooltipDark = new PixelColor(150, 160, 190);
    }
}
