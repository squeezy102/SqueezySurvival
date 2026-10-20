using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// The Beach's finds and features (Docs/Design/Content/BeachFinds.md) until zone content is final. Weights and
    /// how often each is found are placeholders until the balancing pass (OQ-056).
    /// </summary>
    public static class PlaceholderBeachZoneContent
    {
        /// <summary>Stone: a Beach find.</summary>
        public static readonly ItemDefinition Stone = new ItemDefinition("stone", "Stone", "a stone", "stones", "A small, ordinary stone. Good for knapping, campfires and much else; it may break when knapped.", ItemCategory.StoneAndMinerals, 1.0m);

        /// <summary>Big rock: a Beach find.</summary>
        public static readonly ItemDefinition BigRock = new ItemDefinition("big-rock", "Big rock", "a big rock", "big rocks", "A large, heavy rock. Stones can be knapped against it without it breaking.", ItemCategory.StoneAndMinerals, 15.0m);

        /// <summary>Small stick: a Beach find.</summary>
        public static readonly ItemDefinition SmallStick = new ItemDefinition("small-stick", "Small stick", "a small stick", "small sticks", "A branch about as thick as your thumb. Kindling, fuel, traps and hand tools.", ItemCategory.Wood, 0.25m);

        /// <summary>Big stick: a Beach find.</summary>
        public static readonly ItemDefinition BigStick = new ItemDefinition("big-stick", "Big stick", "a big stick", "big sticks", "A limb about as thick as your wrist. Firewood, heavier tools and building.", ItemCategory.Wood, 2.0m);

        /// <summary>Log: a Beach find.</summary>
        public static readonly ItemDefinition Log = new ItemDefinition("log", "Log", "a log", "logs", "Part of a tree trunk, washed up or fallen. Heavy, and good for building.", ItemCategory.Wood, 30.0m);

        /// <summary>Seaweed: a Beach find.</summary>
        public static readonly ItemDefinition Seaweed = new ItemDefinition("seaweed", "Seaweed", "some seaweed", "seaweed", "A rubbery tangle of seaweed. Edible, and its fibers make basic cordage.", ItemCategory.PlantMatter, 0.2m);

        /// <summary>Coconut: a Beach find.</summary>
        public static readonly ItemDefinition Coconut = new ItemDefinition("coconut", "Coconut", "a coconut", "coconuts", "A coconut in its husk. Food and water, if you can get at them; too much will upset your stomach.", ItemCategory.Fruit, 3.0m);

        /// <summary>Palm frond: a Beach find.</summary>
        public static readonly ItemDefinition PalmFrond = new ItemDefinition("palm-frond", "Palm frond", "a palm frond", "palm fronds", "A long, fallen palm leaf. Good for weaving baskets, bedding and shelter.", ItemCategory.PlantMatter, 0.75m);

        /// <summary>Seashell: a Beach find.</summary>
        public static readonly ItemDefinition Seashell = new ItemDefinition("seashell", "Seashell", "a seashell", "seashells", "A small, ridged shell. Pretty, and crushes into a calcium-rich powder.", ItemCategory.Shells, 0.1m);

        /// <summary>Crab: a Beach find.</summary>
        public static readonly ItemDefinition Crab = new ItemDefinition("crab", "Crab", "a crab", "crabs", "A crab scuttling across the sand. Delicious, better cooked; it spoils quickly once caught.", ItemCategory.Animals, 0.5m);

        /// <summary>Lobster: a Beach find.</summary>
        public static readonly ItemDefinition Lobster = new ItemDefinition("lobster", "Lobster", "a lobster", "lobsters", "A lobster, rarer than crabs. Delicious cooked, but best not eaten raw. It spoils quickly.", ItemCategory.Animals, 1.5m);

        /// <summary>Conch: a Beach find.</summary>
        public static readonly ItemDefinition Conch = new ItemDefinition("conch", "Conch", "a conch", "conches", "A heavy spiral shell. There may be something edible inside.", ItemCategory.Shells, 0.75m);

        /// <summary>Clam: a Beach find.</summary>
        public static readonly ItemDefinition Clam = new ItemDefinition("clam", "Clam", "a clam", "clams", "A closed clam. It may hold something edible, and very rarely a pearl.", ItemCategory.Shells, 0.3m);

        /// <summary>Oyster: a Beach find.</summary>
        public static readonly ItemDefinition Oyster = new ItemDefinition("oyster", "Oyster", "an oyster", "oysters", "A rough, closed oyster. It may hold something edible, and very rarely a pearl.", ItemCategory.Shells, 0.2m);

        /// <summary>Gooseneck barnacle: a Beach find.</summary>
        public static readonly ItemDefinition GooseneckBarnacle = new ItemDefinition("gooseneck-barnacle", "Gooseneck barnacle", "a gooseneck barnacle", "gooseneck barnacles", "A rare delicacy clinging to the rocks. You will need a sharp tool to harvest it.", ItemCategory.MeatAndFish, 0.1m);

        /// <summary>Coral: a Beach find.</summary>
        public static readonly ItemDefinition Coral = new ItemDefinition("coral", "Coral", "a piece of coral", "pieces of coral", "A piece of dried, brittle coral. Not much use except crushed.", ItemCategory.StoneAndMinerals, 0.2m);

        /// <summary>Driftwood: a Beach find.</summary>
        public static readonly ItemDefinition Driftwood = new ItemDefinition("driftwood", "Driftwood", "a piece of driftwood", "pieces of driftwood", "Sun-bleached, crooked wood. Splits into sticks, or makes a crude shield.", ItemCategory.Wood, 3.0m);

        /// <summary>Bone: a Beach find.</summary>
        public static readonly ItemDefinition Bone = new ItemDefinition("bone", "Bone", "a bone", "bones", "A weathered bone. Ground into bone meal, or shaped into tools.", ItemCategory.AnimalParts, 0.2m);

        /// <summary>Feather: a Beach find.</summary>
        public static readonly ItemDefinition Feather = new ItemDefinition("feather", "Feather", "a feather", "feathers", "A gull feather, gray and white. Fletching, bedding, kindling.", ItemCategory.AnimalParts, 0.01m);

        /// <summary>Cone snail: a Beach find.</summary>
        public static readonly ItemDefinition ConeSnail = new ItemDefinition("cone-snail", "Cone snail", "a cone snail", "cone snails", "A beautiful patterned shell. You have heard these are very, very dangerous. Best to leave it alone.", ItemCategory.Animals, 0.1m);

        /// <summary>Sea sponge: a Beach find.</summary>
        public static readonly ItemDefinition SeaSponge = new ItemDefinition("sea-sponge", "Sea sponge", "a sea sponge", "sea sponges", "A dried, brittle sponge. It soaks up liquids and helps with washing and cleaning wounds.", ItemCategory.AnimalParts, 0.1m);

        /// <summary>Sand dollar: a Beach find.</summary>
        public static readonly ItemDefinition SandDollar = new ItemDefinition("sand-dollar", "Sand dollar", "a sand dollar", "sand dollars", "A flat, round shell with a five-petaled pattern. Inedible; crushes into powder.", ItemCategory.Shells, 0.05m);

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
                new WeightedTableEntry<ItemDefinition>(Stone, 30),
                new WeightedTableEntry<ItemDefinition>(BigRock, 6),
                new WeightedTableEntry<ItemDefinition>(SmallStick, 30),
                new WeightedTableEntry<ItemDefinition>(BigStick, 12),
                new WeightedTableEntry<ItemDefinition>(Log, 1),
                new WeightedTableEntry<ItemDefinition>(Seaweed, 15),
                new WeightedTableEntry<ItemDefinition>(Coconut, 10),
                new WeightedTableEntry<ItemDefinition>(PalmFrond, 12),
                new WeightedTableEntry<ItemDefinition>(Seashell, 15),
                new WeightedTableEntry<ItemDefinition>(Crab, 6),
                new WeightedTableEntry<ItemDefinition>(Lobster, 2),
                new WeightedTableEntry<ItemDefinition>(Conch, 5),
                new WeightedTableEntry<ItemDefinition>(Clam, 6),
                new WeightedTableEntry<ItemDefinition>(Oyster, 5),
                new WeightedTableEntry<ItemDefinition>(GooseneckBarnacle, 1),
                new WeightedTableEntry<ItemDefinition>(Coral, 6),
                new WeightedTableEntry<ItemDefinition>(Driftwood, 8),
                new WeightedTableEntry<ItemDefinition>(Bone, 4),
                new WeightedTableEntry<ItemDefinition>(Feather, 8),
                new WeightedTableEntry<ItemDefinition>(ConeSnail, 1),
                new WeightedTableEntry<ItemDefinition>(SeaSponge, 3),
                new WeightedTableEntry<ItemDefinition>(SandDollar, 5),
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
                new[] { Stone, Stone, SmallStick },
                new[] { SmallStick, SmallStick, Seaweed },
                new[] { Seaweed, Seashell },
                new[] { Stone, Coconut },
            });
    }
}
