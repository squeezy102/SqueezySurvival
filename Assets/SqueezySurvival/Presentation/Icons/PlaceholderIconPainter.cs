using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Icons
{
    /// <summary>
    /// Draws throwaway placeholder icons for items and zone features with simple vector shapes, until real icon art
    /// exists (Docs/Design/UI/GridPanels.md, The grid). Shapes are described on a 1 x 1 square and scaled to fit.
    /// </summary>
    public static class PlaceholderIconPainter
    {
        private const int CirclePointCount = 28;

        private static readonly Color Stone = new Color(0.62f, 0.62f, 0.60f);
        private static readonly Color DarkStone = new Color(0.42f, 0.42f, 0.42f);
        private static readonly Color Wood = new Color(0.55f, 0.36f, 0.18f);
        private static readonly Color PaleWood = new Color(0.72f, 0.62f, 0.48f);
        private static readonly Color Leaf = new Color(0.25f, 0.62f, 0.25f);
        private static readonly Color DarkLeaf = new Color(0.16f, 0.45f, 0.20f);
        private static readonly Color Seaweed = new Color(0.30f, 0.50f, 0.20f);
        private static readonly Color ShellPink = new Color(0.96f, 0.76f, 0.70f);
        private static readonly Color ShellOrange = new Color(0.95f, 0.62f, 0.38f);
        private static readonly Color Coconut = new Color(0.45f, 0.30f, 0.17f);
        private static readonly Color Water = new Color(0.30f, 0.62f, 0.86f);
        private static readonly Color DeepWater = new Color(0.16f, 0.40f, 0.66f);
        private static readonly Color Unknown = new Color(0.85f, 0.25f, 0.75f);
        private static readonly Color White = new Color(0.94f, 0.94f, 0.92f);
        private static readonly Color Gray = new Color(0.55f, 0.58f, 0.62f);
        private static readonly Color CrabRed = new Color(0.86f, 0.35f, 0.22f);
        private static readonly Color SeaGlass = new Color(0.45f, 0.80f, 0.62f);
        private static readonly Color Sand = new Color(0.90f, 0.84f, 0.66f);
        private static readonly Color Starfish = new Color(0.95f, 0.55f, 0.30f);
        private static readonly Color Sponge = new Color(0.92f, 0.78f, 0.35f);
        private static readonly Color Rope = new Color(0.80f, 0.68f, 0.45f);
        private static readonly Color Bamboo = new Color(0.70f, 0.75f, 0.35f);

        /// <summary>Draws the icon for the thing with the given definition id inside <paramref name="area"/>.</summary>
        public static void Paint(Painter2D painter, Rect area, string definitionId)
        {
            var canvas = new IconCanvas(painter, area);
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
                    break;
            }
        }

        private static void PalmTree(IconCanvas canvas, float height)
        {
            float top = 0.92f - (0.80f * height);
            canvas.Line(Wood, 0.08f, 0.50f, 0.92f, 0.46f, (0.92f + top) / 2f, 0.52f, top);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.22f, top + 0.12f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.82f, top + 0.12f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.30f, top - 0.04f);
            canvas.Line(Leaf, 0.06f, 0.52f, top, 0.74f, top - 0.04f);
            canvas.Ellipse(Coconut, 0.52f, top + 0.04f, 0.05f, 0.05f);
        }

        /// <summary>Maps shapes on a 1 x 1 square onto the icon's area.</summary>
        private readonly struct IconCanvas
        {
            private readonly Painter2D _painter;
            private readonly Rect _area;

            public IconCanvas(Painter2D painter, Rect area)
            {
                _painter = painter;
                _area = area;
            }

            private float Scale => Mathf.Min(_area.width, _area.height);

            public void Polygon(Color color, params float[] coordinates)
            {
                _painter.fillColor = color;
                _painter.BeginPath();
                _painter.MoveTo(Point(coordinates[0], coordinates[1]));
                for (int index = 2; index < coordinates.Length; index += 2)
                {
                    _painter.LineTo(Point(coordinates[index], coordinates[index + 1]));
                }

                _painter.ClosePath();
                _painter.Fill();
            }

            public void Ellipse(Color color, float centerX, float centerY, float radiusX, float radiusY)
            {
                _painter.fillColor = color;
                _painter.BeginPath();
                for (int index = 0; index < CirclePointCount; index++)
                {
                    float angle = index * Mathf.PI * 2f / CirclePointCount;
                    Vector2 point = Point(centerX + (Mathf.Cos(angle) * radiusX), centerY + (Mathf.Sin(angle) * radiusY));
                    if (index == 0)
                    {
                        _painter.MoveTo(point);
                    }
                    else
                    {
                        _painter.LineTo(point);
                    }
                }

                _painter.ClosePath();
                _painter.Fill();
            }

            public void Line(Color color, float width, params float[] coordinates)
            {
                _painter.strokeColor = color;
                _painter.lineWidth = width * Scale;
                _painter.lineCap = LineCap.Round;
                _painter.lineJoin = LineJoin.Round;
                _painter.BeginPath();
                _painter.MoveTo(Point(coordinates[0], coordinates[1]));
                for (int index = 2; index < coordinates.Length; index += 2)
                {
                    _painter.LineTo(Point(coordinates[index], coordinates[index + 1]));
                }

                _painter.Stroke();
            }

            // Centers the unit square in the area, so icons stay square in any cell shape.
            private Vector2 Point(float x, float y) => new Vector2(
                _area.x + ((_area.width - Scale) / 2f) + (x * Scale),
                _area.y + ((_area.height - Scale) / 2f) + (y * Scale));
        }
    }
}
