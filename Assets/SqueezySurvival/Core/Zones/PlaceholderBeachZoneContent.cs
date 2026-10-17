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
        public static readonly ItemDefinition Stone = new ItemDefinition("stone", "Stone", "a stone", "stones", "A fist-sized stone, smooth from the surf. Good for throwing, or for knapping into a tool.", 1.0m);

        /// <summary>A stick.</summary>
        public static readonly ItemDefinition Stick = new ItemDefinition("stick", "Stick", "a stick", "sticks", "A dry stick, about as long as your arm.", 0.5m);

        /// <summary>A strand of seaweed: the Beach's fibrous plant.</summary>
        public static readonly ItemDefinition SeaweedStrand = new ItemDefinition("seaweed-strand", "Seaweed strand", "a strand of seaweed", "strands of seaweed", "A long, rubbery strand of kelp. Its fibers could be twisted into cord.", 0.2m);

        /// <summary>A seashell.</summary>
        public static readonly ItemDefinition Seashell = new ItemDefinition("seashell", "Seashell", "a seashell", "seashells", "A small, ridged shell. Pretty, and sharp along its broken edge.", 0.1m);

        /// <summary>A conch shell.</summary>
        public static readonly ItemDefinition Conch = new ItemDefinition("conch", "Conch", "a conch", "conches", "A heavy spiral shell. The inside is smooth and pink.", 0.75m);

        /// <summary>A coconut.</summary>
        public static readonly ItemDefinition Coconut = new ItemDefinition("coconut", "Coconut", "a coconut", "coconuts", "A coconut in its husk. There is water inside, if you can get at it.", 3.0m);

        /// <summary>Driftwood chunk: placeholder beach find.</summary>
        public static readonly ItemDefinition DriftwoodChunk = new ItemDefinition("driftwood-chunk", "Driftwood chunk", "a chunk of driftwood", "chunks of driftwood", "A sun-bleached chunk of wood, light and dry.", 1.5m);

        /// <summary>Palm frond: placeholder beach find.</summary>
        public static readonly ItemDefinition PalmFrond = new ItemDefinition("palm-frond", "Palm frond", "a palm frond", "palm fronds", "A long, fallen palm leaf. Good for weaving or thatch.", 0.75m);

        /// <summary>Feather: placeholder beach find.</summary>
        public static readonly ItemDefinition Feather = new ItemDefinition("feather", "Feather", "a feather", "feathers", "A gull feather, gray and white.", 0.01m);

        /// <summary>Crab shell: placeholder beach find.</summary>
        public static readonly ItemDefinition CrabShell = new ItemDefinition("crab-shell", "Crab shell", "a crab shell", "crab shells", "The empty shell of a crab. Something got to it first.", 0.1m);

        /// <summary>Sea glass: placeholder beach find.</summary>
        public static readonly ItemDefinition SeaGlass = new ItemDefinition("sea-glass", "Sea glass", "a piece of sea glass", "pieces of sea glass", "A frosted green shard, worn smooth by the waves.", 0.05m);

        /// <summary>Sand dollar: placeholder beach find.</summary>
        public static readonly ItemDefinition SandDollar = new ItemDefinition("sand-dollar", "Sand dollar", "a sand dollar", "sand dollars", "A flat, round shell with a five-petaled pattern.", 0.05m);

        /// <summary>Starfish: placeholder beach find.</summary>
        public static readonly ItemDefinition Starfish = new ItemDefinition("starfish", "Starfish", "a starfish", "starfish", "A dried starfish, stiff and rough.", 0.1m);

        /// <summary>Pumice: placeholder beach find.</summary>
        public static readonly ItemDefinition Pumice = new ItemDefinition("pumice", "Pumice", "a lump of pumice", "lumps of pumice", "A light, porous stone that floats.", 0.25m);

        /// <summary>Coral fragment: placeholder beach find.</summary>
        public static readonly ItemDefinition CoralFragment = new ItemDefinition("coral-fragment", "Coral fragment", "a coral fragment", "coral fragments", "A branch of white coral, brittle and sharp.", 0.2m);

        /// <summary>Sea sponge: placeholder beach find.</summary>
        public static readonly ItemDefinition SeaSponge = new ItemDefinition("sea-sponge", "Sea sponge", "a sea sponge", "sea sponges", "A dried sponge. It would soak up water again.", 0.1m);

        /// <summary>Cuttlebone: placeholder beach find.</summary>
        public static readonly ItemDefinition Cuttlebone = new ItemDefinition("cuttlebone", "Cuttlebone", "a cuttlebone", "cuttlebones", "A chalky, oval bone from a cuttlefish.", 0.05m);

        /// <summary>Rope scrap: placeholder beach find.</summary>
        public static readonly ItemDefinition RopeScrap = new ItemDefinition("rope-scrap", "Rope scrap", "a scrap of rope", "scraps of rope", "A frayed length of rope, tangled with seaweed.", 0.3m);

        /// <summary>Glass float: placeholder beach find.</summary>
        public static readonly ItemDefinition GlassFloat = new ItemDefinition("glass-float", "Glass float", "a glass float", "glass floats", "A hollow glass ball, once part of a fishing net.", 1.0m);

        /// <summary>Bamboo segment: placeholder beach find.</summary>
        public static readonly ItemDefinition BambooSegment = new ItemDefinition("bamboo-segment", "Bamboo segment", "a bamboo segment", "bamboo segments", "A hollow length of bamboo, washed in from somewhere.", 0.5m);

        /// <summary>A small palm tree.</summary>
        public static readonly ZoneFeatureDefinition SmallPalmTree = new ZoneFeatureDefinition("small-palm-tree", "Small palm tree", "a small palm tree", "small palm trees", "A young palm, barely taller than you.", ZoneFeaturePermanence.Permanent);

        /// <summary>A medium palm tree.</summary>
        public static readonly ZoneFeatureDefinition MediumPalmTree = new ZoneFeatureDefinition("medium-palm-tree", "Medium palm tree", "a medium palm tree", "medium palm trees", "A palm with a few coconuts high in its crown.", ZoneFeaturePermanence.Permanent);

        /// <summary>A large palm tree.</summary>
        public static readonly ZoneFeatureDefinition LargePalmTree = new ZoneFeatureDefinition("large-palm-tree", "Large palm tree", "a large palm tree", "large palm trees", "A tall, leaning palm heavy with coconuts.", ZoneFeaturePermanence.Permanent);

        /// <summary>A bush.</summary>
        public static readonly ZoneFeatureDefinition Bush = new ZoneFeatureDefinition("beach-bush", "Bush", "a bush", "bushes", "A tough, salt-tolerant shrub.", ZoneFeaturePermanence.Permanent);

        /// <summary>A rock outcrop: a resource node for stone.</summary>
        public static readonly ZoneFeatureDefinition RockOutcrop = new ZoneFeatureDefinition("rock-outcrop", "Rock outcrop", "a rock outcrop", "rock outcrops", "Weathered rock breaking through the sand. Loose stones lie around it.", ZoneFeaturePermanence.Permanent);

        /// <summary>A tide pool.</summary>
        public static readonly ZoneFeatureDefinition TidePool = new ZoneFeatureDefinition("tide-pool", "Tide pool", "a tide pool", "tide pools", "A pool of seawater left behind by the tide.", ZoneFeaturePermanence.Permanent);

        /// <summary>A pile of driftwood.</summary>
        public static readonly ZoneFeatureDefinition DriftwoodPile = new ZoneFeatureDefinition("driftwood-pile", "Driftwood pile", "a pile of driftwood", "piles of driftwood", "Sun-bleached wood washed up by the sea. The next storm may take it.", ZoneFeaturePermanence.SemiPermanent);

        /// <summary>A puddle.</summary>
        public static readonly ZoneFeatureDefinition Puddle = new ZoneFeatureDefinition("puddle", "Puddle", "a puddle", "puddles", "Rainwater collected in a hollow. It will dry up in the sun.", ZoneFeaturePermanence.SemiPermanent);

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
                new WeightedTableEntry<ItemDefinition>(DriftwoodChunk, 2),
                new WeightedTableEntry<ItemDefinition>(PalmFrond, 2),
                new WeightedTableEntry<ItemDefinition>(Feather, 1),
                new WeightedTableEntry<ItemDefinition>(CrabShell, 1),
                new WeightedTableEntry<ItemDefinition>(SeaGlass, 1),
                new WeightedTableEntry<ItemDefinition>(SandDollar, 1),
                new WeightedTableEntry<ItemDefinition>(Starfish, 1),
                new WeightedTableEntry<ItemDefinition>(Pumice, 1),
                new WeightedTableEntry<ItemDefinition>(CoralFragment, 1),
                new WeightedTableEntry<ItemDefinition>(SeaSponge, 1),
                new WeightedTableEntry<ItemDefinition>(Cuttlebone, 1),
                new WeightedTableEntry<ItemDefinition>(RopeScrap, 1),
                new WeightedTableEntry<ItemDefinition>(GlassFloat, 1),
                new WeightedTableEntry<ItemDefinition>(BambooSegment, 1),
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
