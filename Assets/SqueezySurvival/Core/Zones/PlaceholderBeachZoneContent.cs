using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// Placeholder content for the Beach, the starting zone, used to playtest exploring before real zone content is
    /// designed. Items and features come from the design's Beach examples; their weights are placeholders
    /// (Docs/Design/Systems/Exploration.md, Wireframe stage).
    /// </summary>
    public static class PlaceholderBeachZoneContent
    {
        /// <summary>A loose stone.</summary>
        public static readonly ItemDefinition Stone = new ItemDefinition("stone", "Stone", "a stone", "stones");

        /// <summary>A stick.</summary>
        public static readonly ItemDefinition Stick = new ItemDefinition("stick", "Stick", "a stick", "sticks");

        /// <summary>A strand of seaweed: the Beach's fibrous plant.</summary>
        public static readonly ItemDefinition SeaweedStrand = new ItemDefinition("seaweed-strand", "Seaweed strand", "a strand of seaweed", "strands of seaweed");

        /// <summary>A seashell.</summary>
        public static readonly ItemDefinition Seashell = new ItemDefinition("seashell", "Seashell", "a seashell", "seashells");

        /// <summary>A conch shell.</summary>
        public static readonly ItemDefinition Conch = new ItemDefinition("conch", "Conch", "a conch", "conches");

        /// <summary>A coconut.</summary>
        public static readonly ItemDefinition Coconut = new ItemDefinition("coconut", "Coconut", "a coconut", "coconuts");

        /// <summary>A small palm tree.</summary>
        public static readonly ZoneFeatureDefinition SmallPalmTree = new ZoneFeatureDefinition("small-palm-tree", "Small palm tree", "a small palm tree", "small palm trees", ZoneFeaturePermanence.Permanent);

        /// <summary>A medium palm tree.</summary>
        public static readonly ZoneFeatureDefinition MediumPalmTree = new ZoneFeatureDefinition("medium-palm-tree", "Medium palm tree", "a medium palm tree", "medium palm trees", ZoneFeaturePermanence.Permanent);

        /// <summary>A large palm tree.</summary>
        public static readonly ZoneFeatureDefinition LargePalmTree = new ZoneFeatureDefinition("large-palm-tree", "Large palm tree", "a large palm tree", "large palm trees", ZoneFeaturePermanence.Permanent);

        /// <summary>A bush.</summary>
        public static readonly ZoneFeatureDefinition Bush = new ZoneFeatureDefinition("beach-bush", "Bush", "a bush", "bushes", ZoneFeaturePermanence.Permanent);

        /// <summary>A rock outcrop: a resource node for stone.</summary>
        public static readonly ZoneFeatureDefinition RockOutcrop = new ZoneFeatureDefinition("rock-outcrop", "Rock outcrop", "a rock outcrop", "rock outcrops", ZoneFeaturePermanence.Permanent);

        /// <summary>A tide pool.</summary>
        public static readonly ZoneFeatureDefinition TidePool = new ZoneFeatureDefinition("tide-pool", "Tide pool", "a tide pool", "tide pools", ZoneFeaturePermanence.Permanent);

        /// <summary>A pile of driftwood.</summary>
        public static readonly ZoneFeatureDefinition DriftwoodPile = new ZoneFeatureDefinition("driftwood-pile", "Driftwood pile", "a pile of driftwood", "piles of driftwood", ZoneFeaturePermanence.SemiPermanent);

        /// <summary>A puddle.</summary>
        public static readonly ZoneFeatureDefinition Puddle = new ZoneFeatureDefinition("puddle", "Puddle", "a puddle", "puddles", ZoneFeaturePermanence.SemiPermanent);

        /// <summary>The Beach: 24 explores to map (Docs/Design/Systems/Exploration.md, Pace).</summary>
        public static readonly ZoneDefinition Beach = new ZoneDefinition(
            "beach",
            "Beach",
            exploresToMap: 24,
            findPool: new WeightedTable<ItemDefinition>(new[]
            {
                new WeightedTableEntry<ItemDefinition>(Stone, 3),
                new WeightedTableEntry<ItemDefinition>(Stick, 3),
                new WeightedTableEntry<ItemDefinition>(SeaweedStrand, 2),
                new WeightedTableEntry<ItemDefinition>(Seashell, 2),
                new WeightedTableEntry<ItemDefinition>(Conch, 1),
                new WeightedTableEntry<ItemDefinition>(Coconut, 1),
            }),
            featurePool: new WeightedTable<ZoneFeatureDefinition>(new[]
            {
                new WeightedTableEntry<ZoneFeatureDefinition>(SmallPalmTree, 3),
                new WeightedTableEntry<ZoneFeatureDefinition>(MediumPalmTree, 2),
                new WeightedTableEntry<ZoneFeatureDefinition>(LargePalmTree, 1),
                new WeightedTableEntry<ZoneFeatureDefinition>(Bush, 2),
                new WeightedTableEntry<ZoneFeatureDefinition>(RockOutcrop, 1),
                new WeightedTableEntry<ZoneFeatureDefinition>(TidePool, 1),
                new WeightedTableEntry<ZoneFeatureDefinition>(DriftwoodPile, 2),
                new WeightedTableEntry<ZoneFeatureDefinition>(Puddle, 2),
            }),
            scriptedFinds: new List<IReadOnlyList<ItemDefinition>>
            {
                new[] { Stone, Stone, Stick },
                new[] { Stick, Stick, SeaweedStrand },
                new[] { SeaweedStrand, Seashell },
                new[] { Stone, Coconut },
            });
    }
}
