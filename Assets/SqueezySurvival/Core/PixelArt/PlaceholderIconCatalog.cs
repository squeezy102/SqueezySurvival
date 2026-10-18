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
        private static readonly PixelColor SeaGlass = new PixelColor(115, 204, 158);
        private static readonly PixelColor Sand = new PixelColor(230, 214, 168);
        private static readonly PixelColor Starfish = new PixelColor(242, 140, 76);
        private static readonly PixelColor Sponge = new PixelColor(235, 199, 89);
        private static readonly PixelColor Rope = new PixelColor(204, 173, 115);
        private static readonly PixelColor Bamboo = new PixelColor(178, 191, 89);

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
                case "stick":
                    canvas.Line(Wood, 0.09f, 0.18f, 0.82f, 0.82f, 0.18f);
                    canvas.Line(Wood, 0.06f, 0.50f, 0.50f, 0.66f, 0.26f);
                    break;
                case "seaweed-strand":
                    canvas.Line(Seaweed, 0.08f, 0.50f, 0.90f, 0.38f, 0.70f, 0.58f, 0.50f, 0.42f, 0.30f, 0.56f, 0.10f);
                    break;
                case "seashell":
                    canvas.Polygon(ShellPink, 0.50f, 0.85f, 0.15f, 0.45f, 0.30f, 0.22f, 0.50f, 0.15f, 0.70f, 0.22f, 0.85f, 0.45f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.50f, 0.20f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.32f, 0.28f);
                    canvas.Line(ShellOrange, 0.03f, 0.50f, 0.82f, 0.68f, 0.28f);
                    break;
                case "conch":
                    canvas.Polygon(ShellOrange, 0.20f, 0.80f, 0.40f, 0.25f, 0.62f, 0.12f, 0.82f, 0.30f, 0.70f, 0.55f, 0.45f, 0.85f);
                    canvas.Ellipse(ShellPink, 0.58f, 0.40f, 0.10f, 0.16f);
                    break;
                case "coconut":
                    canvas.Ellipse(Coconut, 0.50f, 0.52f, 0.32f, 0.32f);
                    canvas.Ellipse(DarkStone, 0.42f, 0.40f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.58f, 0.40f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.50f, 0.54f, 0.04f, 0.04f);
                    break;
                case "driftwood-chunk":
                    canvas.Polygon(PaleWood, 0.12f, 0.58f, 0.25f, 0.40f, 0.85f, 0.35f, 0.88f, 0.55f, 0.30f, 0.66f);
                    canvas.Line(Wood, 0.03f, 0.35f, 0.50f, 0.78f, 0.45f);
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
                case "feather":
                    canvas.Polygon(White, 0.22f, 0.86f, 0.40f, 0.45f, 0.70f, 0.14f, 0.62f, 0.50f);
                    canvas.Line(Gray, 0.025f, 0.20f, 0.88f, 0.66f, 0.18f);
                    break;
                case "crab-shell":
                    canvas.Ellipse(CrabRed, 0.50f, 0.56f, 0.30f, 0.20f);
                    canvas.Line(CrabRed, 0.05f, 0.24f, 0.50f, 0.14f, 0.32f);
                    canvas.Line(CrabRed, 0.05f, 0.76f, 0.50f, 0.86f, 0.32f);
                    break;
                case "sea-glass":
                    canvas.Polygon(SeaGlass, 0.30f, 0.35f, 0.62f, 0.25f, 0.76f, 0.55f, 0.55f, 0.76f, 0.26f, 0.62f);
                    break;
                case "sand-dollar":
                    canvas.Ellipse(Sand, 0.50f, 0.50f, 0.32f, 0.32f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.50f, 0.30f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.68f, 0.44f);
                    canvas.Line(Gray, 0.03f, 0.50f, 0.50f, 0.32f, 0.44f);
                    break;
                case "starfish":
                    canvas.Polygon(Starfish, 0.50f, 0.12f, 0.59f, 0.40f, 0.88f, 0.40f, 0.64f, 0.57f, 0.73f, 0.86f, 0.50f, 0.68f, 0.27f, 0.86f, 0.36f, 0.57f, 0.12f, 0.40f, 0.41f, 0.40f);
                    break;
                case "pumice":
                    canvas.Ellipse(Gray, 0.50f, 0.55f, 0.30f, 0.22f);
                    canvas.Ellipse(DarkStone, 0.42f, 0.50f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.60f, 0.60f, 0.04f, 0.04f);
                    canvas.Ellipse(DarkStone, 0.52f, 0.46f, 0.03f, 0.03f);
                    break;
                case "coral-fragment":
                    canvas.Line(White, 0.07f, 0.50f, 0.88f, 0.50f, 0.20f);
                    canvas.Line(White, 0.06f, 0.50f, 0.55f, 0.28f, 0.30f);
                    canvas.Line(White, 0.06f, 0.50f, 0.45f, 0.74f, 0.22f);
                    break;
                case "sea-sponge":
                    canvas.Ellipse(Sponge, 0.50f, 0.55f, 0.28f, 0.26f);
                    canvas.Ellipse(Coconut, 0.40f, 0.48f, 0.04f, 0.04f);
                    canvas.Ellipse(Coconut, 0.58f, 0.62f, 0.04f, 0.04f);
                    break;
                case "cuttlebone":
                    canvas.Ellipse(White, 0.50f, 0.50f, 0.16f, 0.36f);
                    break;
                case "rope-scrap":
                    canvas.Line(Rope, 0.07f, 0.15f, 0.70f, 0.35f, 0.40f, 0.55f, 0.65f, 0.75f, 0.35f, 0.88f, 0.55f);
                    break;
                case "glass-float":
                    canvas.Ellipse(SeaGlass, 0.50f, 0.52f, 0.30f, 0.30f);
                    canvas.Ellipse(White, 0.40f, 0.42f, 0.07f, 0.07f);
                    break;
                case "bamboo-segment":
                    canvas.Line(Bamboo, 0.16f, 0.20f, 0.80f, 0.80f, 0.20f);
                    canvas.Line(Wood, 0.17f, 0.48f, 0.52f, 0.52f, 0.48f);
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
