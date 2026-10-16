# Zones

**Status:** Draft

The world the survivor moves through.

## World

- **Setting:** a large island.
- **Biomes:** categories of environment, each with its own resource set. The beach is the first and only MVP biome.
- **Zones:** discrete locations, each belonging to one biome. Zones connect to one another through [zone connections](Exploration.md#what-exploring-can-reveal) found by exploring. Travelling to a connected zone is an action that costs time.
- **Weather** affects the survivor and the environment (Weather, planned: [OQ-009](../OpenQuestions.md)).

The MVP island's size and layout: [OQ-008](../OpenQuestions.md).

## Ground pool

Each zone has a ground pool: the items lying on the ground there. Items enter it from [exploration](Exploration.md), harvesting [resource nodes](ResourceNodes.md), and anything the survivor puts down. Items in a zone's ground pool are not in the survivor's inventory and stay in that zone.

### Capacity

The ground pool has no capacity limit (resolves OQ-028). Items leave it only by:

| Mechanism | Affects |
|---|---|
| The survivor picking them up or using them | Anything |
| [Decay](Decay.md) | Everything; organic items within days, inorganic items only after a very long time |
| Extreme weather events (floods, storms, landslides and similar) | Anything |
