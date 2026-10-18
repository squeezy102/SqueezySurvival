namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>The frame of each kind of interface element (Docs/Design/ArtDirection.md, Interface).</summary>
    public static class InterfaceFrameStyles
    {
        /// <summary>How many screen pixels wide each frame pixel is drawn, at the reference resolution.</summary>
        public const int PixelScale = 3;

        /// <summary>How much lighter a hovered or focused button is.</summary>
        public const double HoveredButtonLightening = 0.15;

        /// <summary>Gold trim and a navy band around see-through navy glass.</summary>
        public static readonly PixelFrameStyle Panel = GoldTrimmed(InterfacePalette.PanelFill);

        /// <summary>The panel frame, nearly opaque, for windows over the game.</summary>
        public static readonly PixelFrameStyle Window = GoldTrimmed(InterfacePalette.WindowFill);

        /// <summary>A raised blue block.</summary>
        public static readonly PixelFrameStyle Button = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.ButtonLight, InterfacePalette.ButtonDark) },
            InterfacePalette.Button);

        /// <summary>The button, lighter, while hovered or focused.</summary>
        public static readonly PixelFrameStyle HoveredButton = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.ButtonLight.Lightened(HoveredButtonLightening), InterfacePalette.ButtonDark.Lightened(HoveredButtonLightening)) },
            InterfacePalette.Button.Lightened(HoveredButtonLightening));

        /// <summary>The button pushed in: its bevel reversed, its face darker.</summary>
        public static readonly PixelFrameStyle PressedButton = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.ButtonDark, InterfacePalette.ButtonLight) },
            InterfacePalette.ButtonDark);

        /// <summary>A sunken square that items sit in.</summary>
        public static readonly PixelFrameStyle Slot = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.SlotShadow, InterfacePalette.SlotLight) },
            InterfacePalette.Slot);

        /// <summary>A pale card.</summary>
        public static readonly PixelFrameStyle Tooltip = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.TooltipLight, InterfacePalette.TooltipDark) },
            InterfacePalette.Tooltip);

        private static PixelFrameStyle GoldTrimmed(PixelColor fill) => new PixelFrameStyle(
            InterfacePalette.Outline,
            new[]
            {
                new PixelFrameRing(InterfacePalette.GoldLight, InterfacePalette.GoldDark),
                new PixelFrameRing(InterfacePalette.NavyRim, InterfacePalette.NavyRim),
            },
            fill);
    }
}
