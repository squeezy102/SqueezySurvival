namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// What kind of thing an item is, for tooltips and sorting by type. Declared in the order "by type" sorts
    /// (Docs/Design/Systems/Inventory.md, Item categories).
    /// </summary>
    public enum ItemCategory
    {
        /// <summary>Sticks, logs, driftwood, bamboo.</summary>
        Wood,

        /// <summary>Seaweed, fronds, leaves, grass, bark.</summary>
        PlantMatter,

        /// <summary>Stones, flint, pumice, coral, clay, sand.</summary>
        StoneAndMinerals,

        /// <summary>Seashells, conches, sand dollars.</summary>
        Shells,

        /// <summary>Coconuts, bananas, berries.</summary>
        Fruit,

        /// <summary>Roots, tubers, edible leaves.</summary>
        Vegetables,

        /// <summary>Raw and cooked meat, fish, crab meat.</summary>
        MeatAndFish,

        /// <summary>Feathers, bones, hide, crab shells, starfish, sponges.</summary>
        AnimalParts,

        /// <summary>Live animals.</summary>
        Animals,

        /// <summary>Whole dead animals, to be butchered.</summary>
        Carcasses,

        /// <summary>Washed-up man-made things.</summary>
        Salvage,

        /// <summary>Made intermediates, such as cordage.</summary>
        CraftingMaterials,

        /// <summary>Knives, axes, spears, slings.</summary>
        ToolsAndWeapons,
    }
}
