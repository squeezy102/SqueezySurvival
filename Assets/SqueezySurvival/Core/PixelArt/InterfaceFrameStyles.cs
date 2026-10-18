namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>The frame of each kind of interface element (Docs/Design/ArtDirection.md, Interface).</summary>
    public static class InterfaceFrameStyles
    {
        /// <summary>How many screen pixels wide each frame pixel is drawn, at the reference resolution.</summary>
        public const int PixelScale = 3;

        /// <summary>How much lighter a hovered or focused button is.</summary>
        public const double HoveredButtonLightening = 0.15;

        /// <summary>A wood frame around a deep brown, slightly see-through inside.</summary>
        public static readonly PixelFrameStyle Panel = Wooden(InterfacePalette.PanelFill);

        /// <summary>The panel frame, fully opaque, for windows over the game.</summary>
        public static readonly PixelFrameStyle Window = Wooden(InterfacePalette.WindowFill);

        /// <summary>A raised tan block.</summary>
        public static readonly PixelFrameStyle Button = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.TanLight, InterfacePalette.TanDark) },
            InterfacePalette.Tan);

        /// <summary>The button, lighter, while hovered or focused.</summary>
        public static readonly PixelFrameStyle HoveredButton = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.TanLight.Lightened(HoveredButtonLightening), InterfacePalette.TanDark.Lightened(HoveredButtonLightening)) },
            InterfacePalette.Tan.Lightened(HoveredButtonLightening));

        /// <summary>The button pushed in: its bevel reversed, its face darker.</summary>
        public static readonly PixelFrameStyle PressedButton = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.TanDark, InterfacePalette.TanLight) },
            InterfacePalette.TanDark);

        /// <summary>A sunken square that items sit in.</summary>
        public static readonly PixelFrameStyle Slot = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.WoodDark, InterfacePalette.WoodLight) },
            InterfacePalette.Slot);

        /// <summary>A parchment block.</summary>
        public static readonly PixelFrameStyle Tooltip = new PixelFrameStyle(
            InterfacePalette.Outline,
            new[] { new PixelFrameRing(InterfacePalette.Parchment, InterfacePalette.TanDark) },
            InterfacePalette.Parchment);

        private static PixelFrameStyle Wooden(PixelColor fill) => new PixelFrameStyle(
            InterfacePalette.Outline,
            new[]
            {
                new PixelFrameRing(InterfacePalette.WoodLight, InterfacePalette.WoodDark),
                new PixelFrameRing(InterfacePalette.Wood, InterfacePalette.Wood),
            },
            fill);
    }
}
