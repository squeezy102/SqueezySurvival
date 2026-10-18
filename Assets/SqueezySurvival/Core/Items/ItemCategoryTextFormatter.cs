using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>The name shown for each item category (Docs/Design/Systems/Inventory.md, Item categories).</summary>
    public static class ItemCategoryTextFormatter
    {
        /// <summary>The category's name, such as "Stone and minerals".</summary>
        public static string Format(ItemCategory category)
        {
            switch (category)
            {
                case ItemCategory.Wood:
                    return "Wood";
                case ItemCategory.PlantMatter:
                    return "Plant matter";
                case ItemCategory.StoneAndMinerals:
                    return "Stone and minerals";
                case ItemCategory.Shells:
                    return "Shells";
                case ItemCategory.Fruit:
                    return "Fruit";
                case ItemCategory.Vegetables:
                    return "Vegetables";
                case ItemCategory.MeatAndFish:
                    return "Meat and fish";
                case ItemCategory.AnimalParts:
                    return "Animal parts";
                case ItemCategory.Animals:
                    return "Animals";
                case ItemCategory.Carcasses:
                    return "Carcasses";
                case ItemCategory.Salvage:
                    return "Salvage";
                case ItemCategory.CraftingMaterials:
                    return "Crafting materials";
                case ItemCategory.ToolsAndWeapons:
                    return "Tools and weapons";
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, "Unknown item category.");
            }
        }
    }
}
