namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// The shapes and colors of every placeholder icon for items and zone features, keyed by definition id, until
    /// real icon art exists (Docs/Design/ArtDirection.md, Icons). Shapes are described on a 1 x 1 square.
    /// </summary>
    public static class PlaceholderIconCatalog
    {
        /// <summary>The icon grid's size in pixels.</summary>
        public const int IconSize = 16;

        // One pixel on each side is kept clear for the outline.
        private const int OutlineMargin = 1;

        private static readonly PixelColor Stone = new PixelColor(158, 158, 153);
        private static readonly PixelColor DarkStone = new PixelColor(107, 107, 107);
        private static readonly PixelColor Wood = new PixelColor(140, 92, 46);
        private static readonly PixelColor PaleWood = new PixelColor(184, 158, 122);
        private static readonly PixelColor Leaf = new PixelColor(64, 158, 64);
        private static readonly PixelColor DarkLeaf = new PixelColor(41, 115, 51);
        private static readonly PixelColor Seaweed = new PixelColor(76, 128, 51);
        private static readonly PixelColor ShellPink = new PixelColor(245, 194, 178);
        private static readonly PixelColor ShellOrange = new PixelColor(242, 158, 97);
        private static readonly PixelColor Coconut = new PixelColor(115, 76, 43);
        private static readonly PixelColor Water = new PixelColor(76, 158, 219);
        private static readonly PixelColor DeepWater = new PixelColor(41, 102, 168);
        private static readonly PixelColor Unknown = new PixelColor(217, 64, 191);
        private static readonly PixelColor White = new PixelColor(240, 240, 235);
        private static readonly PixelColor Gray = new PixelColor(140, 148, 158);
        private static readonly PixelColor CrabRed = new PixelColor(219, 89, 56);
        private static readonly PixelColor Sand = new PixelColor(230, 214, 168);
        private static readonly PixelColor DarkWood = new PixelColor(96, 62, 30);
        private static readonly PixelColor DarkCrabRed = new PixelColor(160, 58, 36);
        private static readonly PixelColor Clam = new PixelColor(222, 206, 180);
        private static readonly PixelColor ClamRidge = new PixelColor(168, 150, 124);
        private static readonly PixelColor Oyster = new PixelColor(150, 142, 126);
        private static readonly PixelColor OysterRidge = new PixelColor(110, 102, 90);
        private static readonly PixelColor BarnacleStalk = new PixelColor(120, 84, 60);
        private static readonly PixelColor Bone = new PixelColor(232, 226, 204);
        private static readonly PixelColor ConeTan = new PixelColor(232, 204, 150);
        private static readonly PixelColor ConeBrown = new PixelColor(120, 70, 36);
        private static readonly PixelColor Sponge = new PixelColor(235, 199, 89);

        /// <summary>The finished icon for the thing with the given definition id: drawn, shaded and outlined.</summary>
        public static PixelImage CreateIcon(string definitionId)
        {
            var flat = new PixelImage(IconSize, IconSize);
            Draw(new PixelIconCanvas(flat, OutlineMargin), definitionId);
            return PixelIconStylizer.Stylize(flat);
        }

        /// <summary>Whether the catalog has an icon for the id, rather than the magenta square for anything unfinished.</summary>
        public static bool HasIcon(string definitionId) =>
            Draw(new PixelIconCanvas(new PixelImage(IconSize, IconSize), OutlineMargin), definitionId);

        // Draws the icon and returns whether the id has one; anything else gets the magenta square.
        private static bool Draw(PixelIconCanvas canvas, string definitionId)
        {
            switch (definitionId)
            {
                case "stone":
                    canvas.Polygon(Stone, 0.18f, 0.62f, 0.30f, 0.38f, 0.55f, 0.30f, 0.80f, 0.45f, 0.84f, 0.70f, 0.55f, 0.80f, 0.28f, 0.78f);
                    break;
                case "big-rock":
                    canvas.Polygon(DarkStone, 0.06f, 0.86f, 0.12f, 0.46f, 0.32f, 0.20f, 0.62f, 0.14f, 0.86f, 0.32f, 0.95f, 0.72f, 0.80f, 0.90f);
                    canvas.Polygon(Stone, 0.20f, 0.80f, 0.24f, 0.48f, 0.40f, 0.30f, 0.62f, 0.26f, 0.80f, 0.42f, 0.84f, 0.74f);
                    canvas.Line(DarkStone, 0.03f, 0.46f, 0.36f, 0.54f, 0.56f, 0.48f, 0.74f);
                    break;
                case "small-stick":
                    canvas.Line(Wood, 0.07f, 0.18f, 0.82f, 0.82f, 0.18f);
                    canvas.Line(Wood, 0.05f, 0.52f, 0.48f, 0.66f, 0.28f);
                    break;
                case "big-stick":
                    canvas.Line(Wood, 0.14f, 0.12f, 0.88f, 0.88f, 0.12f);
                    canvas.Line(Wood, 0.08f, 0.44f, 0.56f, 0.70f, 0.50f);
                    canvas.Ellipse(PaleWood, 0.88f, 0.12f, 0.06f, 0.06f);
                    break;
                case "log":
                    canvas.Polygon(Wood, 0.10f, 0.34f, 0.80f, 0.34f, 0.80f, 0.72f, 0.10f, 0.72f);
                    canvas.Ellipse(PaleWood, 0.80f, 0.53f, 0.12f, 0.19f);
                    canvas.Ellipse(Wood, 0.80f, 0.53f, 0.05f, 0.08f);
                    canvas.Line(DarkWood, 0.03f, 0.20f, 0.46f, 0.60f, 0.46f);
                    canvas.Line(DarkWood, 0.03f, 0.30f, 0.62f, 0.66f, 0.62f);
                    break;
                case "seaweed":
                    canvas.Line(Seaweed, 0.08f, 0.36f, 0.92f, 0.26f, 0.70f, 0.42f, 0.50f, 0.30f, 0.28f, 0.42f, 0.08f);
                    canvas.Line(DarkLeaf, 0.07f, 0.62f, 0.92f, 0.72f, 0.72f, 0.58f, 0.52f, 0.70f, 0.32f, 0.62f, 0.16f);
                    break;
                case "coconut":
                    canvas.Ellipse(Coconut, 0.50f, 0.52f, 0.32f, 0.32f);
                    canvas.Ellipse(DarkStone, 0.42f, 0.40f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.58f, 0.40f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.50f, 0.54f, 0.04f, 0.04f);
                    break;
                case "palm-frond":
                    canvas.Line(Wood, 0.04f, 0.15f, 0.85f, 0.85f, 0.15f);
                    for (int leaf = 1; leaf <= 5; leaf++)
                    {
                        float along = 0.15f + (leaf * 0.12f);
                        canvas.Line(Leaf, 0.05f, along, 1f - along, along - 0.12f, 1f - along - 0.14f);
                        canvas.Line(Leaf, 0.05f, along, 1f - along, along + 0.14f, 1f - along + 0.12f);
                    }

                    break;
                case "seashell":
                    canvas.Polygon(ShellPink, 0.50f, 0.85f, 0.15f, 0.45f, 0.30f, 0.22f, 0.50f, 0.15f, 0.70f, 0.22f, 0.85f, 0.45f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.50f, 0.20f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.32f, 0.28f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.68f, 0.28f);
                    break;
                case "crab":
                    // Three legs a side, two clawed arms, and eyes on stalks.
                    foreach (float side in new[] { -1f, 1f })
                    {
                        canvas.Line(CrabRed, 0.05f, 0.50f + (0.18f * side), 0.62f, 0.50f + (0.40f * side), 0.70f);
                        canvas.Line(CrabRed, 0.05f, 0.50f + (0.18f * side), 0.66f, 0.50f + (0.38f * side), 0.82f);
                        canvas.Line(CrabRed, 0.05f, 0.50f + (0.14f * side), 0.70f, 0.50f + (0.28f * side), 0.92f);
                        canvas.Line(CrabRed, 0.06f, 0.50f + (0.16f * side), 0.50f, 0.50f + (0.30f * side), 0.30f);
                        canvas.Ellipse(CrabRed, 0.50f + (0.32f * side), 0.22f, 0.10f, 0.08f);
                        canvas.Line(DarkStone, 0.03f, 0.50f + (0.07f * side), 0.48f, 0.50f + (0.08f * side), 0.38f);
                    }

                    canvas.Ellipse(CrabRed, 0.50f, 0.58f, 0.24f, 0.16f);
                    break;
                case "lobster":
                    foreach (float side in new[] { -1f, 1f })
                    {
                        canvas.Line(CrabRed, 0.05f, 0.50f + (0.06f * side), 0.30f, 0.50f + (0.22f * side), 0.16f);
                        canvas.Ellipse(CrabRed, 0.50f + (0.26f * side), 0.12f, 0.09f, 0.07f);
                        canvas.Line(CrabRed, 0.04f, 0.50f + (0.08f * side), 0.46f, 0.50f + (0.22f * side), 0.52f);
                        canvas.Line(CrabRed, 0.04f, 0.50f + (0.08f * side), 0.54f, 0.50f + (0.22f * side), 0.62f);
                    }

                    canvas.Line(CrabRed, 0.18f, 0.50f, 0.30f, 0.50f, 0.78f);
                    canvas.Polygon(CrabRed, 0.50f, 0.78f, 0.30f, 0.94f, 0.70f, 0.94f);
                    canvas.Line(DarkCrabRed, 0.03f, 0.42f, 0.58f, 0.58f, 0.58f);
                    canvas.Line(DarkCrabRed, 0.03f, 0.42f, 0.68f, 0.58f, 0.68f);
                    break;
                case "conch":
                    canvas.Polygon(ShellOrange, 0.20f, 0.80f, 0.40f, 0.25f, 0.62f, 0.12f, 0.82f, 0.30f, 0.70f, 0.55f, 0.45f, 0.85f);
                    canvas.Ellipse(ShellPink, 0.58f, 0.40f, 0.10f, 0.16f);
                    break;
                case "clam":
                    canvas.Polygon(Clam, 0.50f, 0.20f, 0.20f, 0.36f, 0.12f, 0.62f, 0.26f, 0.82f, 0.74f, 0.82f, 0.88f, 0.62f, 0.80f, 0.36f);
                    canvas.Line(ClamRidge, 0.03f, 0.50f, 0.22f, 0.50f, 0.80f);
                    canvas.Line(ClamRidge, 0.03f, 0.50f, 0.22f, 0.30f, 0.76f);
                    canvas.Line(ClamRidge, 0.03f, 0.50f, 0.22f, 0.70f, 0.76f);
                    break;
                case "oyster":
                    canvas.Polygon(Oyster, 0.18f, 0.40f, 0.36f, 0.20f, 0.62f, 0.22f, 0.84f, 0.42f, 0.80f, 0.70f, 0.58f, 0.86f, 0.30f, 0.80f, 0.14f, 0.62f);
                    canvas.Line(OysterRidge, 0.04f, 0.26f, 0.48f, 0.48f, 0.40f, 0.72f, 0.50f);
                    canvas.Line(OysterRidge, 0.04f, 0.28f, 0.64f, 0.52f, 0.60f, 0.70f, 0.68f);
                    break;
                case "gooseneck-barnacle":
                    foreach (float x in new[] { 0.32f, 0.50f, 0.68f })
                    {
                        canvas.Line(BarnacleStalk, 0.08f, 0.50f, 0.90f, x, 0.42f);
                        canvas.Polygon(White, x - 0.08f, 0.44f, x, 0.14f, x + 0.08f, 0.44f);
                    }

                    break;
                case "coral":
                    canvas.Line(White, 0.07f, 0.50f, 0.88f, 0.50f, 0.20f);
                    canvas.Line(White, 0.06f, 0.50f, 0.55f, 0.28f, 0.30f);
                    canvas.Line(White, 0.06f, 0.50f, 0.45f, 0.74f, 0.22f);
                    break;
                case "driftwood":
                    canvas.Polygon(PaleWood, 0.12f, 0.58f, 0.25f, 0.40f, 0.85f, 0.35f, 0.88f, 0.55f, 0.30f, 0.66f);
                    canvas.Line(Wood, 0.03f, 0.35f, 0.50f, 0.78f, 0.45f);
                    break;
                case "bone":
                    canvas.Line(Bone, 0.10f, 0.26f, 0.74f, 0.74f, 0.26f);
                    canvas.Ellipse(Bone, 0.18f, 0.74f, 0.08f, 0.08f);
                    canvas.Ellipse(Bone, 0.26f, 0.84f, 0.08f, 0.08f);
                    canvas.Ellipse(Bone, 0.74f, 0.16f, 0.08f, 0.08f);
                    canvas.Ellipse(Bone, 0.84f, 0.26f, 0.08f, 0.08f);
                    break;
                case "feather":
                    canvas.Polygon(White, 0.22f, 0.86f, 0.40f, 0.45f, 0.70f, 0.14f, 0.62f, 0.50f);
                    canvas.Line(Gray, 0.025f, 0.20f, 0.88f, 0.66f, 0.18f);
                    break;
                case "cone-snail":
                    canvas.Polygon(ConeTan, 0.26f, 0.30f, 0.74f, 0.30f, 0.50f, 0.92f);
                    canvas.Polygon(ConeTan, 0.34f, 0.30f, 0.50f, 0.12f, 0.66f, 0.30f);
                    canvas.Line(ConeBrown, 0.04f, 0.30f, 0.40f, 0.70f, 0.40f);
                    canvas.Ellipse(ConeBrown, 0.42f, 0.54f, 0.05f, 0.05f);
                    canvas.Ellipse(ConeBrown, 0.58f, 0.62f, 0.05f, 0.05f);
                    canvas.Ellipse(ConeBrown, 0.48f, 0.74f, 0.04f, 0.04f);
                    break;
                case "sea-sponge":
                    canvas.Ellipse(Sponge, 0.50f, 0.55f, 0.28f, 0.26f);
                    canvas.Ellipse(Coconut, 0.40f, 0.48f, 0.04f, 0.04f);
                    canvas.Ellipse(Coconut, 0.58f, 0.62f, 0.04f, 0.04f);
                    break;
                case "sand-dollar":
                    canvas.Ellipse(Sand, 0.50f, 0.50f, 0.32f, 0.32f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.50f, 0.30f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.68f, 0.44f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.32f, 0.44f);
                    break;
                case "small-palm-tree":
                    PalmTree(canvas, 0.55f);
                    break;
                case "medium-palm-tree":
                    PalmTree(canvas, 0.75f);
                    break;
                case "large-palm-tree":
                    PalmTree(canvas, 0.95f);
                    break;
                case "beach-bush":
                    canvas.Ellipse(DarkLeaf, 0.32f, 0.62f, 0.20f, 0.18f);
                    canvas.Ellipse(DarkLeaf, 0.68f, 0.62f, 0.20f, 0.18f);
                    canvas.Ellipse(Leaf, 0.50f, 0.48f, 0.24f, 0.22f);
                    break;
                case "rock-outcrop":
                    canvas.Polygon(DarkStone, 0.08f, 0.85f, 0.22f, 0.40f, 0.40f, 0.22f, 0.58f, 0.35f, 0.75f, 0.20f, 0.92f, 0.85f);
                    canvas.Polygon(Stone, 0.30f, 0.85f, 0.42f, 0.45f, 0.58f, 0.52f, 0.70f, 0.85f);
                    break;
                case "tide-pool":
                    canvas.Ellipse(DarkStone, 0.50f, 0.55f, 0.42f, 0.30f);
                    canvas.Ellipse(DeepWater, 0.50f, 0.55f, 0.34f, 0.22f);
                    canvas.Ellipse(Water, 0.44f, 0.50f, 0.18f, 0.10f);
                    break;
                case "driftwood-pile":
                    canvas.Line(PaleWood, 0.12f, 0.15f, 0.72f, 0.85f, 0.62f);
                    canvas.Line(PaleWood, 0.12f, 0.20f, 0.50f, 0.80f, 0.42f);
                    canvas.Line(PaleWood, 0.10f, 0.30f, 0.30f, 0.70f, 0.28f);
                    break;
                case "puddle":
                    canvas.Ellipse(Water, 0.50f, 0.62f, 0.40f, 0.16f);
                    canvas.Ellipse(DeepWater, 0.58f, 0.64f, 0.16f, 0.06f);
                    break;
                default:
                    // Anything without a placeholder yet shows a plain square, so it is visible and obviously unfinished.
                    canvas.Polygon(Unknown, 0.25f, 0.25f, 0.75f, 0.25f, 0.75f, 0.75f, 0.25f, 0.75f);
                    return false;
            }

            return true;
        }

        private static void PalmTree(PixelIconCanvas canvas, float height)
        {
            float top = 0.92f - (0.80f * height);
            canvas.Line(Wood, 0.08f, 0.50f, 0.92f, 0.46f, (0.92f + top) / 2f, 0.52f, top);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.22f, top + 0.12f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.82f, top + 0.12f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.30f, top - 0.04f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.74f, top - 0.04f);
            canvas.Ellipse(Coconut, 0.52f, top + 0.04f, 0.05f, 0.05f);
        }
    }
}
