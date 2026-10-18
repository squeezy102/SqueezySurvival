using System.Collections.Generic;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Navigation
{
    /// <summary>The nav bar's buttons, top to bottom (Docs/Design/UI/ScreenLayout.md, Nav bar).</summary>
    public static class NavigationBarEntries
    {
        /// <summary>Options, which sits apart at the bottom of the bar.</summary>
        public static readonly NavigationBarEntry Options =
            new NavigationBarEntry("options", "Options", "Game settings.", NavigationIconCatalog.Options, isBuilt: true);

        /// <summary>The gameplay windows, in bar order. The inventory has none: it is always on screen.</summary>
        public static readonly IReadOnlyList<NavigationBarEntry> GameplayWindows = new[]
        {
            new NavigationBarEntry("map", "Map", "Where you've been, and what you know is out there.", NavigationIconCatalog.Map, isBuilt: false),
            new NavigationBarEntry("crafting", "Crafting", "Make things from what you've found.", NavigationIconCatalog.Crafting, isBuilt: false),
            new NavigationBarEntry("skills", "Skills", "What you're good at, and how good.", NavigationIconCatalog.Skills, isBuilt: false),
            new NavigationBarEntry("research", "Research", "What you've learned, and what you could learn next.", NavigationIconCatalog.Research, isBuilt: false),
            new NavigationBarEntry("equipment", "Equipment", "What you're wearing and holding.", NavigationIconCatalog.Equipment, isBuilt: false),
            new NavigationBarEntry("cooking", "Cooking", "Prepare food.", NavigationIconCatalog.Cooking, isBuilt: false),
        };
    }
}
