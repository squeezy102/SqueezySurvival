using System;

namespace SqueezySurvival.Core.PixelArt
{
    /// <summary>
    /// The icons of the nav bar's buttons, drawn and styled like item icons (Docs/Design/UI/ScreenLayout.md, Nav bar;
    /// Docs/Design/ArtDirection.md, Icons). Shapes are described on a 1 x 1 square.
    /// </summary>
    public static class NavigationIconCatalog
    {
        /// <summary>The map: a folded map.</summary>
        public const string Map = "nav-map";

        /// <summary>Crafting: a hammer and anvil.</summary>
        public const string Crafting = "nav-crafting";

        /// <summary>Skills: a book.</summary>
        public const string Skills = "nav-skills";

        /// <summary>Research: a vial and an atom.</summary>
        public const string Research = "nav-research";

        /// <summary>Equipment: a crossed sword and pickaxe.</summary>
        public const string Equipment = "nav-equipment";

        /// <summary>Cooking: a steaming pot.</summary>
        public const string Cooking = "nav-cooking";

        /// <summary>Options: a gear.</summary>
        public const string Options = "nav-options";

        private const int OutlineMargin = 1;

        private static readonly PixelColor Parchment = new PixelColor(232, 214, 166);
        private static readonly PixelColor Fold = new PixelColor(190, 168, 120);
        private static readonly PixelColor MarkRed = new PixelColor(210, 52, 44);
        private static readonly PixelColor Iron = new PixelColor(124, 130, 142);
        private static readonly PixelColor Steel = new PixelColor(196, 204, 216);
        private static readonly PixelColor Wood = new PixelColor(150, 98, 52);
        private static readonly PixelColor BookCover = new PixelColor(150, 52, 52);
        private static readonly PixelColor Pages = new PixelColor(244, 238, 220);
        private static readonly PixelColor Gold = new PixelColor(230, 196, 96);
        private static readonly PixelColor Glass = new PixelColor(196, 228, 240);
        private static readonly PixelColor Potion = new PixelColor(92, 204, 120);
        private static readonly PixelColor Orbit = new PixelColor(120, 196, 240);
        private static readonly PixelColor Nucleus = new PixelColor(240, 96, 80);
        private static readonly PixelColor Pot = new PixelColor(78, 82, 94);
        private static readonly PixelColor Steam = new PixelColor(226, 230, 240);
        private static readonly PixelColor GearMetal = new PixelColor(170, 176, 190);
        private static readonly PixelColor GearHole = new PixelColor(70, 74, 88);

        /// <summary>The finished icon for a nav button: drawn, shaded and outlined.</summary>
        public static PixelImage CreateIcon(string iconId)
        {
            var flat = new PixelImage(PlaceholderIconCatalog.IconSize, PlaceholderIconCatalog.IconSize);
            var canvas = new PixelIconCanvas(flat, OutlineMargin);
            switch (iconId)
            {
                case Map:
                    canvas.Polygon(Parchment, 0.06f, 0.24f, 0.36f, 0.14f, 0.64f, 0.24f, 0.94f, 0.14f, 0.94f, 0.78f, 0.64f, 0.88f, 0.36f, 0.78f, 0.06f, 0.88f);
                    canvas.Line(Fold, 0.04f, 0.36f, 0.16f, 0.36f, 0.78f);
                    canvas.Line(Fold, 0.04f, 0.64f, 0.26f, 0.64f, 0.86f);
                    canvas.Line(Fold, 0.04f, 0.18f, 0.70f, 0.28f, 0.56f, 0.42f, 0.60f, 0.52f, 0.48f);
                    canvas.Line(MarkRed, 0.07f, 0.66f, 0.40f, 0.80f, 0.56f);
                    canvas.Line(MarkRed, 0.07f, 0.80f, 0.40f, 0.66f, 0.56f);
                    break;
                case Crafting:
                    canvas.Polygon(Iron, 0.04f, 0.56f, 0.88f, 0.56f, 0.80f, 0.68f, 0.64f, 0.70f, 0.66f, 0.84f, 0.80f, 0.92f, 0.20f, 0.92f, 0.34f, 0.84f, 0.36f, 0.70f, 0.20f, 0.68f);
                    canvas.Line(Wood, 0.08f, 0.26f, 0.50f, 0.66f, 0.12f);
                    canvas.Polygon(Steel, 0.52f, 0.10f, 0.68f, 0.02f, 0.86f, 0.22f, 0.70f, 0.30f);
                    break;
                case Skills:
                    canvas.Polygon(Pages, 0.26f, 0.16f, 0.86f, 0.18f, 0.86f, 0.90f, 0.26f, 0.88f);
                    canvas.Polygon(BookCover, 0.14f, 0.10f, 0.78f, 0.10f, 0.78f, 0.84f, 0.14f, 0.84f);
                    canvas.Line(new PixelColor(110, 36, 36), 0.06f, 0.22f, 0.12f, 0.22f, 0.82f);
                    canvas.Ellipse(Gold, 0.50f, 0.40f, 0.12f, 0.12f);
                    canvas.Line(Gold, 0.04f, 0.34f, 0.66f, 0.66f, 0.66f);
                    break;
                case Research:
                    foreach (float angle in new[] { 35f, 145f })
                    {
                        canvas.Line(Orbit, 0.035f, RotatedEllipse(0.64f, 0.34f, 0.32f, 0.10f, angle));
                    }

                    canvas.Ellipse(Nucleus, 0.64f, 0.34f, 0.07f, 0.07f);
                    canvas.Polygon(Glass, 0.22f, 0.36f, 0.38f, 0.36f, 0.38f, 0.56f, 0.22f, 0.56f);
                    canvas.Ellipse(Glass, 0.30f, 0.74f, 0.26f, 0.22f);
                    canvas.Ellipse(Potion, 0.30f, 0.78f, 0.21f, 0.15f);
                    canvas.Line(Glass, 0.06f, 0.18f, 0.36f, 0.42f, 0.36f);
                    break;
                case Equipment:
                    canvas.Line(Wood, 0.08f, 0.84f, 0.14f, 0.16f, 0.88f);
                    canvas.Line(Iron, 0.09f, 0.52f, 0.08f, 0.76f, 0.06f, 0.94f, 0.28f, 0.96f, 0.48f);
                    canvas.Line(Steel, 0.09f, 0.10f, 0.10f, 0.72f, 0.72f);
                    canvas.Line(Gold, 0.07f, 0.62f, 0.84f, 0.84f, 0.62f);
                    canvas.Line(Wood, 0.08f, 0.76f, 0.76f, 0.90f, 0.90f);
                    break;
                case Cooking:
                    canvas.Line(Steam, 0.05f, 0.34f, 0.38f, 0.26f, 0.27f, 0.36f, 0.16f, 0.28f, 0.04f);
                    canvas.Line(Steam, 0.05f, 0.66f, 0.38f, 0.58f, 0.27f, 0.68f, 0.16f, 0.60f, 0.04f);
                    canvas.Polygon(Pot, 0.16f, 0.46f, 0.84f, 0.46f, 0.78f, 0.88f, 0.22f, 0.88f);
                    canvas.Line(Iron, 0.07f, 0.08f, 0.46f, 0.92f, 0.46f);
                    canvas.Line(Pot, 0.06f, 0.04f, 0.56f, 0.16f, 0.56f);
                    canvas.Line(Pot, 0.06f, 0.84f, 0.56f, 0.96f, 0.56f);
                    break;
                case Options:
                    canvas.Polygon(GearMetal, Gear(0.5f, 0.5f, 0.46f, 0.34f, 8));
                    canvas.Ellipse(GearHole, 0.50f, 0.50f, 0.14f, 0.14f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iconId), iconId, "No nav icon has that id.");
            }

            return PixelIconStylizer.Stylize(flat);
        }

        // An ellipse turned by an angle, as a closed polyline: x, y pairs.
        private static float[] RotatedEllipse(float centerX, float centerY, float radiusX, float radiusY, float angleDegrees)
        {
            const int Points = 32;
            double angle = angleDegrees * Math.PI / 180.0;
            var coordinates = new float[(Points + 1) * 2];
            for (int index = 0; index <= Points; index++)
            {
                double t = index * 2.0 * Math.PI / Points;
                double x = radiusX * Math.Cos(t);
                double y = radiusY * Math.Sin(t);
                coordinates[index * 2] = centerX + (float)((x * Math.Cos(angle)) - (y * Math.Sin(angle)));
                coordinates[(index * 2) + 1] = centerY + (float)((x * Math.Sin(angle)) + (y * Math.Cos(angle)));
            }

            return coordinates;
        }

        // A gear outline: square teeth alternating between the outer and inner radius, as x, y pairs.
        private static float[] Gear(float centerX, float centerY, float outerRadius, float innerRadius, int teeth)
        {
            int corners = teeth * 4;
            var coordinates = new float[corners * 2];
            for (int index = 0; index < corners; index++)
            {
                double t = index * 2.0 * Math.PI / corners;
                float radius = (index % 4) < 2 ? outerRadius : innerRadius;
                coordinates[index * 2] = centerX + (float)(radius * Math.Cos(t));
                coordinates[(index * 2) + 1] = centerY + (float)(radius * Math.Sin(t));
            }

            return coordinates;
        }
    }
}
