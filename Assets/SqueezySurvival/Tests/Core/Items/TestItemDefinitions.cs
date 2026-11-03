using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    /// <summary>
    /// Items with known names, categories and weights for tests that need real-looking things. They are the tests' own,
    /// so balancing the shipped content never breaks an unrelated test (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public static class TestItemDefinitions
    {
        public static readonly ItemDefinition Stone = new ItemDefinition("stone", "Stone", "a stone", "stones", "A small, ordinary stone.", ItemCategory.StoneAndMinerals, 1.0m);
        public static readonly ItemDefinition Coral = new ItemDefinition("coral", "Coral", "a piece of coral", "pieces of coral", "A piece of dried coral.", ItemCategory.StoneAndMinerals, 0.2m);
        public static readonly ItemDefinition SmallStick = new ItemDefinition("small-stick", "Small stick", "a small stick", "small sticks", "A branch as thick as a thumb.", ItemCategory.Wood, 0.25m);
        public static readonly ItemDefinition Log = new ItemDefinition("log", "Log", "a log", "logs", "Part of a tree trunk.", ItemCategory.Wood, 30.0m);
        public static readonly ItemDefinition Seashell = new ItemDefinition("seashell", "Seashell", "a seashell", "seashells", "A small, ridged shell.", ItemCategory.Shells, 0.1m);
        public static readonly ItemDefinition Conch = new ItemDefinition("conch", "Conch", "a conch", "conches", "A heavy spiral shell.", ItemCategory.Shells, 0.75m);
        public static readonly ItemDefinition Coconut = new ItemDefinition("coconut", "Coconut", "a coconut", "coconuts", "A coconut in its husk.", ItemCategory.Fruit, 3.0m);
    }
}
