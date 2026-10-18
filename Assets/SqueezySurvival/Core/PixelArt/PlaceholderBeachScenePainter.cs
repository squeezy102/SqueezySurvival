namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// Paints the placeholder beach backdrop as pixel art, in the same style as the icons, until real scene art
    /// exists (Docs/Design/ArtDirection.md, Backgrounds): a dithered sky with clouds and the sun, a banded sea with
    /// glints, and a leaning palm, shaded and outlined like an icon.
    /// </summary>
    public static class PlaceholderBeachScenePainter
    {
        /// <summary>Width in art pixels: a quarter of the 1920 x 1080 reference resolution.</summary>
        public const int Width = 480;

        /// <summary>Height in art pixels.</summary>
        public const int Height = 270;

        /// <summary>The row where the sky meets the sea.</summary>
        public const int Horizon = 152;

        private static readonly PixelColor[] SkyBands =
        {
            new PixelColor(78, 140, 214),
            new PixelColor(96, 156, 222),
            new PixelColor(116, 172, 228),
            new PixelColor(138, 188, 234),
            new PixelColor(160, 202, 238),
            new PixelColor(184, 216, 240),
            new PixelColor(206, 228, 242),
        };

        private static readonly PixelColor[] SeaBands =
        {
            new PixelColor(30, 92, 140),
            new PixelColor(36, 108, 152),
            new PixelColor(42, 124, 162),
            new PixelColor(50, 140, 170),
            new PixelColor(58, 154, 176),
        };

        private static readonly PixelColor SunCore = new PixelColor(255, 248, 220);
        private static readonly PixelColor SunRim = new PixelColor(252, 232, 168);
        private static readonly PixelColor Cloud = new PixelColor(246, 250, 255);
        private static readonly PixelColor CloudShade = new PixelColor(206, 222, 240);
        private static readonly PixelColor Glint = new PixelColor(170, 216, 230);
        private static readonly PixelColor Trunk = new PixelColor(122, 84, 52);
        private static readonly PixelColor TrunkRing = new PixelColor(98, 66, 40);
        private static readonly PixelColor Frond = new PixelColor(52, 128, 64);
        private static readonly PixelColor FrondDark = new PixelColor(36, 98, 52);

        /// <summary>Paints the backdrop.</summary>
        public static PixelImage Create()
        {
            var scene = new PixelImage(Width, Height);
            var canvas = new PixelSceneCanvas(scene);
            canvas.DitheredBands(0, Horizon, SkyBands);
            canvas.DitheredBands(Horizon, Height, SeaBands);

            canvas.Ellipse(SunRim, 400f, 62f, 25f, 25f);
            canvas.Ellipse(SunCore, 400f, 62f, 21f, 21f);
            PaintCloud(canvas, 30f, 52f, 1.0f);
            PaintCloud(canvas, 160f, 38f, 0.8f);
            PaintCloud(canvas, 250f, 80f, 0.9f);
            PaintGlints(canvas);

            PixelImageCompositor.DrawOver(scene, PixelIconStylizer.Stylize(PaintPalm()));
            return scene;
        }

        // A cloud is a few overlapping puffs with a shaded underside.
        private static void PaintCloud(PixelSceneCanvas canvas, float left, float baseline, float scale)
        {
            canvas.Ellipse(CloudShade, left + (24f * scale), baseline, 26f * scale, 9f * scale);
            canvas.Ellipse(CloudShade, left + (50f * scale), baseline - (2f * scale), 20f * scale, 9f * scale);
            canvas.Ellipse(Cloud, left + (22f * scale), baseline - (5f * scale), 20f * scale, 9f * scale);
            canvas.Ellipse(Cloud, left + (40f * scale), baseline - (12f * scale), 15f * scale, 13f * scale);
            canvas.Ellipse(Cloud, left + (56f * scale), baseline - (7f * scale), 13f * scale, 8f * scale);
        }

        // Short light dashes on the water, placed by a fixed pseudo-random sequence so the scene never changes.
        private static void PaintGlints(PixelSceneCanvas canvas)
        {
            uint state = 2463534242;
            for (int index = 0; index < 70; index++)
            {
                state = NextXorshift(state);
                int y = Horizon + 3 + (int)(state % (uint)(Height - Horizon - 6));
                state = NextXorshift(state);
                int x = (int)(state % Width);
                state = NextXorshift(state);
                int length = 3 + (int)(state % 8) + ((y - Horizon) / 12);
                canvas.HorizontalRun(Glint, x, y, length);
            }
        }

        // Marsaglia's xorshift32: a fixed, portable sequence, so the glints are the same on every platform.
        private static uint NextXorshift(uint state)
        {
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;
            return state;
        }

        // The palm is drawn on its own layer so it can be shaded and outlined like an icon.
        private static PixelImage PaintPalm()
        {
            var layer = new PixelImage(Width, Height);
            var canvas = new PixelSceneCanvas(layer);
            float[] trunk = { 150f, 280f, 190f, 238f, 236f, 198f, 284f, 168f, 318f, 152f };
            canvas.Line(Trunk, 14f, trunk);
            for (float along = 0.08f; along < 1f; along += 0.09f)
            {
                PointOnPolyline(trunk, along, out float x, out float y);
                canvas.Line(TrunkRing, 3f, x - 5f, y - 5f, x + 5f, y + 5f);
            }

            PaintFrond(canvas, FrondDark, 318f, 150f, 286f, 140f, 254f, 150f, 236f, 166f);
            PaintFrond(canvas, Frond, 318f, 150f, 296f, 128f, 268f, 124f, 248f, 132f);
            PaintFrond(canvas, Frond, 318f, 150f, 334f, 128f, 362f, 124f, 384f, 136f);
            PaintFrond(canvas, FrondDark, 318f, 150f, 352f, 146f, 380f, 162f, 394f, 184f);
            PaintFrond(canvas, Frond, 318f, 150f, 340f, 156f, 356f, 176f, 362f, 200f);
            canvas.Ellipse(new PixelColor(104, 72, 42), 316f, 156f, 6f, 5f);
            return layer;
        }

        // A frond is a stem with leaflets hanging from it, longest near the middle.
        private static void PaintFrond(PixelSceneCanvas canvas, PixelColor color, params float[] stem)
        {
            canvas.Line(color, 4f, stem);
            for (float along = 0.12f; along < 0.98f; along += 0.07f)
            {
                PointOnPolyline(stem, along, out float x, out float y);
                float length = 4f + (10f * (1f - System.Math.Abs((along * 2f) - 1f)));
                canvas.Line(color, 3f, x, y, x - (length * 0.35f), y + length);
                canvas.Line(color, 3f, x, y, x + (length * 0.35f), y + length);
            }
        }

        private static void PointOnPolyline(float[] points, float fraction, out float x, out float y)
        {
            int segments = (points.Length / 2) - 1;
            float position = fraction * segments;
            int segment = System.Math.Min(segments - 1, (int)position);
            float local = position - segment;
            x = points[segment * 2] + ((points[(segment * 2) + 2] - points[segment * 2]) * local);
            y = points[(segment * 2) + 1] + ((points[(segment * 2) + 3] - points[(segment * 2) + 1]) * local);
        }
    }
}
